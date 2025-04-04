using Server.DataModel;

namespace Server.DTO.Results;

public class GetGameResult
{
    public Game Game { get; set; }
    
    public string Message { get; set; }
    
    public string ErrorMessage { get; set; }
}