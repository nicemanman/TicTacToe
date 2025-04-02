using MessageQueue.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MqTestController 
	(IMqSender mqSender, IMqReceiver mqReceiver) : Controller
{
	[HttpPost]
	public async Task<IActionResult> SendTestMessage(string message = "Test Message")
	{
		await mqSender.Send(message);
		return Ok();
	}

	[HttpGet]
	public async Task<IActionResult> ReceiveTestMessage()
	{
		var result = await mqReceiver.Receive();
		return Ok(result.Payload);
	}
}