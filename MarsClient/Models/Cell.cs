namespace MarsClient.Models;
public class Cell : IEquatable<Cell>
{
    public Cell(int row, int col, int diff, bool isExplored = false)
    {
        Row = row;
        Col = col;
        Difficulty = diff;
        IsExplored = isExplored;
    }
    public int Row { get; }
    public int Col { get; }

    public int Difficulty { get; set; }
    public bool IsExplored { get; set; }

    public override bool Equals(object obj)
    {
        return Equals(obj as Cell);
    }

    public bool Equals(Cell other)
    {
        return other is not null &&
               Row == other.Row &&
               Col == other.Col &&
               Difficulty == other.Difficulty &&
               IsExplored == other.IsExplored;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col, Difficulty, IsExplored);
    }

    public override string ToString() => $"({Row},{Col}) {(IsExplored ? "" : "~")}{Difficulty}";

    public static bool operator ==(Cell left, Cell right)
    {
        return EqualityComparer<Cell>.Default.Equals(left, right);
    }

    public static bool operator !=(Cell left, Cell right)
    {
        return !(left == right);
    }
}
