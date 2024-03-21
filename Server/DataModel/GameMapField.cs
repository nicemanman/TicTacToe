namespace Server.DataModel;

public class GameMapField
{
    public Guid UUID { get; set; }
    
    public int IndexX { get; }
    
    public int IndexY { get; }
    
    public bool IsX { get; }
}