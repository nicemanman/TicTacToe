using Server.DataModel;

namespace Server.Services.Interfaces;

public interface IGameSessionManager
{
	/// <summary>
	/// Обновить статус активности сессии
	/// </summary>
	/// <param name="playerId"></param>
	public Task<Result<GameSession>> RegisterActivity(string playerId);

	/// <summary>
	/// Присоединить к сессии второго игрока
	/// </summary>
	/// <param name="playerId">ID второго игрока</param>
	public Task<Result<GameSession>> Join(string playerId, string joinCode);
	
	/// <summary>
	/// Покинуть сессию
	/// </summary>
	/// <param name="playerId">ID второго игрока</param>
	public Task<Result<GameSession>> Leave(string playerId, string joinCode);
	
	/// <summary>
	/// Создать сессию для игрока
	/// </summary>
	/// <param name="playerId">ID игрока</param>
	/// <param name="joinCode">Код присоединения</param>
	public Task<Result<GameSession>> StartParty(string playerId, bool againstBot = false);

	/// <summary>
	/// Поиск активной игровой сессии
	/// </summary>
	/// <param name="tokenOrUserId"></param>
	/// <returns></returns>
	public Task<Result<GameSession>> FindActiveSession(string tokenOrUserId);
	
	/// <summary>
	/// Удалить игру из памяти
	/// </summary>
	/// <param name="playerId"></param>
	public Task<Result<GameSession>> DeleteSession(string joinCode);

	Task<Result<GameSession>> MakeMove(GameSession session, string currentUserId, int row, int column);
}