namespace MarsClient.Models;

public class Map
{
    private Map(IEnumerable<Cell> cells)
    {
        Cells = new(cells.Select(c => new KeyValuePair<(int, int), Cell>((c.Row, c.Col), c)));
    }

    public Map(IEnumerable<LowResolutionMapTile> lowResMapTiles)
    {
        //Cells = new(from tile in lowResMapTiles
        //            let tileA = tile
        //            from row in Enumerable.Range(tile.LowerLeftRow, tile.UpperRightRow + 1)
        //            from col in Enumerable.Range(tile.LowerLeftColumn, tile.UpperRightColumn + 1)
        //            select new KeyValuePair<(int, int), Cell>((row, col), new Cell(row, col, tile.AverageDifficulty)));

        cells = new();
        foreach (var tile in lowResMapTiles)
        {
            for (int row = tile.LowerLeftRow; row <= tile.UpperRightRow; row++)
            {
                for (int col = tile.LowerLeftColumn; col <= tile.UpperRightColumn; col++)
                {
                    cells.Add((row, col), new Cell(row, col, tile.AverageDifficulty));
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
            Height = cells.Max(c => c.Value.Col);
            Width = cells.Max(c => c.Value.Row);
        }
    }

    public ViewableCells GetCellsInView((int row, int col) location, Orientation orientation)
    {
        (int r, int c) offset(int deltaRow, int deltaCol) => (location.row + deltaRow, location.col + deltaCol);

        Cell getCell(int deltaCol, int deltaRow)
        {
            var location = offset(deltaRow, deltaCol);
            if (Cells.ContainsKey(location))
            {
                return Cells[location];
            }
            return null;
        }

        return orientation switch
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
    }

    public static Map CreateFromCells(IEnumerable<Cell> cells) => new Map(cells);
}
