using Server.DataModel;

namespace Server.AI;

public interface IAiManager
{
    public Game MakeMove(Game game);
}