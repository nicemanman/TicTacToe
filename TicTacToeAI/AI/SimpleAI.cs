using TicTacToeAI.AI.Interfaces;
using TicTacToeAI.DataModel;
using TicTacToeAI.DataModel.Interfaces;

namespace TicTacToeAI.Algorithms;

public class SimpleAI : IAI
{
    /// <summary>
    /// Выбрать лучший ход
    /// </summary>
    /// <param name="board"></param>
    /// <returns></returns>
    public Turn GetNextBestMove(IBoard board)
    {
        int bestValue = -1000;
        Turn bestTurn = new()
        {
            Row = -1,
            Column = -1
        };
        
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (!board.IsEmpty(row, column)) 
                    continue;
                
                board.SetPlayer(row, column);
                int currentTurnValue = ComputeMinMax(board, 0, false);
                
                board.UndoMove(row, column);

                if (currentTurnValue <= bestValue) 
                    continue;
                
                bestTurn.Row = row;
                bestTurn.Column = column;
                bestValue = currentTurnValue;
            }
        }
        
        return bestTurn;
    }

    /// <summary>
    /// Выполнить MinMax алгоритм
    /// </summary>
    /// <param name="board"></param>
    /// <param name="depth"></param>
    /// <param name="isMax"></param>
    /// <returns></returns>
    private static int ComputeMinMax(IBoard board, int depth, bool isMax)
    {
        int score = board.GetCurrentScore();
        
        if (score == 10) 
            return score;
        
        if (score == -10) 
            return score;
        
        if (board.IsPossibleToMakeAMove() == false) 
            return 0;
        
        if (isMax)
        {
            int bestValue = -1000;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (!board.IsEmpty(row, col)) 
                        continue;
                    
                    board.SetPlayer(row, col);
                    bestValue = Math.Max(bestValue, ComputeMinMax(board, depth + 1, !isMax));
                    board.UndoMove(row, col);
                }
            }
            
            return bestValue;
        }
        else
        {
            int bestValue = 1000;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (!board.IsEmpty(row, col)) 
                        continue;
                    
                    board.SetOpponent(row, col);
                    bestValue = Math.Min(bestValue, ComputeMinMax(board, depth + 1, !isMax));
                    board.UndoMove(row, col);
                }
            }
            
            return bestValue;
        }
    }
}