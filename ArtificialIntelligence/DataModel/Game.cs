using ArtificialIntelligence.DataModel.Interfaces;

namespace ArtificialIntelligence.DataModel;

/// <summary>
/// <inheritdoc cref="IGame"/>
/// </summary>
public class Game : IGame
{
    /// <summary>
    /// <inheritdoc cref="IGame.Board"/>
    /// </summary>
    public IBoard Board { get; set; }
    
    /// <summary>
    /// <inheritdoc cref="IGame.State"/>
    /// </summary>
    public GameState State { get; private set; }
    
    /// <summary>
    /// <inheritdoc cref="IGame.UpdateGameState"/>
    /// </summary>
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