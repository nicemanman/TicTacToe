using Server.DataModel;

namespace Server.AI;

public interface IOpponentManager
{
    public Game MakeMove(Game game);
}