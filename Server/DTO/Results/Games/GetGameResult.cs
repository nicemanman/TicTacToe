using Server.DataModel;

namespace Server.DTO.Results.Games;

public class GetGameResult
{
    public Game Game { get; set; }
    
    public string Message { get; set; }
    
    public string ErrorMessage { get; set; }
}