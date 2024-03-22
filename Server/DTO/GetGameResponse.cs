namespace Server.DTO;

public class GetGameErrorResponse
{
    public string Error { get; set; }
}

public class GetGameSuccessResponse 
{
    public Guid UUID { get; set; }
    
    public int[,] GameMap { get; set; }
}