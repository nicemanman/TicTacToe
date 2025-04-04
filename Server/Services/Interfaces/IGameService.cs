using Server.DataModel;
using Server.DTO.Results;

namespace Server.Services.Interfaces;

public interface IGameService
{
    public Task<Result<Game>> CreateAsync();
    
    Task<Result<Game>> MakeAMoveAsync(Game game, int row, int column);
}