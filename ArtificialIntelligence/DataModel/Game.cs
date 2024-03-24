using ArtificialIntelligence.DataModel.Interfaces;

namespace ArtificialIntelligence.DataModel;

public class Game : IGame
{
    public IBoard Board { get; set; }
    
    public GameState State { get; private set; }
    
    public void UpdateGameState()
    {
        if (Board.CheckIfPlayerHasRowsSet())
        {
            State = GameState.PlayerWin;
            return;
        }

        if (Board.CheckIfOpponentHasRowsSet())
        {
            State = GameState.BotWin;
            return;
        }

        if (!Board.IsPossibleToMakeAMove())
        {
            State = GameState.Tie;
            return;
        }

        State = GameState.InProgress;
    }
}