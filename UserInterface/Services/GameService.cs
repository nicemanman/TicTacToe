using System.Net;
using UserInterface.DTO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace UserInterface.Services;
public class GameService
{
    private readonly HttpClient _httpClient;
    private string _sessionCookieName = ".AspNetCore.Session";
    
    public GameService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MyClient");
    }

    public async Task<GameResponse> CreateGame(bool playerFirst)
    {
        var response = await _httpClient.PostAsJsonAsync($"/api/game?playerFirst={playerFirst}", new {});
        return await response.Content.ReadFromJsonAsync<GameResponse>();
    }

    public async Task<GameResponse> GetGame()
    {
        return await _httpClient.GetFromJsonAsync<GameResponse>("/api/game");
    }

    public async Task<GameResponse> MakeMove(int row, int column)
    {
        var response = await _httpClient.PatchAsync($"/api/game?row={row}&column={column}", null);
        return await response.Content.ReadFromJsonAsync<GameResponse>();
    }
}