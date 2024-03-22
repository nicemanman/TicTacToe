using Server.AI;
using Server.Data.Interfaces;
using Server.DataModel;
using Server.DTO;
using Server.Services.Interfaces;
using TicTacToeAI.DataModel;
using Game = Server.DataModel.Game;
using GameState = Server.DataModel.GameState;

namespace Server.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAiManager _aiManager;
    
    public GameService(IUnitOfWork unitOfWork, IAiManager aiManager)
    {
        _unitOfWork = unitOfWork;
        _aiManager = aiManager;
    }

    public async Task<Game> CreateAsync()
    {
        Game game = new Game()
        {
            GameMap = new GameMap(3,3)
        };
        
        game = _aiManager.MakeMove(game);
        game = await _unitOfWork.GamesRepository.CreateAsync(game);
        return game;
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        return await _unitOfWork.GamesRepository.GetAllAsync();
    }

    public async Task<Game> GetAsync(Guid uuid)
    {
        return await _unitOfWork.GamesRepository.GetAsync(uuid);
    }

    public async Task<MakeAMoveResult> MakeAMoveAsync(Game game, int row, int column)
    {
        var field = game.GameMap.Fields.FirstOrDefault(x => x.Row == row && x.Column == column);

        if (field == null)
            throw new Exception($"Невозможно установить выбор в поле {row}:{column}: поле с таким индексом отсутствует");
        
        if (!string.IsNullOrWhiteSpace(field.Char))
            throw new Exception($"Невозможно установить выбор в поле {row}:{column}: выбор в этом поле уже сделан");
        
        //TODO: не стоит хардкодить, времени мало, потом можно будет доделать
        field.Char = "X";
        
        game = _aiManager.MakeMove(game);

        MakeAMoveResult result = new MakeAMoveResult()
        {
            Game = game
        };

        await _unitOfWork.GamesRepository.UpdateAsync(game);
        
        if (!game.IsFinished)
            return result;

        if (game.State == GameState.Tie) 
            result.Message = "Результат матча - ничья";
        
        if (game.State == GameState.PlayerWin) 
            result.Message = "Результат матча - побебил игрок";
        
        if (game.State == GameState.BotWin) 
            result.Message = "Результат матча - побебил бот";

        return result;
    }
}