using Server.DataModel;
using Server.DTO;

namespace Server.Services.Interfaces;

public interface IGameService
{
    public Task<Game> CreateAsync();
    
    Task<List<Game>> GetAllGamesAsync();
    
    Task<Game> GetAsync(Guid uuid);
    
    Task<MakeAMoveResult> MakeAMoveAsync(Game game, int row, int column);
}