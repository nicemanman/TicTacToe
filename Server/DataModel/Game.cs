using Database.Interfaces;

namespace Server.DataModel;

public class Game : IEntity
{
    public Guid UUID { get; set; }
    
    public GameMap GameMap { get; init; }

    public List<CellCoord> WinningCells { get; init; } = [];
    
    public GameState State { get; set; }
}