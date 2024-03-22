using Localization.Game;

namespace Server.DTO;

public class CreateGameErrorResponse
{
    public string Error { get; set; }
}

public class CreateGameSuccessResponse 
{
    public GameDTO Game { get; set; }
}