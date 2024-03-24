using AutoMapper;
using Localization.Common;
using Localization.Game;
using Microsoft.AspNetCore.Mvc;
using Server.DTO;
using Server.DTO.Responses;
using Server.DTO.Results;
using Server.Services;
using Server.Services.Interfaces;
using ArtificialIntelligence.DataModel;
using Game = Server.DataModel.Game;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : Controller
{
    private const string _currentGameToken = "CURRENT_GAME_TOKEN";
    
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
    
    /// <summary>
    /// Создать игровой раунд Крестики-нолики
    /// </summary>
    /// <param name="playerFirst">Будет ли игрок ходить первым</param>
    [HttpPost]   
    public async Task<IActionResult> Create(bool playerFirst = false)
    {
        try
        {
            var result = await _gameService.CreateAsync(playerFirst);
            var gameDto = _mapper.Map<Game, GameDTO>(result.Game);
            
            AddGameTokenToSession(result.Game);
            
            return Ok(new CreateGameSuccessResponse()
            {
                Game = gameDto,
                Message = result.Message
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
    
    /// <summary>
    /// Получить состояние текущего игрового раунда
    /// </summary>
    [HttpGet]   
    public async Task<IActionResult> Get()
    {
        try
        {
            GetGameResult result = await _gameService.GetAsync(GetGameTokenFromSession());
            
            if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
                return Ok(new GetGameErrorResponse()
                {
                    Error = result.ErrorMessage
                });
            
            return Ok(new GetGameSuccessResponse()
            {
                Game = _mapper.Map<Game, GameDTO>(result.Game),
                Message = result.Message
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

    /// <summary>
    /// Сделать ход в текущем игровом раунде Крестики-нолики
    /// </summary>
    [HttpPatch]   
    public async Task<IActionResult> MakeAMove(int row, int column)
    {
        try
        {
            GetGameResult getResult = await _gameService.GetAsync(GetGameTokenFromSession());
            
            if (!string.IsNullOrWhiteSpace(getResult.ErrorMessage))
                return Ok(new GetGameErrorResponse()
                {
                    Error = getResult.ErrorMessage
                });

            MakeAMoveResult makeAMoveResult = await _gameService.MakeAMoveAsync(getResult.Game, row, column);
            
            if (!string.IsNullOrWhiteSpace(makeAMoveResult.ErrorMessage))
                return Ok(new MakeAMoveErrorResponse()
                {
                    Error = makeAMoveResult.ErrorMessage
                });

            if (makeAMoveResult.GameIsFinished)
            {
                RemoveGameTokenFromSession();
                return Ok(new MakeAMoveGameIsFinishedResponse()
                {
                    Message = makeAMoveResult.Message
                });
            }
            
            return Ok(new MakeAMoveSuccessResponse()
            {
                Game = _mapper.Map<Game, GameDTO>(makeAMoveResult.Game)
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
    
    private Guid GetGameTokenFromSession()
    {
        string token = HttpContext.Session.GetString(_currentGameToken);
        
        if (string.IsNullOrWhiteSpace(token))
            return Guid.Empty;
        
        return new Guid(token);
    }
    
    private void AddGameTokenToSession(Game game)
    {
        HttpContext.Session.SetString(_currentGameToken, game.UUID.ToString());
    }
    
    private void RemoveGameTokenFromSession()
    {
        HttpContext.Session.Remove(_currentGameToken);
    }
}