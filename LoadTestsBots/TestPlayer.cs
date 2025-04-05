using System.Text.Json;
using LoadTests.Contracts;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LoadTests;

public class TestPlayer
{
	private readonly HttpClient _client;
	private readonly string _username;
	private GameDTO _currentGame;

	public string Username => _username;

	public event Action<string, Exception> OnError;
	public event Action<string> OnCompleted;

	public TestPlayer(HttpClient client, string username)
	{
		_client = client;
		_username = username;
	}

	public async Task RunAsync()
	{
		try
		{
			await IdentifyAsync();
			await CreateGameAsync();

			while (_currentGame.GameState == "InProgress")
			{
				var move = FindFirstEmptyCell(_currentGame.Field);
				if (move == null) 
					break;

				var response = await _client.PatchAsync($"/api/Game?row={move.Value.row}&column={move.Value.col}", null);
				var content = await response.Content.ReadAsStringAsync();
				var makeMoveResponse = JsonConvert.DeserializeObject<MakeAMoveSuccessResponse>(content);
				if (response.IsSuccessStatusCode)
				{
					var gameResp = JsonSerializer.Deserialize<MakeAMoveSuccessResponse>(content, JsonOpts());
					CheckMoveValidity(_currentGame.Field, gameResp.Game.Field);
					_currentGame = gameResp.Game;
				}
				else if (makeMoveResponse.Game.GameState != "InProgress")
				{
					break;
				}
				else
				{
					throw new Exception($"Ошибка хода: {content}");
				}

				await Task.Delay(30);
			}

			OnCompleted?.Invoke(_username);
		}
		catch (Exception ex)
		{
			OnError?.Invoke(_username, ex);
		}
	}

	private async Task IdentifyAsync()
	{
		var response = await _client.PostAsync($"/api/Identification?username={_username}", null);
		response.EnsureSuccessStatusCode();
	}

	private async Task CreateGameAsync()
	{
		var response = await _client.PostAsync($"/api/Game?againstBot=true", null);
		var content = await response.Content.ReadAsStringAsync();

		if (!response.IsSuccessStatusCode)
			throw new Exception($"CreateGame error: {response.StatusCode}, {response.ReasonPhrase}, {content}");

		var gameResp = JsonSerializer.Deserialize<CreateGameResponse>(content, JsonOpts());
		_currentGame = gameResp.Game;
	}

	private static (int row, int col)? FindFirstEmptyCell(string[,] field)
	{
		for (int i = 0; i < field.GetLength(0); i++)
		for (int j = 0; j < field.GetLength(1); j++)
			if (string.IsNullOrEmpty(field[i, j]))
				return (i, j);
		return null;
	}

	private void CheckMoveValidity(string[,] before, string[,] after)
	{
		int playerMoves = 0;
		int botMoves = 0;

		for (int i = 0; i < before.GetLength(0); i++)
		for (int j = 0; j < before.GetLength(1); j++)
		{
			var b = before[i, j];
			var a = after[i, j];

			if (b == a)
				continue;

			// Было пусто — стало что-то
			if (string.IsNullOrEmpty(b) && !string.IsNullOrEmpty(a))
			{
				if (a == "X")
					playerMoves++;
				else if (a == "O")
					botMoves++;
				else
					throw new Exception($"Недопустимый символ '{a}' на ({i},{j})");
			}
			else
			{
				// Было что-то, стало другое — ошибка
				throw new Exception($"Нарушена целостность: клетка ({i},{j}) изменилась с '{b}' на '{a}'");
			}
		}

		if (playerMoves != 1 || botMoves != 1)
			throw new Exception($"Ожидалось: 1 ход игрока (X) и 1 ход бота (O), получено: X={playerMoves}, O={botMoves}");
	}


	private JsonSerializerOptions JsonOpts() => new()
	{
		PropertyNameCaseInsensitive = true
	};
}