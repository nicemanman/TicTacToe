using Localization.Common;
using Localization.Game;
using Microsoft.AspNetCore.Mvc;
using Server.DataModel;
using Server.DTO;
using Server.Services;
using Server.Services.Interfaces;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : Controller
{
    private readonly IGameService _gameService;
    private readonly ILogger<GameController> _logger;

    public GameController(IGameService gameService, ILogger<GameController> logger)
    {
        _gameService = gameService;
        _logger = logger;
    }
    
    [HttpPost]   
    public async Task<IActionResult> Create()
    {
        try
        {
            var game = await _gameService.CreateAsync();
            
            return Ok(new CreateGameSuccessResponse()
            {
                UUID = game.UUID,
                GameMap = new int[3,3]
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return StatusCode(500, new CreateGameErrorResponse()
            {
                Error = Errors.UnknownError
            });
        }
    }
    
    [HttpGet]   
    public async Task<IActionResult> Get(Guid uuid)
    {
        try
        {
            Game game = await _gameService.GetAsync(uuid);
            
            if (game == null)
                return Ok(new GetGameErrorResponse()
                {
                    Error = GameMessages.GameNotFound
                });
            
            return Ok(new GetGameSuccessResponse()
            {
                UUID = game.UUID,
                GameMap = new int[3,3]
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return StatusCode(500, new GetGameErrorResponse()
            {
                Error = Errors.UnknownError
            });
        }
    }
    
    [HttpPatch]   
    public async Task<IActionResult> Patch()
    {
        return Ok();
    }
}