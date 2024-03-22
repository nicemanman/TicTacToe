using Server.DataModel;

namespace Server.DTO;

public class GameDTO
{
    public Guid UUID { get; set; }
    
    public string State { get; set; }
    
    public Dictionary<int, Dictionary<int, string>> GameMap { get; set; }
}