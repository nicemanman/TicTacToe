using ArtificialIntelligence.DataModel;
using ArtificialIntelligence.DataModel.Interfaces;

namespace ArtificialIntelligence.AI.Interfaces;

/// <summary>
/// Искуственный интеллект
/// </summary>
public interface IAI
{
    Turn GetNextBestMove(IBoard board);
}