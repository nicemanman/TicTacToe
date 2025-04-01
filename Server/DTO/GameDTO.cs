namespace Server.DTO;

public class GameDTO
{
    public string State { get; set; }
    
    public Dictionary<int, Dictionary<int, string>> GameMap { get; set; }
}