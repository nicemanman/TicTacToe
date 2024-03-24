namespace ArtificialIntelligence.DataModel.Interfaces;

/// <summary>
/// Процесс игры в крестики-нолики
/// </summary>
public interface IGame
{
    /// <summary>
    /// Игровая доска
    /// </summary>
    public IBoard Board { get; set; }

    /// <summary>
    /// Состояние игры
    /// </summary>
    public GameState State { get; }
    
    /// <summary>
    /// Обновить состояние игры
    /// </summary>
    void UpdateGameState();
}