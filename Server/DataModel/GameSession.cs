using System.ComponentModel.DataAnnotations.Schema;
using Database.Interfaces;

namespace Server.DataModel;

public class GameSession : IEntity
{
	public Guid UUID { get; set; }
	
	public string Player1Id { get; set; }
	
	public string? Player2Id { get; set; }
	
	public string? PlayerIdTurn { get; set; }
	
	public string? PlayerIdWin { get; set; }
	
	public string? JoinCode { get; init; }
	
	public DateTime LastActivity { get; set; }
	
	public Game Game { get; set; }

	public GameState GameState { get; set; }
	
	public bool IsGameFinished => GameState != GameState.InProgress;
	
}