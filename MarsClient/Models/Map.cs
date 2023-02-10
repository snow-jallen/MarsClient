namespace MarsClient.Models;

public class Map
{
    private Map(IEnumerable<Cell> cells)
    {
        Cells = new(cells.Select(c => new KeyValuePair<(int, int), Cell>((c.X, c.Y), c)));
    }

    public Map(IEnumerable<LowResolutionMapTile> lowResMapTiles)
    {
        cells = new();
        foreach (var tile in lowResMapTiles)
        {
            for (int x = tile.LowerLeftX; x <= tile.UpperRightX; x++)
            {
                for (int y = tile.LowerLeftY; y <= tile.UpperRightY; y++)
                {
                    cells.Add((x, y), new Cell(x, y, tile.AverageDifficulty));
                }
            }
        }
        Cells = cells;
    }

    private readonly Dictionary<(int, int), Cell> cells;

    public int Height { get; private set; }
    public int Width { get; private set; }

    public Dictionary<(int, int), Cell> Cells
    {
        get { return cells; }
        init
        {
            cells = value;
            Height = cells.Max(c => c.Value.Y);
            Width = cells.Max(c => c.Value.X);
        }
    }

    public ViewableCells GetCellsInView((int x, int y) location, Orientation orientation)
    {
        (int r, int c) offset(int deltaX, int deltaY) => (location.x + deltaX, location.y + deltaY);

        Cell getCell(int deltaX, int deltaY)
        {
            var location = offset(deltaX, deltaY);
            if (Cells.ContainsKey(location))
            {
                return Cells[location];
            }
            return null;
        }

        var viewable = orientation switch
        {
            Orientation.North => new ViewableCells
            {
                LLUU = getCell(-2, 2),
                LUU = getCell(-1, 2),
                UU = getCell(0, 2),
                RUU = getCell(1, 2),
                RRUU = getCell(2, 2),

                LLU = getCell(-2, 1),
                LU = getCell(-1, 1),
                U = getCell(0, 1),
                RU = getCell(1, 1),
                RRU = getCell(2, 1),

                LL = getCell(-2, 0),
                L = getCell(-1, 0),
                Me = getCell(0, 0),
                R = getCell(1, 0),
                RR = getCell(2, 0),
            },
            Orientation.East => new ViewableCells
            {
                LLUU = getCell(2, 2),
                LUU = getCell(2, 1),
                UU = getCell(2, 0),
                RUU = getCell(2, -1),
                RRUU = getCell(2, -2),

                LLU = getCell(1, 2),
                LU = getCell(1, 1),
                U = getCell(1, 0),
                RU = getCell(1, -1),
                RRU = getCell(1, -2),

                LL = getCell(0, 2),
                L = getCell(0, 1),
                Me = getCell(0, 0),
                R = getCell(0, -1),
                RR = getCell(0, -2),
            },
            Orientation.South => new ViewableCells
            {
                LLUU = getCell(2, -2),
                LUU = getCell(1, -2),
                UU = getCell(0, -2),
                RUU = getCell(-1, -2),
                RRUU = getCell(-2, -2),

                LLU = getCell(2, -1),
                LU = getCell(1, -1),
                U = getCell(0, -1),
                RU = getCell(-1, -1),
                RRU = getCell(-2, -1),

                LL = getCell(2, 0),
                L = getCell(1, 0),
                Me = getCell(0, 0),
                R = getCell(-1, 0),
                RR = getCell(-2, 0),
            },
            Orientation.West => new ViewableCells
            {
                LLUU = getCell(-2, -2),
                LUU = getCell(-2, -1),
                UU = getCell(-2, 0),
                RUU = getCell(-2, 1),
                RRUU = getCell(-2, 2),

                LLU = getCell(-1, -2),
                LU = getCell(-1, -1),
                U = getCell(-1, 0),
                RU = getCell(-1, 1),
                RRU = getCell(-1, 2),

                LL = getCell(0, -2),
                L = getCell(0, -1),
                Me = getCell(0, 0),
                R = getCell(0, 1),
                RR = getCell(0, 2),
            }
        };
        return viewable;
    }

    public static Map CreateFromCells(IEnumerable<Cell> cells) => new Map(cells);
}
