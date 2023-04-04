using Microsoft.Extensions.Logging;

namespace ConsoleClient;

public class IngenuityFlyer
{
    private readonly ILogger<IngenuityFlyer> logger;
    private readonly GameState gameState;
    private readonly HttpClient httpClient;

    public IngenuityFlyer(ILogger<IngenuityFlyer> logger, GameState gameState, HttpClient httpClient)
    {
        this.logger = logger;
        this.gameState = gameState;
        this.httpClient = httpClient;
    }

    public async Task StartFlyingAsync()
    {
        var waypoints = BuildWaypointList(gameState);

        while (waypoints.Any())
        {
            (int x, int y) = waypoints.Dequeue();
            logger.LogInformation("Flying to {x},{y}", x, y);
            await flyTo(x, y);
        }

        logger.LogInformation("Finished moving...now where?");
    }

    public Queue<(int x, int y)> BuildWaypointList(GameState gameState)
    {
        var maxX = gameState.Width;
        var maxY = gameState.Height;
        var waypoints = new Queue<(int x, int y)>();

        logger.LogInformation("Starting at {ingenuityX},{ingenuityY}", gameState.IngenuityX, gameState.IngenuityY);

        foreach (var target in gameState.JoinResponse.Targets)
        {
            int directionX = target.X > gameState.IngenuityX ? 1 : -1;
            int directionY = target.Y > gameState.IngenuityY ? 1 : -1;
            logger.LogInformation("The target is {directionX} and {directionY} from my starting location", directionX == 1 ? "Left" : "Right", directionY == 1 ? "Up" : "Down");

            var targetIsToTheRight = directionX > 0;
            var targetIsAbove = directionY > 0;

            int desiredX = target.X;
            int desiredY = gameState.IngenuityY;
            waypoints.Enqueue((desiredX, desiredY));//move x to target x

            while (targetIsAbove ? desiredY < target.Y : desiredY > target.Y)
            {
                desiredY += directionY * 10;
                waypoints.Enqueue((desiredX, desiredY));

                directionX *= -1;
                if (targetIsToTheRight)
                {
                    desiredX = directionX > 0 ? target.X : gameState.IngenuityX;
                }
                else
                {
                    desiredX = directionX < 0 ? target.X : gameState.IngenuityX;
                }
                waypoints.Enqueue((desiredX, desiredY));
            }
        }

        return waypoints;
    }

    async Task flyTo(int x, int y)
    {
        if (gameState?.JoinResponse == null)
        {
            return;
        }

        int targetX = gameState.Targets.First().X;
        int targetY = gameState.Targets.First().Y;

        int directionX = targetX > gameState.IngenuityX ? 1 : -1;
        int directionY = targetY > gameState.IngenuityY ? 1 : -1;

        while (gameState.IngenuityX != x || gameState.IngenuityY != y)
        {
            int deltaX = Math.Max(-2, Math.Min(2, x - gameState.IngenuityX));
            int deltaY = Math.Max(-2, Math.Min(2, y - gameState.IngenuityY));
            await moveIngenuity(gameState.IngenuityX + deltaX, gameState.IngenuityY + deltaY);
        }
    }

    async Task moveIngenuity(int x, int y)
    {
        var response = await httpClient.GetAsync($"/game/moveingenuity?token={gameState.Token}&destinationRow={x}&destinationColumn={y}");
        if (response.IsSuccessStatusCode)
        {
            var moveResponse = await response.Content.ReadFromJsonAsync<IngenuityMoveResponse>();
            gameState.IngenuityX = moveResponse.X;
            gameState.IngenuityY = moveResponse.Y;

            gameState.UpdateMap(moveResponse.Neighbors);
        }
    }
}
