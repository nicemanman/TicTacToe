using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers.TestControllers;

[ApiController]
[Route("api/test/[controller]")]
public class HelloController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        string name = HttpContext.Session.GetString(TicTacToeConstants.UserNameField)!;
        return Ok($"Привет, {name}");
    }
}