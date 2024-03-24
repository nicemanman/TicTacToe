namespace ArtificialIntelligence.DataModel;

/// <summary>
/// Клетка игровой доски
/// </summary>
public class BoardCell
{
    /// <summary>
    /// Ряд
    /// </summary>
    public int Row { get; set; }
    
    /// <summary>
    /// Столбец
    /// </summary>
    public int Column { get; set; }
    
    /// <summary>
    /// Значок (х или о)
    /// </summary>
    public string Char { get; set; }
}