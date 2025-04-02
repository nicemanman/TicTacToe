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

public class BackgroundJob(IOptions<RabbitOptions> options) : IHostedService, IMqSender, IMqReceiver
{
	private Queue<RabbitMessage> _in = new ();
	private BufferBlock<RabbitMessage> _out = new ();
	private IChannel _channel;
	private IConnection _connection;
	private AsyncEventingBasicConsumer _consumer;
	
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
		
		await _channel.QueueDeclareAsync(
			queue: "hello", 
			durable: false, 
			exclusive: false, 
			autoDelete: false,
			arguments: null);
		
		_consumer = new AsyncEventingBasicConsumer(_channel);
		_consumer.ReceivedAsync += (model, ea) =>
		{
			var body = ea.Body.ToArray();
			var serializedMessage = Encoding.UTF8.GetString(body);
			var deserializedMessage = JsonConvert.DeserializeObject<RabbitMessage>(serializedMessage);
			
			_in.Enqueue(deserializedMessage);
			
			return Task.CompletedTask;
		};
		
		await _channel.BasicConsumeAsync("hello", autoAck: true, consumer: _consumer, cancellationToken: cancellationToken);
		//Task.Run(() => ProcessOutMessages(cancellationToken), cancellationToken);
	}

	public async Task StopAsync(CancellationToken cancellationToken)
	{
		await _connection.DisposeAsync();
		await _channel.DisposeAsync();
	}

	async Task IMqSender.Send<T>(T t)
	{
		var serializedMessage = JsonConvert.SerializeObject(t);
		
		var message = new RabbitMessage()
		{
			Payload = serializedMessage
		};
		
		await Send(CancellationToken.None, message);
	}

	private async Task ProcessOutMessages(CancellationToken token)
	{
		while (true)
		{
			if (token.IsCancellationRequested)
				return;

			var message = await _out.ReceiveAsync(token);
			await Send(token, message);
		}									
	}

	private async Task Send(CancellationToken token, RabbitMessage message)
	{
		var serializedMessage = JsonConvert.SerializeObject(message);
		var body = Encoding.UTF8.GetBytes(serializedMessage);
		await _channel.BasicPublishAsync(
			exchange: string.Empty, 
			routingKey: "hello", 
			body: body, 
			cancellationToken: token);
	}

	public Task<RabbitMessage> Receive()
	{
		return _in.TryDequeue(out var message) 
			? Task.FromResult(message) 
			: Task.FromResult<RabbitMessage>(null);
	}
}