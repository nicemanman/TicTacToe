using Server.DataModel;
using Server.DTO;
using Server.DTO.Results;

namespace Server.Services.Interfaces;

public interface IGameService
{
    public Task<CreateGameResult> CreateAsync(bool playerFirst);
    
    Task<GetGameResult> GetAsync(Guid uuid);
    
    Task<MakeAMoveResult> MakeAMoveAsync(Game game, int row, int column);
}