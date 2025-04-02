namespace MessageQueue.Services.Interfaces;

public interface IMqSender
{
	public Task Send<T>(T t);
}