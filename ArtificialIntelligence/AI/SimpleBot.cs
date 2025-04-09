using ArtificialIntelligence.AI.Interfaces;
using ArtificialIntelligence.DataModel;
using ArtificialIntelligence.DataModel.Interfaces;

namespace ArtificialIntelligence.AI;

/// <summary>
/// Простая реализация бота для игры в крестики-нолики
/// </summary>
public class SimpleBot : IBot
{
    private readonly IAI _ai = new SimpleAI();

    /// <summary>
    /// <inheritdoc cref="IBot.MakeMove"/>
    /// </summary>
    public void MakeMove(IGame game)
    {
        Turn currentTurn = _ai.GetNextBestMove(game.Board);
        game.Board.SetOpponent(currentTurn.Row, currentTurn.Column);
    }
}