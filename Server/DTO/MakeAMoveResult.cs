using Server.DataModel;

namespace Server.DTO;

public class MakeAMoveResult
{
    public Game Game { get; set; }
    
    public string Message { get; set; }
}