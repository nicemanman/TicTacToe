using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using UserInterface.DTO;
using UserInterface.Data;

namespace UserInterface.Services;
public class GameService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime js;
    private readonly ProtectedLocalStorage _protectedLocalStorage;
    
    public GameService(IHttpClientFactory httpClientFactory, 
        ProtectedLocalStorage protectedLocalStorage, 
        IJSRuntime js)
    {

        _protectedLocalStorage = protectedLocalStorage;
        this.js = js;
        _httpClient = httpClientFactory.CreateClient("MyClient");
    }
    
    public async Task<GameResponse> CreateGame(bool againstBot)
    {
        var message = HttpRequestBuilder.BuildRequest(HttpMethod.Post, $"/api/game?againstBot={againstBot}",
            await GetCookieHeadersAsync());

        var response = await _httpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            return new GameResponse()
            {
                Error = "Не удалось создать игру, попробуйте позже"
            };
        
        await SaveCookieHeadersAsync(response);
        var gameResponse = await response.Content.ReadFromJsonAsync<GameResponse>();

        await js.InvokeVoidAsync("console.log", $"CreateGame - {JsonConvert.SerializeObject(gameResponse)}");
        
        return gameResponse;
    }

    public async Task<bool> JoinGame(string code)
    {
        var message = HttpRequestBuilder.BuildRequest(HttpMethod.Post, $"/api/Game/Join?joinCode={code}",
            await GetCookieHeadersAsync());

        var response = await _httpClient.SendAsync(message);
        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content.ReadFromJsonAsync<GameResponse>();
        if (result?.Game == null)
            return false;

        await SaveCookieHeadersAsync(response);
        
        return true;
    }
    
    public async Task<GameResponse> GetGame()
    {
        var message = HttpRequestBuilder.BuildRequest(HttpMethod.Get, "/api/game",
            await GetCookieHeadersAsync());
        var response = await _httpClient.SendAsync(message);

        if (response.StatusCode == HttpStatusCode.NoContent)
            return new GameResponse();

        if (!response.IsSuccessStatusCode)
            return new GameResponse
            {
                Error = "Не удалось получить игру, попробуйте позже"
            };
        
        var body = await response.Content.ReadFromJsonAsync<GameResponse>();
        await SaveCookieHeadersAsync(response);
        await js.InvokeVoidAsync("console.log", $"CreateGame - {JsonConvert.SerializeObject(body)}");
        return body;
    }

    public async Task<GameResponse> MakeMove(int row, int column)
    {
        var message = HttpRequestBuilder.BuildRequest(HttpMethod.Patch, $"/api/game?row={row}&column={column}",
            await GetCookieHeadersAsync());
        var response = await _httpClient.SendAsync(message);

        if (!response.IsSuccessStatusCode)
            return new GameResponse()
            {
                Error = "Не удалось сделать ход, попробуйте позже"
            };
        
        await SaveCookieHeadersAsync(response);
        
        return await response.Content.ReadFromJsonAsync<GameResponse>();
    }

    public async Task<string> GetCurrentUserId()
    {
        var message = HttpRequestBuilder.BuildRequest(HttpMethod.Get, "/api/Identification",
            await GetCookieHeadersAsync());
        
        var resp = await _httpClient.SendAsync(message);
        resp.EnsureSuccessStatusCode();
        
        await SaveCookieHeadersAsync(resp);
        return await resp.Content.ReadAsStringAsync();
    }
    
    public async Task Identify(string playerName)
    {
        var message = HttpRequestBuilder.BuildRequest(HttpMethod.Post, $"/api/Identification?username={playerName}",
            await GetCookieHeadersAsync());
        var response = await _httpClient.SendAsync(message);
        response.EnsureSuccessStatusCode();
        await SaveCookieHeadersAsync(response);
    }

    private async Task SaveCookieHeadersAsync(HttpResponseMessage response)
    {
        IEnumerable<string> cookies = response.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;

        if (cookies is not null && cookies.Any())
        {
            var cookieHeader = string.Join("; ", cookies);
            await _protectedLocalStorage.SetAsync("Session Cookie String", HeaderNames.Cookie, cookieHeader);
        }
    }

    private async Task<Dictionary<string, string>> GetCookieHeadersAsync()
    {
        var cookies = await _protectedLocalStorage.GetAsync<string>("Session Cookie String", HeaderNames.Cookie);
        
        if (cookies.Success)
            return new Dictionary<string, string>()
            {
                { HeaderNames.Cookie, cookies.Value }
            };
        
        return null;
    }
}