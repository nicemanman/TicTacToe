namespace Server.DataModel;

public class MakeMoveNotificationMessage
{
	public Game Game { get; init; }
	
	public uint UserId { get; init; }
	
	public string SessionId { get; init; }
}