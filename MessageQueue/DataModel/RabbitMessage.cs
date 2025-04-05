namespace MessageQueue.DataModel;

public class RabbitMessage
{
	public string SenderId { get; set; }
	public string ReceiverId { get; set; }
	
	public string SessionId { get; set; }
	
	public MessageType Type { get; set; }
	
	public MessageType MessageType { get; init; }
}

public enum MessageType
{
	Move
}