using Server.DataModel;

namespace Server.OpponentManager;

/// <summary>
/// Оппонент в игре крестики-нолики
/// </summary>
public interface IBotManager
{
    public Game MakeMove(Game game);
}