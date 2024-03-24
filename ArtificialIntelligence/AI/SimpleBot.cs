using ArtificialIntelligence.AI.Interfaces;
using ArtificialIntelligence.DataModel;
using ArtificialIntelligence.DataModel.Interfaces;

namespace ArtificialIntelligence.AI;

public class SimpleBot : IBot
{
    private readonly IAI _ai = new SimpleAI();

    public void MakeMove(IGame game)
    {
        Turn currentTurn = _ai.GetNextBestMove(game.Board);
        game.Board.SetOpponent(currentTurn.Row, currentTurn.Column);
        game.UpdateGameState();
    }
}