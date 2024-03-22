﻿namespace Server.DTO;

public class GetGameErrorResponse
{
    public string Error { get; set; }
}

public class GetGameSuccessResponse 
{
    public GameDTO Game { get; set; }
}