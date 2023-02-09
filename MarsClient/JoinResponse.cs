namespace MarsClient;

public class JoinResponse
{
    public string token { get; set; }
    public int startingRow { get; set; }
    public int startingColumn { get; set; }
    public int targetRow { get; set; }
    public int targetColumn { get; set; }
    public Neighbor[] neighbors { get; set; }
    public Lowresolutionmap[] lowResolutionMap { get; set; }
    public string orientation { get; set; }
}

public class Neighbor
{
    public int row { get; set; }
    public int column { get; set; }
    public int difficulty { get; set; }
}

public class Lowresolutionmap
{
    public int lowerLeftRow { get; set; }
    public int lowerLeftColumn { get; set; }
    public int upperRightRow { get; set; }
    public int upperRightColumn { get; set; }
    public int averageDifficulty { get; set; }
}
