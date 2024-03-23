namespace UserInterface.DTO;

public class GameResponse
{
    public Game Game { get; set; }
    public string Error { get; set; }
    public string Message { get; set; }
}

public class Game
{
    public string State { get; set; }
    public Dictionary<string, Dictionary<string, string>> GameMap { get; set; }
}