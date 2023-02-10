namespace MarsClient;

public class MapService
{
    public MapService()
    {

    }

    public Models.Map Map { get; set; }

    internal void UpdateMap(IEnumerable<Neighbor> neighbors)
    {
        foreach (var neighbor in neighbors)
        {
            if (Map.Cells[(neighbor.X, neighbor.Y)].IsExplored == false)
            {
                Map.Cells[(neighbor.X, neighbor.Y)].Difficulty = neighbor.Difficulty;
                Map.Cells[(neighbor.X, neighbor.Y)].IsExplored = true;
            }
        }
    }
}