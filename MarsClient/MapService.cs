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
            if (Map.Cells[(neighbor.Row, neighbor.Column)].IsExplored == false)
            {
                Map.Cells[(neighbor.Row, neighbor.Column)].Difficulty = neighbor.Difficulty;
                Map.Cells[(neighbor.Row, neighbor.Column)].IsExplored = true;
            }
        }
    }
}