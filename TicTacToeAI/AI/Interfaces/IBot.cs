using TicTacToeAI.DataModel;
using TicTacToeAI.DataModel.Interfaces;

namespace TicTacToeAI.AI.Interfaces;

public interface IBot
{
    public void MakeMove(IGame game);
}