using AutoMapper;
using Localization.Common;
using Localization.Game;
using Localization.GameSession;
using Microsoft.AspNetCore.Mvc;
using Server.DataModel;
using Server.DTO;
using Server.DTO.Responses;
using Server.Services.Interfaces;
using Game = Server.DataModel.Game;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : Controller
{
    private readonly IGameSessionManager _gameSessionManager;
    private readonly ILogger<GameController> _logger;
    private readonly IMapper _mapper;

    public GameController(IGameSessionManager gameSessionManager, 
        ILogger<GameController> logger, 
        IMapper mapper)
    {
        _gameSessionManager = gameSessionManager;
        _logger = logger;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Создать игровой раунд Крестики-нолики
    /// </summary>
    [HttpPost]   
    public async Task<IActionResult> Create(bool againstBot = false)
    {
        try
        {
            var userId = HttpContext.Session.GetString(TicTacToeConstants.UserIdField);
            
            var result = await _gameSessionManager.StartParty(userId, againstBot);
            
            if (result.Failure)
                return StatusCode(500, new CreateGameErrorResponse()
                {
                    Error = GameSessionMessages.SessionFailedAtCreation
                });
            
            var gameDto = _mapper.Map<GameSession, GameDTO>(result.Value);
            
            return Ok(new CreateGameSuccessResponse()
            {
                Game = gameDto
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
    
    [HttpPost(nameof(Join))]   
    public async Task<IActionResult> Join(string joinCode)
    {
        try
        {
            var userId = HttpContext.Session.GetString(TicTacToeConstants.UserIdField);
            
            var result = await _gameSessionManager.Join(userId, joinCode);
                
            if (result.Failure)
                return StatusCode(500, new CreateGameErrorResponse()
                {
                    Error = GameSessionMessages.JoiningError
                });
                
            var gameDto = _mapper.Map<GameSession, GameDTO>(result.Value);
            
            return Ok(new CreateGameSuccessResponse()
            {
                Game = gameDto
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
            var userId = HttpContext.Session.GetString(TicTacToeConstants.UserIdField);
            var result = await _gameSessionManager.FindActiveSession(userId);
            
            if (result.Failure)
                return Ok(new GetGameErrorResponse()
                {
                    Error = result.Error
                });
            
            if (result.Success && result.Value == null)
                return StatusCode(204);
            
            return Ok(new GetGameSuccessResponse()
            {
                Game = _mapper.Map<GameSession, GameDTO>(result.Value),
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
            var userId = HttpContext.Session.GetString(TicTacToeConstants.UserIdField);
            var session = await _gameSessionManager.FindActiveSession(userId);
            
            if (session.Failure)
                return Ok(new GetGameErrorResponse()
                {
                    Error = session.Error
                });
            
            if (session is { Success: true, Value: null })
                return Ok(new MakeAMoveErrorResponse()
                {
                    Error = GameMessages.GameNotFound
                });

            var makeAMoveResult = await _gameSessionManager.MakeMove(session.Value, userId, row, column);
            
            if (makeAMoveResult.Failure)
                return Ok(new MakeAMoveErrorResponse()
                {
                    Error = makeAMoveResult.Error
                });

            if (!makeAMoveResult.Value.IsGameFinished)
                return Ok(new MakeAMoveSuccessResponse()
                {
                    Game = _mapper.Map<GameSession, GameDTO>(makeAMoveResult.Value)
                });
            
            TryGetWinnerMessage(makeAMoveResult.Value.Game, out var outcome);
            
            return Ok(new MakeAMoveGameIsFinishedResponse()
            {
                Message = outcome
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
    
    private static bool TryGetWinnerMessage(Game game, out string message)
    {
        message = string.Empty;
        
        if (!game.IsFinished)
            return false;
        
        switch (game.State)
        {
            case GameState.Tie:
                message = GameMessages.GameFinished_ItsATie;
                return true;
            case GameState.Player1Win:
                message = GameMessages.GameFinished_PlayerWin;
                return true;
            case GameState.Player2Win:
                message = GameMessages.GameFinished_OpponentWin;
                return true;
            default:
                return false;
        }
    }
}