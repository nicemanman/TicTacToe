using MessageQueue.DataModel;
using MessageQueue.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.TestControllers;

[ApiController]
[Route("api/test/[controller]")]
public class MqTestController 
	(IMqClient mqClient) : Controller
{
	[HttpPost]
	public async Task<IActionResult> SendTestMessage()
	{
		var userId = HttpContext.Session.GetString(TicTacToeConstants.UserIdField);
		await mqClient.Send(new RabbitMessage()
		{
			SenderId = userId,
			ReceiverId = userId,
			MessageType = MessageType.Move
		});
		
		return Ok();
	}
}