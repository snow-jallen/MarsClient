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
            var direction = determineDirection(gameState.Orientation, gameState.Perseverance, gameState.Targets.First());
            var response = await httpClient.GetAsync($"/game/moveperseverance?token={gameState.Token}&direction={direction}");
            if (response.IsSuccessStatusCode)
            {
                var moveResult = await response.Content.ReadFromJsonAsync<MoveResponse>();
                if (moveResult.Message.Contains("Insufficient battery"))
                {
                    logger.LogInformation("Insufficient battery.  Current battery level: {batteryLevel}", moveResult.BatteryLevel);
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                if (moveResult.Message == "You made it to the target!")
                {
                    gameState.Targets.Dequeue();
                    Console.WriteLine();
                    Console.WriteLine($"You made it to a target, {gameState.Targets.Count} targets remain.");
                }

                gameState.Orientation = moveResult.Orientation;
                gameState.PerseveranceBatteryLevel = moveResult.BatteryLevel;
                gameState.UpdateMap(moveResult.Neighbors);
                gameState.Perseverance = (moveResult.X, moveResult.Y);
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    logger.LogWarning("You either won, or the game is over, or something weird...I'm not playing any more.");
                    break;
                }
                logger.LogInformation("Status code: {statusCode}; Reason Phrase: {reasonPhrase}", response.StatusCode, response.ReasonPhrase);
            }
        }
    }

    private string determineDirection(string orientation, (int x, int y) perseverance, Location target)
    {
        var targetIsToTheRight = (target.X > perseverance.x);
        if (target.Y == perseverance.y)
        {
            if (targetIsToTheRight)
            {
                if (orientation == "East")
                {
                    return "Forward";
                }
                return "Right";
            }
            if (orientation == "West")
            {
                return "Forward";
            }
            return "Left";
        }
        var targetIsAbove = target.Y > perseverance.y;
        if (targetIsAbove)
        {
            if (orientation == "North")
            {
                return "Forward";
            }

            return "Right";
        }
        if (orientation == "South")
        {
            return "Forward";
        }
        return "Right";
    }
}