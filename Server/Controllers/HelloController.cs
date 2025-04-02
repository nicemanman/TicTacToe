using MessageQueue.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hey");
    }
}