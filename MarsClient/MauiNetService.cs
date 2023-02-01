using System.Net.Http.Json;

namespace MarsClient;


public interface INetService
{
    Task JoinGameAsync(string gameId, string playerName);
    Task MoveAsync(string direction);
}

public class MauiNetService : INetService
{
    private string token;
    private HttpClient client = new HttpClient();
    public MauiNetService()
    {
        client.BaseAddress = new Uri("https://snow-rover-pr-7.azurewebsites.net");
    }

    public async Task JoinGameAsync(string gameId, string playerName)
    {
        var joinReesponse = await client.GetFromJsonAsync<JoinResponse>($"/game/join?gameId={gameId}&name={playerName}");
        token = joinReesponse.token;
    }

    public async Task MoveAsync(string direction)
    {
        var moveResponse = await client.GetStringAsync($"/game/moveperseverance?token={token}&direction={direction}");
    }
}

