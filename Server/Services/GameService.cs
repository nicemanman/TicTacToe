using Localization.Game;
using Server.Data.Interfaces;
using Server.DataModel;
using Server.OpponentManager;
using Server.Services.Interfaces;
using Game = Server.DataModel.Game;

namespace Server.Services;

public class GameService(IUnitOfWork unitOfWork, IBotManager botManager) : IGameService
{
    public async Task<Result<Game>> CreateAsync()
    {
        Game game = new Game()
        {
            GameMap = new GameMap(3,3),
            State = GameState.InProgress
        };
        
        game = await unitOfWork.GamesRepository.CreateAsync(game);
        
        return Result.Ok(game);
    }

    public async Task<Result<Game>> MakeAMoveAsync(GameSession session, int row, int column, string ch)
    {
        var field = session.Game.GameMap.Fields.FirstOrDefault(x => x.Row == row && x.Column == column);

        if (field == null)
            return Result.Fail<Game>(GameMessages.UnableToSetCell_UnknownCell);

        if (!string.IsNullOrWhiteSpace(field.Char))
            return Result.Fail<Game>(GameMessages.UnableToSetCell_AlreadySet);
        
        field.Char = ch;
        
        if (session.Player2Id == TicTacToeConstants.BotId)
            session.Game = botManager.MakeMove(session.Game);
        
        session.Game = await unitOfWork.GamesRepository.UpdateAsync(session.Game);
        
        return Result.Ok(session.Game);
    }
}