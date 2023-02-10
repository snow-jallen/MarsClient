using System.Net.Http.Json;

namespace MarsClient;


public interface INetService
{
    Task JoinGameAsync(string gameId, string playerName);
    Task<MoveResponse> MoveAsync(string direction);
    Task<bool> SeeIfGameHasStarted();
}

public class MauiNetService : INetService
{
    public MauiNetService(MapService mapService)
    {
        client.BaseAddress = new Uri("https://snow-rover.azurewebsites.net");
        //client.BaseAddress = new Uri("https://localhost:7287");
        this.mapService = mapService;
    }

    private string token;
    private HttpClient client = new HttpClient();
    private bool gameHasStarted = false;
    private int targetColumn;
    private int targetRow;
    private readonly MapService mapService;

    public async Task JoinGameAsync(string gameId, string playerName)
    {
        try
        {
            var joinReesponse = await client.GetFromJsonAsync<JoinResponse>($"/game/join?gameId={gameId}&name={playerName}");
            token = joinReesponse.Token;
            targetRow = joinReesponse.TargetRow;
            targetColumn = joinReesponse.TargetColumn;

            mapService.Map = new Models.Map(joinReesponse.LowResolutionMap);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<MoveResponse> MoveAsync(string direction)
    {
        if (!(await SeeIfGameHasStarted()))
        {
            return new MoveResponse { message = "Waiting for game to start" };
        }

        var moveResponse = await client.GetAsync($"/game/moveperseverance?token={token}&direction={direction}");
        if (moveResponse.IsSuccessStatusCode)
        {
            var mr = await moveResponse.Content.ReadFromJsonAsync<MoveResponse>();
            mr.targetColumn = targetColumn;
            mr.targetRow = targetRow;
            mapService.UpdateMap(mr.neighbors);
            return mr;
        }
        else
        {
            var content = await moveResponse.Content.ReadAsStringAsync();
            return new MoveResponse { message = content };
        }
    }

    public async Task<bool> SeeIfGameHasStarted()
    {
        if (gameHasStarted)
        {
            return true;
        }

        try
        {
            var statusResponse = await client.GetFromJsonAsync<StatusResult>($"/game/status?token={token}");
            if (statusResponse.status != "Playing")
            {
                return false;
            }
            gameHasStarted = true;
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class MoveResponse
{
    public int row { get; set; }
    public int column { get; set; }
    public int targetRow { get; set; }
    public int targetColumn { get; set; }
    public int batteryLevel { get; set; }
    public Neighbor[] neighbors { get; set; }
    public string message { get; set; }
    public string orientation { get; set; }
}

public class StatusResult
{
    public string status { get; set; }
}

