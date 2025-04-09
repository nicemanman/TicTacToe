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
    
    public List<CellCoord> WinningCells { get; set; }
    
    public string JoinCode { get; set; }
    
    public string SessionId { get; set; }
    
    public string Player2Id { get; set; }
    
    public string PlayerIdTurn { get; set; }
    
    public string PlayerIdWin { get; set; }
}

public class CellCoord
{
    public int Row { get; set; }
	
    public int Col { get; set; }
}