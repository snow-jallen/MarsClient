namespace ConsoleClient;

public class MoveResponse
{
    public int X { get; set; }
    public int Y { get; set; }
    public int BatteryLevel { get; set; }
    public Neighbor[] Neighbors { get; set; }
    public string Message { get; set; }
    public string Orientation { get; set; }
}

public class IngenuityMoveResponse
{
    public int X { get; set; }
    public int Y { get; set; }
    public int BatteryLevel { get; set; }
    public IEnumerable<Neighbor> Neighbors { get; set; }
    public string Message { get; set; }
}

public class StatusResult
{
    public string status { get; set; }
}

public class JoinResponse
{
    public string Token { get; set; }
    public int StartingX { get; set; }
    public int StartingY { get; set; }
    public Location[] Targets { get; set; }
    public Neighbor[] Neighbors { get; set; }
    public Lowresolutionmap[] LowResolutionMap { get; set; }
    public string Orientation { get; set; }
}

public record Location(int X, int Y);

public class Neighbor
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Difficulty { get; set; }
}

public class Lowresolutionmap
{
    public int LowerLeftX { get; set; }
    public int LowerLeftY { get; set; }
    public int UpperRightX { get; set; }
    public int UpperRightY { get; set; }
    public int AverageDifficulty { get; set; }
}

