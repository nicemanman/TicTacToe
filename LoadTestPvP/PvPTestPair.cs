using System.Text.Json;
using LoadTestPvP.Contracts;
using Microsoft.AspNetCore.SignalR.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LoadTestPvP;

public class PvPTestPair
{
    private readonly string _player1;
    private readonly string _player2;
    private readonly HttpClient _client1;
    private readonly HttpClient _client2;

    private HubConnection _hub1;
    private HubConnection _hub2;

    private GameDTO _game;
    private string _sessionId;

    private readonly TaskCompletionSource<GameDTO> _player1Update = new();
    private readonly TaskCompletionSource<GameDTO> _player2Update = new();

    public PvPTestPair(string player1, string player2, HttpClient client1, HttpClient client2)
    {
        _player1 = player1;
        _player2 = player2;
        _client1 = client1;
        _client2 = client2;
    }

    public async Task RunAsync(Action<string> onSuccess, Action<string, Exception> onError)
    {
        try
        {
            await Identify(_client1, _player1);
            await Identify(_client2, _player2);

            // Игрок 1 создаёт игру
            var response = await _client1.PostAsync("/api/Game?againstBot=false", null);
            var content = await response.Content.ReadAsStringAsync();
            var createResp = JsonSerializer.Deserialize<CreateGameResponse>(content, JsonOpts());

            _game = createResp.Game;
            _sessionId = createResp.Game.SessionId;

            if (string.IsNullOrWhiteSpace(_sessionId))
                throw new Exception("SessionId не получен");

            // Подключение к SignalR
            await SetupHubConnections();

            // Игрок 2 подключается к игре
            var joinCode = createResp.Game.JoinCode;
            var joinResp = await _client2.PostAsync($"/api/Game/Join?joinCode={joinCode}", null);
            joinResp.EnsureSuccessStatusCode();

            while (_game.GameState == "InProgress")
            {
                // Игрок 1 делает первый ход
                await MakeMove(_client1, _player1);
                
                // Ждём обновления у второго игрока
                _game = await WaitForUpdate(_player2Update);
                if (_game.GameState != "InProgress") 
                    break;

                await MakeMove(_client2, _player2);

                // Ждём обновления у первого игрока
                _game = await WaitForUpdate(_player1Update);
                if (_game.GameState != "InProgress") 
                    break;
            }

            onSuccess?.Invoke($"{_player1} vs {_player2}");
        }
        catch (Exception ex)
        {
            onError?.Invoke($"{_player1} vs {_player2}", ex);
        }
        finally
        {
            if (_hub1 != null) await _hub1.DisposeAsync();
            if (_hub2 != null) await _hub2.DisposeAsync();
        }
    }

    private async Task SetupHubConnections()
    {
        _hub1 = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/gameHub")
            .WithAutomaticReconnect()
            .Build();

        _hub2 = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/gameHub")
            .WithAutomaticReconnect()
            .Build();

        _hub1.On<GameDTO>("GameUpdated", game =>
        {
            _player1Update.TrySetResult(game);
        });

        _hub2.On<GameDTO>("GameUpdated", game =>
        {
            _player2Update.TrySetResult(game);
        });

        await _hub1.StartAsync();
        await _hub2.StartAsync();

        await _hub1.InvokeAsync("JoinSession", _sessionId);
        await _hub2.InvokeAsync("JoinSession", _sessionId);
    }

    private async Task Identify(HttpClient client, string name)
    {
        var response = await client.PostAsync($"/api/Identification?username={name}", null);
        response.EnsureSuccessStatusCode();
    }

    private async Task MakeMove(HttpClient client, string username)
    {
        var move = FindFirstEmptyCell(_game.Field);
        if (move == null) return;

        var response = await client.PatchAsync($"/api/Game?row={move.Value.row}&column={move.Value.col}", null);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Ошибка хода {username}: {content}");

        var gameResp = JsonSerializer.Deserialize<MakeAMoveSuccessResponse>(content, JsonOpts());
        _game = gameResp.Game;

        await Task.Delay(30);
    }

    private async Task<GameDTO> WaitForUpdate(TaskCompletionSource<GameDTO> source)
    {
        var game = await source.Task;
        return game;
    }

    private static (int row, int col)? FindFirstEmptyCell(string[,] field)
    {
        for (int i = 0; i < field.GetLength(0); i++)
        for (int j = 0; j < field.GetLength(1); j++)
            if (string.IsNullOrEmpty(field[i, j]))
                return (i, j);
        return null;
    }

    private static JsonSerializerOptions JsonOpts() => new() { PropertyNameCaseInsensitive = true };
}
