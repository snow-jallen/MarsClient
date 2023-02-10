using MarsClient.Models;

namespace MarsClient;

public class JoinResponse
{
    public string Token { get; set; }
    public int StartingRow { get; set; }
    public int StartingColumn { get; set; }
    public int TargetRow { get; set; }
    public int TargetColumn { get; set; }
    public Neighbor[] Neighbors { get; set; }
    public LowResolutionMapTile[] LowResolutionMap { get; set; }
    public string Orientation { get; set; }
}

public class Neighbor
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int Difficulty { get; set; }
}
