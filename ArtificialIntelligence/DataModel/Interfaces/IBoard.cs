namespace ArtificialIntelligence.DataModel.Interfaces;

public interface IBoard
{
    public List<BoardCell> GetCells();
    
    bool IsEmpty(int row, int col);

    int GetCurrentScore();

    void SetPlayer(int row, int col);

    void SetOpponent(int row, int col);

    void UndoMove(int row, int col);

    bool CheckIfPlayerHasRowsSet();

    bool CheckIfOpponentHasRowsSet();

    bool IsPossibleToMakeAMove();
}