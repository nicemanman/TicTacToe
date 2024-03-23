namespace Server.DTO.Responses;

public class CreateGameErrorResponse
{
    public string Error { get; set; }
}

public class CreateGameSuccessResponse 
{
    public GameDTO Game { get; set; }
    
    public string Message { get; set; }
}