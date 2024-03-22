namespace Server.DTO;

public class GameDTO
{
    public Guid UUID { get; set; }
    
    public Dictionary<int, Dictionary<int, bool>> GameMap { get; set; }
}