namespace Server.DTO.Responses;

public class GetGameErrorResponse
{
    public string Error { get; set; }
}

public class GetGameSuccessResponse 
{
    public GameDTO Game { get; set; }
    
    public string Message { get; set; }
}