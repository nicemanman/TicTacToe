﻿namespace Server.DTO;

public class CreateGameErrorResponse
{
    public string Error { get; set; }
}

public class CreateGameSuccessResponse 
{
    public Guid UUID { get; set; }
    
    public int[,] GameMap { get; set; }
}