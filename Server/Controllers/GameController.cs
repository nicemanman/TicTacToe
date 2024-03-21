using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : Controller
{
    [HttpPost]   
    public async Task<IActionResult> CreateGame()
    {
        return Ok();
    }
}