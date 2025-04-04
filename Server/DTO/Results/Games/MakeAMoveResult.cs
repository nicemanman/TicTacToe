using Server.DataModel;

namespace Server.DTO.Results;

public class MakeAMoveResult
{
    public Game Game { get; set; }

    public bool GameIsFinished => Game?.IsFinished ?? false;
    
    public string ErrorMessage { get; set; }
    
    public string Message { get; set; }
}