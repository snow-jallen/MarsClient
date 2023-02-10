namespace MarsClient.Models;

public class LowResolutionMapTile
{
    public int LowerLeftRow { get; set; }
    public int LowerLeftColumn { get; set; }

    public int UpperRightRow { get; set; }
    public int UpperRightColumn { get; set; }

    public int AverageDifficulty { get; set; }
    public override string ToString() => $"({LowerLeftRow},{LowerLeftColumn}) to ({UpperRightRow},{UpperRightColumn}): {AverageDifficulty}";
}
