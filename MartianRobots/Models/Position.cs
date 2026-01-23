namespace MartianRobots.Models;

public struct Position(int x, int y)
{
    public int X => x;
    public int Y => y;

    public static Position Rotate90Clockwise(Position p) => new(p.Y, -p.X);
    public static Position Rotate180(Position p) => new(-p.X, -p.Y);
    public static Position Rotate270Clockwise(Position p) => new(-p.Y, p.X);
}
