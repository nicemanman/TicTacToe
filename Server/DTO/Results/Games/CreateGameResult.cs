using Server.DataModel;

namespace Server.DTO.Results.Games;

public class CreateGameResult
{
    public Game Game { get; set; }
    
    public string ErrorMessage { get; set; }
    
    public string Message { get; set; }
}