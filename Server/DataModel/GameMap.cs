using Database.Interfaces;

namespace Server.DataModel;

public class GameMap : IEntity
{
    public Guid UUID { get; set; }
    
    public List<GameMapField> Map { get; set; }
}