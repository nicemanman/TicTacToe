using AutoMapper;
using Localization.Game;
using Localization.GameSession;
using MessageQueue.DataModel;
using MessageQueue.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Data.Interfaces;
using Server.DataModel;
using Server.DTO;
using Server.Exceptions.GameSessionExceptions;
using Server.OpponentManager;
using Server.Services.Interfaces;
using Server.SignalR;

namespace Server.Services;

/// <summary>
/// Класс нужно доделать для многопоточки
/// </summary>
/// <param name="unitOfWork"></param>
/// <param name="gameService"></param>
public class GameSessionManager(
	IUnitOfWork unitOfWork, 
	IGameService gameService,
	IJoinCodeService joinCodeService,
	IMqClient mqClient,
	IBotManager botManager,
	IHubContext<GameHub> hubContext,
	IMapper mapper) : IGameSessionManager
{
	public async Task<Result<GameSession>> RegisterActivity(string playerId)
	{
		GameSession session = await FindByPlayer(playerId);

		if (session == null)
			return Result.Fail<GameSession>(GameSessionMessages.SessionNotFound);

		return await RegisterActivity(session);
	}

	public async Task<Result<GameSession>> RegisterActivity(GameSession session)
	{
		session.LastActivity = DateTime.UtcNow;
		var updatedSession = await unitOfWork.GameSessionRepository.UpdateAsync(session);
		return Result.Ok(updatedSession);
	}
	
	public async Task<Result<GameSession>> Join(string playerId, string joinCode)
	{
		GameSession session = await FindByJoinCode(joinCode);

		if (session == null)
			return Result.Fail<GameSession>(GameSessionMessages.SessionNotFound);
		
		if (!string.IsNullOrWhiteSpace(session.Player2Id) && session.Player2Id != playerId)
			return Result.Fail<GameSession>(GameSessionMessages.SessionIsFull);
		
		if (session.Player2Id == playerId)
			return Result.Ok(session);

		session.Player2Id = playerId;

		if (string.IsNullOrWhiteSpace(session.PlayerIdTurn))
			session.PlayerIdTurn = playerId;
		
		var updatedSession = await unitOfWork.GameSessionRepository.UpdateAsync(session);
		await mqClient.Subscribe(playerId, OnMessage);
		return Result.Ok(updatedSession);;
	}

	public async Task<Result<GameSession>> Leave(string playerId, string joinCode)
	{
		throw new NotImplementedException();
	}

	public async Task<Result<GameSession>> StartParty(string playerId, bool againstBot = false)
	{
		GameSession session = await FindByPlayer(playerId);
		if (session is { IsGameFinished: false })
			return Result.Fail<GameSession>(GameSessionMessages.SessionAlreadyCreated);

		var result = await gameService.CreateAsync();

		if (result.Failure)
			return Result.Fail<GameSession>(GameSessionMessages.SessionFailedAtCreation);

		if (againstBot)
		{
			session = new()
			{
				LastActivity = DateTime.UtcNow,
				Player1Id = playerId,
				Player2Id = TicTacToeConstants.BotId,
				JoinCode = string.Empty,
				Game = result.Value,
				GameState = GameState.InProgress
			};

			GameSession playWithBotSession;

			try
			{
				playWithBotSession = await unitOfWork.GameSessionRepository.CreateAsync(session);
			}
			catch (DbUpdateException ex) 
				when (ex.InnerException?.Message?.Contains("IX_GameSessions_Player1Id") ?? false)
			{
				return Result.Fail<GameSession>(GameSessionMessages.SessionAlreadyCreated);
			}

			bool isBotFirst = true;
			
			if (isBotFirst)
				session.Game = botManager.MakeMove(session.Game);
			
			return Result.Ok(playWithBotSession);
		}
		
		string joinCode;
		
		//Не самый лучший способ найти уникальный JoinCode, но самый простой сейчас
		while (true)
		{
			joinCode = joinCodeService.GetJoinCode();
			
			GameSession sessionByJoinCode = await FindByJoinCode(joinCode);
			if (sessionByJoinCode is { IsGameFinished: false })
				await Task.Delay(1);
			else 
				break;
		}
		
		session = new()
		{
			LastActivity = DateTime.UtcNow,
			Player1Id = playerId,
			PlayerIdTurn = playerId,
			JoinCode = joinCode,
			Game = result.Value,
			GameState = result.Value.State
		};

		var createdSession = await unitOfWork.GameSessionRepository.CreateAsync(session);
		await mqClient.Subscribe(playerId, OnMessage);
		return Result.Ok(createdSession);
	}

	public async Task<Result<GameSession>> FindActiveSession(string tokenOrUserId)
	{
		GameSession session = await FindByPlayer(tokenOrUserId);
		if (session is { IsGameFinished: false })
			return Result.Ok(session);

		session = await FindByJoinCode(tokenOrUserId);
		if (session is { IsGameFinished: false })
			return Result.Ok(session);

		return Result.Ok<GameSession>(null);
	}

	private async Task<GameSession> FindByPlayer(string playerId)
	{
		var session = await unitOfWork.GameSessionRepository
			.FirstOrDefault(s => 
				(s.Player1Id == playerId || s.Player2Id == playerId) 
			                     && s.GameState == GameState.InProgress);

		return session;
	}

	private async Task<GameSession> FindByJoinCode(string joinCode)
	{
		var session = await unitOfWork.GameSessionRepository
			.FirstOrDefault(s => 
				s.JoinCode == joinCode && s.GameState == GameState.InProgress);

		return session;
	}

	public async Task<Result<GameSession>> DeleteSession(string joinCode)
	{
		GameSession session = await FindByJoinCode(joinCode);

		if (session == null)
			throw new SessionNotCreatedException();

		var deletedSession = await unitOfWork.GameSessionRepository.DeleteAsync(session.UUID);
		return Result.Ok(deletedSession);
	}

	public async Task<Result<GameSession>> MakeMove(GameSession session, string currentUserId, int row, int column)
	{
		if (session.Player2Id != TicTacToeConstants.BotId && currentUserId != session.PlayerIdTurn)
			return Result.Fail<GameSession>(GameMessages.UnableToSetCell_NotYourTurn);
		
		string ch = session.Player1Id == currentUserId ? "X" : "O";
		
		var makeMoveResult = await gameService.MakeAMoveAsync(session, row, column, ch);
		if (makeMoveResult.Failure)
			return Result.Fail<GameSession>(makeMoveResult.Error);

		await RegisterActivity(session);

		if (session.Player2Id != TicTacToeConstants.BotId)
		{
			string nextTurnPlayer = session.Player1Id == currentUserId 
				? session.Player2Id : session.Player1Id;
			
			session.PlayerIdTurn = nextTurnPlayer;
		}

		var result = CheckWinner(session.Game.GameMap.Fields);
		session.Game.State = result.state;
		session.Game.WinningCells = result.winningCells;
		session.GameState = session.Game.State;

		session.PlayerIdWin = session.Game.State switch
		{
			GameState.Player1Win => session.Player1Id,
			GameState.Player2Win => session.Player2Id,
			_ => session.PlayerIdWin
		};

		var updatedSession = await unitOfWork.GameSessionRepository.UpdateAsync(session);
		
		var gameDto = mapper.Map<GameSession, GameDTO>(updatedSession);
		if (session.Player2Id != TicTacToeConstants.BotId)
		{
			var message = new RabbitMessage()
			{
				SenderId = currentUserId,
				SessionId = session.UUID.ToString(),
				ReceiverId = session.Player1Id == currentUserId ? session.Player2Id : session.Player1Id,
				Payload = gameDto
			};
			
			if (session.GameState == GameState.InProgress)
				message.MessageType = MessageType.Move;
			else
				message.MessageType = MessageType.GameOver;
			
			await mqClient.Send(message);
		}
		else
		{
			await mqClient.Send(new RabbitMessage()
			{
				SenderId = currentUserId,
				MessageType = MessageType.GameOver,
				SessionId = session.UUID.ToString(),
				ReceiverId = session.Player1Id == currentUserId ? session.Player2Id : session.Player1Id,
				Payload = gameDto
			});
		}
		return Result.Ok(updatedSession);
	}

	private static (GameState state, List<CellCoord> winningCells) CheckWinner(List<GameMapField> fields)
	{
		var board = new string[3, 3];
		var winningCells = new List<CellCoord>();

		// Заполняем двумерный массив
		foreach (var field in fields)
		{
			board[field.Row, field.Column] = field.Char;
		}

		// Проверка всех победных комбинаций
		var lines = new List<List<CellCoord>>
		{
			// Горизонтали
			new() { new(0,0), new(0,1), new(0,2) },
			new() { new(1,0), new(1,1), new(1,2) },
			new() { new(2,0), new(2,1), new(2,2) },

			// Вертикали
			new() { new(0,0), new(1,0), new(2,0) },
			new() { new(0,1), new(1,1), new(2,1) },
			new() { new(0,2), new(1,2), new(2,2) },

			// Диагонали
			new() { new(0,0), new(1,1), new(2,2) },
			new() { new(0,2), new(1,1), new(2,0) },
		};

		foreach (var line in lines)
		{
			var chars = line.Select(c => board[c.Row, c.Col]).ToArray();

			if (chars.All(c => c == "X"))
			{
				winningCells = line;
				return (GameState.Player1Win, winningCells);
			}

			if (chars.All(c => c == "O"))
			{
				winningCells = line;
				return (GameState.Player2Win, winningCells);
			}
		}

		// Проверка на ничью
		if (fields.All(f => !string.IsNullOrEmpty(f.Char)))
		{
			return (GameState.Tie, new List<CellCoord>());
		}

		// Игра ещё не закончена
		return (GameState.InProgress, new List<CellCoord>());
	}

	
	private async Task OnMessage(RabbitMessage message)
	{
		await hubContext.Clients.Group(message.SessionId).SendAsync("GameUpdated", JsonConvert.SerializeObject(message.Payload));
	}
}