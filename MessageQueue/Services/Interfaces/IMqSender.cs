using MessageQueue.DataModel;
using RabbitMQ.Client.Events;

namespace MessageQueue.Services.Interfaces;

public interface IMqClient
{
	Task Subscribe(string userId, Func<RabbitMessage, Task> onMessage);
	
	public Task Send(RabbitMessage t);
}