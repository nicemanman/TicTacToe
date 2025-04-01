using Database.Interfaces;

namespace Server.DataModel;

public class Game : IEntity
{
    public Guid UUID { get; set; }
    
    public GameMap GameMap { get; init; }

    public GameState State { get; init; } = GameState.InProgress;

    public bool IsFinished => State is GameState.Tie or GameState.OpponentWin or GameState.PlayerWin;
    
    public uint CreatorId { get; init; }
    
    public uint OpponentId { get; init; }
    
    public bool IsOpponentTurn { get; init; }
    
    public bool OpponentIsBot { get; init; }
}