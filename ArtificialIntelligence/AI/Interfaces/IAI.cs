using ArtificialIntelligence.DataModel;
using ArtificialIntelligence.DataModel.Interfaces;

namespace ArtificialIntelligence.AI.Interfaces;

public interface IAI
{
    Turn GetNextBestMove(IBoard board);
}