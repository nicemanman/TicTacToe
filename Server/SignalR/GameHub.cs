using Microsoft.AspNetCore.SignalR;

namespace Server.SignalR;

public class GameHub : Hub
{
	public async Task JoinSession(string sessionId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
	}

	public async Task LeaveSession(string sessionId)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
	}
}