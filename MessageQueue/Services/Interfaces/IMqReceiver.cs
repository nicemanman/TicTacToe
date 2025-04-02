using MessageQueue.DataModel;

namespace MessageQueue.Services.Interfaces;

public interface IMqReceiver
{
	public Task<RabbitMessage> Receive();
}