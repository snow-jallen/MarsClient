using Microsoft.Extensions.Logging;

namespace ConsoleClient;

public class PerseveranceDriver
{
    private readonly ILogger<PerseveranceDriver> logger;
    private readonly GameState gameState;
    private readonly HttpClient httpClient;

    public PerseveranceDriver(ILogger<PerseveranceDriver> logger, GameState gameState, HttpClient httpClient)
    {
        this.logger = logger;
        this.gameState = gameState;
        this.httpClient = httpClient;
    }

    public async Task StartDrivingAsync()
    {
        while (true)
        {
            var direction = determineDirection(gameState.Orientation, gameState.Target);
            var response = await httpClient.GetAsync($"/game/moveperseverance?token={gameState.Token}&direction={direction}");
            if (response.IsSuccessStatusCode)
            {
                var moveResult = await response.Content.ReadFromJsonAsync<MoveResponse>();
                gameState.Orientation = moveResult.Orientation;
                gameState.PerseveranceBatteryLevel = moveResult.BatteryLevel;
                gameState.UpdateMap(moveResult.Neighbors);
                gameState.PerseveranceX = moveResult.Row;
                gameState.PerseveranceY = moveResult.Column;

            }
        }
    }

    private string determineDirection(string orientation, (int TargetX, int TargetY) target)
    {
        return "Forward";
    }
}