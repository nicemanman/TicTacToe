using ArtificialIntelligence.DataModel;
using Server.DataModel;

namespace Server.DTO;

public class GameDTO
{
    public string State { get; init; }
    
    public Dictionary<int, Dictionary<int, string>> GameMap { get; init; }
    
    public List<CellCoord> WinningCells { get; init; }
    
    public string JoinCode { get; init; }
    
    public string SessionId { get; init; }
    
    public string PlayerIdTurn { get; init; }
    
    public string Player1Id { get; init; }
    
    public string Player2Id { get; init; }
    
    public string? PlayerIdWin { get; init; }
}