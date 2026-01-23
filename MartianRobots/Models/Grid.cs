using MartianRobots.Models.Enums;

namespace MartianRobots.Models;

public class Grid(uint width, uint height, HashSet<Position> scents, Position trueNorth)
{
    public uint Width => width;
    public uint Height => height;
    public HashSet<Position> Scents = scents;

    public bool IsPositionInvalid(Position position) =>
       position.X < 0 || position.X > width || position.Y < 0 || position.Y > height;

    public bool ScentExists(Position position) => scents.Contains(position);

    public void AddScent(Position position) => scents.Add(position);

    public Position GetDirection(OrientationEnum orientation) => orientation switch
    {
        OrientationEnum.N => trueNorth,
        OrientationEnum.E => Position.Rotate90Clockwise(trueNorth),
        OrientationEnum.S => Position.Rotate180(trueNorth),
        OrientationEnum.W => Position.Rotate270Clockwise(trueNorth),
    };
}
