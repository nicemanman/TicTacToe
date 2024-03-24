using Server.DataModel;

namespace Server.AI;

/// <summary>
/// Оппонент в игре крестики-нолики
/// </summary>
public interface IOpponentManager
{
    public Game MakeMove(Game game);
}