namespace MarsClient.Models;
public class Cell : IEquatable<Cell>
{
    public Cell(int x, int y, int diff, bool isExplored = false)
    {
        X = x;
        Y = y;
        Difficulty = diff;
        IsExplored = isExplored;
    }
    public int X { get; }
    public int Y { get; }
    public Color Color
    {
        get
        {
            float ratio = (float)Difficulty / 300;

            if (ratio < 0.5f)
            {
                return Color.FromRgb(ratio * 2, 1, 0);
            }
            else
            {
                return Color.FromRgb(1, (1 - ratio) * 2, 0);
            }
        }
    }
    public int Difficulty { get; set; }
    public bool IsExplored { get; set; }

    public override bool Equals(object obj)
    {
        return Equals(obj as Cell);
    }

    public bool Equals(Cell other)
    {
        return other is not null &&
               X == other.X &&
               Y == other.Y &&
               Difficulty == other.Difficulty &&
               IsExplored == other.IsExplored;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Difficulty, IsExplored);
    }

    public override string ToString() => $"({X},{Y}) {(IsExplored ? "" : "~")}{Difficulty}";

    public static bool operator ==(Cell left, Cell right)
    {
        return EqualityComparer<Cell>.Default.Equals(left, right);
    }

    public static bool operator !=(Cell left, Cell right)
    {
        return !(left == right);
    }
}
