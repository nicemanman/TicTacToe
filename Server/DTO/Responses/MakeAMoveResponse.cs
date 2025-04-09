namespace Server.DTO.Responses;

public class MakeAMoveErrorResponse
{
    public string Error { get; set; }
}

public class MakeAMoveSuccessResponse 
{
    public GameDTO Game { get; set; }
    
    public string Message { get; set; }
}