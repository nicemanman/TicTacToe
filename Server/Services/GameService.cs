using Server.Data.Interfaces;
using Server.DataModel;
using Server.Services.Interfaces;

namespace Server.Services;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;

    public GameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Game> CreateAsync()
    {
        return await _unitOfWork.GamesRepository.CreateAsync(new Game()
        {
            GameMap = new GameMap(3,3)
        });
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        return await _unitOfWork.GamesRepository.GetAllAsync();
    }

    public async Task<Game> GetAsync(Guid uuid)
    {
        return await _unitOfWork.GamesRepository.GetAsync(uuid);
    }
}