namespace ArtificialIntelligence.DataModel.Interfaces;

public interface IGame
{
    public IBoard Board { get; set; }

    public GameState State { get; }
    
    void UpdateGameState();
}