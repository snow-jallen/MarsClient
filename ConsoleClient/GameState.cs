using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ConsoleClient;

public class GameState
{
    private readonly ILogger<GameState> logger;

    public GameState(ILogger<GameState> logger)
    {
        this.logger = logger;
    }

    private JoinResponse? joinResponse;
    public JoinResponse? JoinResponse
    {
        get => joinResponse;
        set
        {
            joinResponse = value ?? throw new ArgumentNullException();

            logger.LogInformation("Joined game: target at ({targetX},{targetY})", joinResponse.TargetX, joinResponse.TargetY);

            IngenuityX = joinResponse.StartingX;
            IngenuityY = joinResponse.StartingY;
            Token = joinResponse.Token;
            Height = joinResponse.LowResolutionMap.Max(t => t.UpperRightY);
            Width = joinResponse.LowResolutionMap.Max(w => w.UpperRightX);
            Orientation = joinResponse.Orientation;
            Target = (joinResponse.TargetX, joinResponse.TargetY);
        }
    }

    public int IngenuityY { get; internal set; }
    public int IngenuityX { get; internal set; }
    public string Token { get; internal set; }
    public int Height { get; private set; }
    public int Width { get; private set; }
    public string Orientation { get; set; }
    public (int TargetX, int TargetY) Target { get; private set; }
    public ConcurrentDictionary<(int, int), int> Map { get; } = new();
    public int PerseveranceBatteryLevel { get; internal set; }
    public (int X, int Y) Perseverance { get; internal set; }

    public int GetDifficulty(int x, int y)
    {
        if (Map.ContainsKey((x, y)))
        {
            return Map[(x, y)];
        }

        return joinResponse?.LowResolutionMap.Single(t => t.LowerLeftX <= x && t.LowerLeftY <= y && t.UpperRightX <= x && t.UpperRightY <= y).AverageDifficulty ?? -1;
    }

    internal void UpdateMap(IEnumerable<Neighbor> neighbors)
    {
        foreach (var neighbor in neighbors)
        {
            Map.AddOrUpdate(
                (neighbor.X, neighbor.Y), //key
                neighbor.Difficulty, //value (if no entry with key exists)
                (location, val) => neighbor.Difficulty //lambda to update existing value
            );
        }
    }
}
