namespace Server.DTO.Responses;

public class MakeAMoveErrorResponse
{
    public string Error { get; set; }
}

public class MakeAMoveSuccessResponse 
{
    public GameDTO Game { get; set; }
}

public class MakeAMoveGameIsFinishedResponse
{
    public string Message { get; set; }
}