namespace MessageQueue.DataModel;

public class RabbitMessage
{
	public string SenderId { get; init; }
	
	public string ReceiverId { get; init; }
	
	public object Payload { get; init; }
	
	public MessageType MessageType { get; init; }
}

public enum MessageType
{
	
}