using ArtificialIntelligence.DataModel.Interfaces;

namespace ArtificialIntelligence.AI.Interfaces;

/// <summary>
/// Бот для игры в крестики-нолики
/// </summary>
public interface IBot
{
    /// <summary>
    /// Сделать ход в игре
    /// </summary>
    /// <param name="game">Игра</param>
    public void MakeMove(IGame game);
}