using ArtificialIntelligence.DataModel.Interfaces;
using ArtificialIntelligence.DataModel;

namespace ArtificialIntelligence.AI.Interfaces;

public interface IBot
{
    public void MakeMove(IGame game);
}