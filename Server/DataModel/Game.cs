using Database.Interfaces;

namespace Server.DataModel;

public class Game : IEntity
{
    public Guid UUID { get; set; }
    
    public GameMap GameMap { get; set; }
}