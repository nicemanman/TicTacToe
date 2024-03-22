using TicTacToeAI.DataModel.Interfaces;

namespace TicTacToeAI.DataModel;

public interface IGame
{
    public IBoard Board { get; set; }

    public GameState State { get; }
    
    void UpdateGameState();
}