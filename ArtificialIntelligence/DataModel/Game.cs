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
    
    public List<(int, int)> WinningCells { get; set; }
    
    /// <summary>
    /// <inheritdoc cref="IGame.UpdateGameState"/>
    /// </summary>
    public void UpdateGameState()
    {
        WinningCells = [];
        if (Board.CheckIfPlayerHasRowsSet(out var playerCells))
        {
            State = GameState.PlayerWin;
            WinningCells.AddRange(playerCells);
            return;
        }

        if (Board.CheckIfOpponentHasRowsSet(out var botCells))
        {
            State = GameState.BotWin;
            WinningCells.AddRange(botCells);
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