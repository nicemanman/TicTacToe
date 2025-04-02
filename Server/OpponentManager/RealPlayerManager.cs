using Server.DataModel;

namespace Server.AI;

public class RealPlayerManager : IOpponentManager
{
	public async Task<Game> MakeMove(Game game)
	{
		MakeMoveNotificationMessage message = new()
		{
			Game = game,
			UserId = GetUserId(game),
			SessionId = game.UUID.ToString()
		};
		
		//1. Отправляем нотификацию с
		//uint ID оппонента (-1 если это игра с ботом),
		//uint ID игры,
		//сериализованной текущей игрой в рэббит,
		//2. Приложение фронта получает эту нотификацию, обновляет состояние игры на экране,
		//и включает возможность ходить для оппонента
		//3. Оппонент делает ход. Переход к п. 1
		throw new NotImplementedException();
	}

	private static uint GetUserId(Game game)
	{
		if (game.OpponentIsBot)
			return 0;

		return game.IsOpponentTurn ? game.OpponentId : game.CreatorId;
	}
}