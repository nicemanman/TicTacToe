using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks.Dataflow;
using MessageQueue.DataModel;
using MessageQueue.Options;
using MessageQueue.Services.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MessageQueue.Hosted;

public class RabbitClient(IOptions<RabbitOptions> options) : IHostedService, IMqClient
{
	private IChannel _channel;
	private IConnection _connection;
	
	public async Task StartAsync(CancellationToken cancellationToken)
	{
		var factory = new ConnectionFactory()
		{
			HostName = options.Value.Hostname,
			UserName = options.Value.Username,
			Password = options.Value.Password
		};
		
		_connection = await factory.CreateConnectionAsync(cancellationToken);
		_channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

		await _channel.ExchangeDeclareAsync("game_exchange", ExchangeType.Direct, cancellationToken: cancellationToken);
	}

	public async Task Subscribe(string userId, Func<RabbitMessage, Task> onMessage)
	{
		await _channel.QueueDeclareAsync(userId,
			durable: false,
			exclusive: false,
			autoDelete: true);
		
		await _channel.QueueBindAsync(queue: userId, exchange: "game_exchange", routingKey: userId);
		var consumer = new AsyncEventingBasicConsumer(_channel);
		consumer.ReceivedAsync += async (_, ea) =>
		{
			var body = ea.Body.ToArray();
			var serializedMessage = Encoding.UTF8.GetString(body);
			var deserializedMessage = JsonConvert.DeserializeObject<RabbitMessage>(serializedMessage);
			await onMessage(deserializedMessage);
		};
		
		await _channel.BasicConsumeAsync(queue: userId, autoAck: true, consumer: consumer);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _connection.DisposeAsync();
		await _channel.DisposeAsync();
	}

	async Task IMqClient.Send(RabbitMessage message)
	{
		await Send(message, CancellationToken.None);
	}

	private async Task Send(RabbitMessage message, CancellationToken token)
	{
		var serializedMessage = JsonConvert.SerializeObject(message);
		var body = Encoding.UTF8.GetBytes(serializedMessage);
		await _channel.BasicPublishAsync(
			exchange: "game_exchange", 
			routingKey: message.ReceiverId, 
			body: body, 
			cancellationToken: token);
	}
}