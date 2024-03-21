using Server.DataModel;

namespace Server.Services.Interfaces;

public interface IGameService
{
    public Task<Game> CreateAsync();
    
    Task<List<Game>> GetAllGamesAsync();
}