using ArtificialIntelligence.DataModel.Interfaces;

namespace ArtificialIntelligence.DataModel;

/// <summary>
/// <inheritdoc cref="IBoard"/>
/// </summary>
public class Board : IBoard
{
    private static readonly string _player = "X";
    private static readonly string _opponent = "O";
    private static readonly string _empty = "";
    private string[,] board = new string[3, 3];

    public Board(List<BoardCell> cells)
    {
        foreach (var cell in cells)
        {
            board[cell.Row, cell.Column] = cell.Char;
        }
    }
    
    public Board()
    {
        
    }

    public List<BoardCell> GetCells()
    {
        List<BoardCell> boardCells = new ();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                boardCells.Add(new BoardCell()
                {
                    Column = col,
                    Row = row,
                    Char = board[row, col]
                });
            }
        }

        return boardCells;
    }
    
    /// <summary>
    /// <inheritdoc cref="IBoard.IsEmpty"/>
    /// </summary>
    public bool IsEmpty(int row, int col)
    {
        if (row < 0 || col < 0)
            return false;
        
        return board[row, col] == _empty;
    }

    /// <summary>
    /// <inheritdoc cref="IBoard.GetCurrentScore"/>
    /// </summary>
    public int GetCurrentScore()
    {
        //Проверить строки
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != board[i, 1] || board[i, 1] != board[i, 2]) 
                continue;
                
            if (board[i, 0] == _player)
                return +10;
                    
            if (board[i, 0] == _opponent)
                return -10;
        }

        // Проверить столбцы
        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] != board[1, j] || board[1, j] != board[2, j]) 
                continue;
                
            if (board[0, j] == _player)
                return +10;

            if (board[0, j] == _opponent)
                return -10;
        }

        // Проверить диагональ
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == _player)
                return +10;
                
            if (board[0, 0] == _opponent)
                return -10;
        }
            
        // Проверить вторую диагональ
        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
        {
            if (board[0, 2] == _player)
                return +10;
                
            if (board[0, 2] == _opponent)
                return -10;
        }

        return 0;
    }

    /// <summary>
    /// <inheritdoc cref="IBoard.SetPlayer"/>
    /// </summary>
    public void SetPlayer(int row, int col)
    {
        if (row < 0 || col < 0)
            return;
        
        board[row, col] = _player;
    }

    /// <summary>
    /// <inheritdoc cref="IBoard.SetOpponent"/>
    /// </summary>
    public void SetOpponent(int row, int col)
    {
        if (row < 0 || col < 0)
            return;
        
        board[row, col] = _opponent;
    }

    /// <summary>
    /// <inheritdoc cref="IBoard.UndoMove"/>
    /// </summary>
    public void UndoMove(int row, int col)
    {
        if (row < 0 || col < 0)
            return;
        
        board[row, col] = _empty;
    }
    
    /// <summary>
    /// <inheritdoc cref="IBoard.CheckIfPlayerHasRowsSet"/>
    /// </summary>
    public bool CheckIfPlayerHasRowsSet()
    {
        return IsWinner(_player);
    }

    /// <summary>
    /// <inheritdoc cref="IBoard.CheckIfOpponentHasRowsSet"/>
    /// </summary>
    public bool CheckIfOpponentHasRowsSet()
    {
        return IsWinner(_opponent);
    }
    
    private bool IsWinner(string player)
    {
        if (board[0, 0] == player && board[0, 1] == player && board[0, 2] == player) 
            return true;
        if (board[1, 0] == player && board[1, 1] == player && board[1, 2] == player) 
            return true;
        if (board[2, 0] == player && board[2, 1] == player && board[2, 2] == player) 
            return true;
        if (board[0, 0] == player && board[1, 0] == player && board[2, 0] == player) 
            return true;
        if (board[0, 1] == player && board[1, 1] == player && board[2, 1] == player) 
            return true;
        if (board[0, 2] == player && board[1, 2] == player && board[2, 2] == player)
            return true;
        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) 
            return true;
        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) 
            return true;
        
        return false;
    }
    
    /// <summary>
    /// <inheritdoc cref="IBoard.IsPossibleToMakeAMove"/>
    /// </summary>
    public bool IsPossibleToMakeAMove()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (board[row, col] == _empty)
                    return true;
            }
        }
        
        return false;
    }
}