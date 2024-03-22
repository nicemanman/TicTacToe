using Database.Interfaces;
using TicTacToeAI.DataModel;

namespace Server.DataModel;

public class Game : IEntity
{
    public Guid UUID { get; set; }
    
    public GameMap GameMap { get; set; }

    public GameState State { get; set; }

    public bool IsFinished => State is GameState.Tie or GameState.BotWin or GameState.PlayerWin;
}