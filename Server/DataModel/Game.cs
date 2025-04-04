using Database.Interfaces;

namespace Server.DataModel;

public class Game : IEntity
{
    public Guid UUID { get; set; }
    
    public GameMap GameMap { get; init; }

    public GameState State { get; init; } = GameState.InProgress;

    public bool IsFinished => State is GameState.Tie or GameState.Player2Win or GameState.Player1Win;
}