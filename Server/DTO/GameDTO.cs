namespace Server.DTO;

public class GameDTO
{
    public string State { get; init; }
    
    public Dictionary<int, Dictionary<int, string>> GameMap { get; init; }
    
    public string JoinCode { get; init; }
}