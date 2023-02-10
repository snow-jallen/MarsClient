using MarsClient.Models;

namespace MarsClient;

public class JoinResponse
{
    public string Token { get; set; }
    public int StartingX { get; set; }
    public int StartingY { get; set; }
    public int TargetX { get; set; }
    public int TargetY { get; set; }
    public Neighbor[] Neighbors { get; set; }
    public LowResolutionMapTile[] LowResolutionMap { get; set; }
    public string Orientation { get; set; }
}

public class Neighbor
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Difficulty { get; set; }
}
