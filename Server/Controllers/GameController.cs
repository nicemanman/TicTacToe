using AutoMapper;
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
    private readonly IMapper _mapper;

    public GameController(IGameService gameService, 
        ILogger<GameController> logger, 
        IMapper mapper)
    {
        _gameService = gameService;
        _logger = logger;
        _mapper = mapper;
    }
    
    [HttpPost]   
    public async Task<IActionResult> Create()
    {
        try
        {
            var game = await _gameService.CreateAsync();
            
            return Ok(new CreateGameSuccessResponse()
            {
                Game = _mapper.Map<Game, GameDTO>(game)
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
                Game = _mapper.Map<Game, GameDTO>(game)
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
    public async Task<IActionResult> MakeAMove(Guid uuid)
    {
        try
        {
            Game game = await _gameService.GetAsync(uuid);
            
            if (game == null)
                return Ok(new GetGameErrorResponse()
                {
                    Error = GameMessages.GameNotFound
                });
            
            return Ok(new MakeAMoveSuccessResponse()
            {
                Game = _mapper.Map<Game, GameDTO>(game)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            
            return StatusCode(500, new MakeAMoveErrorResponse()
            {
                Error = Errors.UnknownError
            });
        }
    }
}