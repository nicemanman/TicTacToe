using Server.DataModel;

namespace Server.DTO.Results.Games;

public class MakeAMoveResult
{
    public Game Game { get; set; }

    public bool GameIsFinished { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public string Message { get; set; }
}