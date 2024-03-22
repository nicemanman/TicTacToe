namespace Server.DTO;

public class MakeAMoveErrorResponse
{
    public string Error { get; set; }
}

public class MakeAMoveSuccessResponse 
{
    public GameDTO Game { get; set; }
}