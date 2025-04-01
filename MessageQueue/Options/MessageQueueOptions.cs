namespace MessageQueue.Options;

public class MessageQueueOptions
{
	public static string SectionName = nameof(MessageQueueOptions);
	
	public string Hostname { get; init; }
	
	public string Username { get; init; }
	
	public string Password { get; init; }
}