namespace MarsClient.Models;

public class LowResolutionMapTile
{
    public int LowerLeftX { get; set; }
    public int LowerLeftY { get; set; }

    public int UpperRightX { get; set; }
    public int UpperRightY { get; set; }

    public int AverageDifficulty { get; set; }
    public override string ToString() => $"({LowerLeftX},{LowerLeftY}) to ({UpperRightX},{UpperRightY}): {AverageDifficulty}";
}
