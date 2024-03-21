using Microsoft.AspNetCore.Mvc;
using Server.Services;
using Server.Services.Interfaces;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : Controller
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    [HttpPost]   
    public async Task<IActionResult> Create()
    {
        return Ok(await _gameService.CreateAsync());
    }
    
    [HttpGet]   
    public async Task<IActionResult> Get()
    {
        var games = await _gameService.GetAllGamesAsync();
        return Ok(games);
    }
}