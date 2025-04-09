namespace MessageQueue.Options;

public class RabbitOptions
{
	public static string SectionName = nameof(RabbitOptions);
	
	public string Hostname { get; init; }
	
	public string Username { get; init; }
	
	public string Password { get; init; }
}