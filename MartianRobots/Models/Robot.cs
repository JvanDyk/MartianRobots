using MartianRobots.Models.Enums;
namespace MartianRobots.Models;

public class Robot(Position position, OrientationEnum orientation, char[] instructions)
{
    bool IsLost = false;
    public void TurnLeft()
    {
        orientation = orientation switch
        {
            OrientationEnum.N => OrientationEnum.W,
            OrientationEnum.E => OrientationEnum.N,
            OrientationEnum.S => OrientationEnum.E,
            OrientationEnum.W => OrientationEnum.S,
        };
    }

    public void TurnRight()
    {
        orientation = orientation switch
        {
            OrientationEnum.N => OrientationEnum.E,
            OrientationEnum.E => OrientationEnum.S,
            OrientationEnum.S => OrientationEnum.W,
            OrientationEnum.W => OrientationEnum.N,
        };
    }

    public void MoveForward(Grid grid)
    {
        if (IsLost) return;

        // Calculate the following position
        var _trueNorthDirection = grid.GetDirection(orientation);
        var forwardPosition = new Position(position.X + _trueNorthDirection.X, position.Y + _trueNorthDirection.Y);

        // Check if forwardPosition is invalid
        if (grid.IsPositionInvalid(forwardPosition))
        {

            // Check if scent does not exist
            if (!grid.ScentExists(position))
            {
                IsLost = true;
                grid.AddScent(position);
                return;
            }

            // Ignore this move forward instruction
            return;
        }

        // Move forward
        position = forwardPosition;
    }

    public void ExecuteInstructions(Grid grid)
    {
        foreach (var instruction in instructions)
        {
            switch (instruction)
            {
                case 'L':
                    TurnLeft();
                    break;
                case 'R':
                    TurnRight();
                    break;
                case 'F':
                    MoveForward(grid);
                    break;
            }
        }
    }

    public override string ToString() => $"{position.X} {position.Y} {orientation}{(IsLost ? " LOST" : "")}";
}
