using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentificationController : Controller
{
	[HttpPost]
	public IActionResult SetName(string username)
	{
		HttpContext.Session.SetString(TicTacToeConstants.UserNameField, username);
		return Ok();
	}
}