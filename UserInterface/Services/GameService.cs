using System.Net;
using UserInterface.DTO;

namespace UserInterface.Services;
public class GameService
{
    private readonly HttpClient _httpClient;
    private string _sessionCookieName = ".AspNetCore.Session";
    
    public GameService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MyClient");
    }

    public async Task<GameResponse> CreateGame(bool againstBot)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/game?againstBot={againstBot}", new {});
        return await response.Content.ReadFromJsonAsync<GameResponse>();
    }

    public async Task<GameResponse> GetGame()
    {
        var response = await _httpClient.GetAsync("/api/game");

        if (response.StatusCode == HttpStatusCode.NoContent)
            return new GameResponse();

        var body = await response.Content.ReadFromJsonAsync<GameResponse>();

        return body;
    }

    public async Task<GameResponse> MakeMove(int row, int column)
    {
        var response = await _httpClient.PatchAsync($"/api/game?row={row}&column={column}", null);
        return await response.Content.ReadFromJsonAsync<GameResponse>();
    }
}