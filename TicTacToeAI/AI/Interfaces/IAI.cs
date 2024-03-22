using TicTacToeAI.DataModel;
using TicTacToeAI.DataModel.Interfaces;

namespace TicTacToeAI.AI.Interfaces;

public interface IAI
{
    Turn GetNextBestMove(IBoard board);
}