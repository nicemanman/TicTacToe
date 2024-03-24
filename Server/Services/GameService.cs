using System.Security.Cryptography;
using Localization.Game;
using Server.AI;
using Server.Data.Interfaces;
using Server.DataModel;
using Server.DTO;
using Server.DTO.Results;
using Server.Services.Interfaces;
using ArtificialIntelligence.DataModel;
using Game = Server.DataModel.Game;
using GameState = Server.DataModel.GameState;

namespace Server.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOpponentManager _opponentManager;
    
    public GameService(IUnitOfWork unitOfWork, IOpponentManager opponentManager)
    {
        _unitOfWork = unitOfWork;
        _opponentManager = opponentManager;
    }

    public async Task<CreateGameResult> CreateAsync(bool playerFirst)
    {
        Game game = new Game()
        {
            GameMap = new GameMap(3,3)
        };
        
        if (!playerFirst)
            game = _opponentManager.MakeMove(game);
        
        game = await _unitOfWork.GamesRepository.CreateAsync(game);
        
        CreateGameResult result = new()
        {
            Game = game,
            Message = GameMessages.YourTurn
        };
        
        return result;
    }

    public async Task<GetGameResult> GetAsync(Guid uuid)
    {
        var game = await _unitOfWork.GamesRepository.GetAsync(uuid);

        if (game == null)
            return new GetGameResult()
            {
                ErrorMessage = GameMessages.GameNotFound
            };
        
        string message = string.Empty;
        
        if (game.State == GameState.Tie) 
            message = GameMessages.GameFinished_ItsATie;
        
        if (game.State == GameState.PlayerWin) 
            message = GameMessages.GameFinished_PlayerWin;
        
        if (game.State == GameState.BotWin) 
            message = GameMessages.GameFinished_BotWin;
        
        GetGameResult result = new()
        {
            Game = game,
            Message = message
        };

        return result;
    }

    public async Task<MakeAMoveResult> MakeAMoveAsync(Game game, int row, int column)
    {
        var field = game.GameMap.Fields.FirstOrDefault(x => x.Row == row && x.Column == column);

        if (field == null)
            return new MakeAMoveResult()
            {
                ErrorMessage = GameMessages.UnableToSetCell_UnknownCell
            };

        if (!string.IsNullOrWhiteSpace(field.Char))
            return new MakeAMoveResult()
            {
                ErrorMessage = GameMessages.UnableToSetCell_AlreadySet
            };
        
        //TODO: не стоит хардкодить, времени мало, потом можно будет доделать
        field.Char = "X";
        
        game = _opponentManager.MakeMove(game);
        game = await _unitOfWork.GamesRepository.UpdateAsync(game);
        
        if (!TryGetWinnerMessage(game, out string message))
            return new MakeAMoveResult()
            {
                Game = game
            };

        return new MakeAMoveResult()
        {
            Game = game,
            Message = message
        };
    }

    private static bool TryGetWinnerMessage(Game game, out string message)
    {
        message = string.Empty;
        
        if (!game.IsFinished)
            return false;
        
        if (game.State == GameState.Tie)
        {
            message = GameMessages.GameFinished_ItsATie;
            return true;
        }
        
        if (game.State == GameState.PlayerWin)
        {
            message = GameMessages.GameFinished_PlayerWin;
            return true;
        }
        
        if (game.State == GameState.BotWin)
        {
            message = GameMessages.GameFinished_BotWin;
            return true;
        }

        return false;
    }
}