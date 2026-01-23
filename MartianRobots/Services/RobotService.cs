using MartianRobots.Models;
using MartianRobots.Models.Enums;

namespace MartianRobots.Services;

public class RobotService
{
    const int MAX_GRID_SIZE = 50;

    public Queue<Robot> QueueOfRobots { get; private set; } = new Queue<Robot>();
    public static Grid? grid { get; private set; } = default;

    public async Task ProcessRobotsAsync(string fileLocation)
    {
        // Read grid dimensions from first line
        using (var reader = new StreamReader(fileLocation))
        {
            var readGridSize = reader.ReadLine()?.Trim().Split();
            if (readGridSize == null || readGridSize.Length < 2)
            {
                throw new Exception("Invalid grid size in input file.");
            }
            var gridWidth = uint.Parse(readGridSize[0]);
            var gridHeight = uint.Parse(readGridSize[1]);

            if (gridWidth > MAX_GRID_SIZE) gridWidth = MAX_GRID_SIZE;
            if (gridHeight > MAX_GRID_SIZE) gridHeight = MAX_GRID_SIZE;

            grid = new Grid(gridWidth, gridHeight);

            // Parse robots while reading from file
            while (!reader.EndOfStream)
            {
                try
                {
                    var robotPosition = reader.ReadLine()?.Trim().Split();
                    var robotInstructions = reader.ReadLine()?.Trim().ToCharArray();

                    if ((robotPosition == null || robotPosition.Length < 3) ||
                    (robotInstructions == null || robotInstructions?.Length == 0))
                    {
                        throw new Exception("Robot position or instructions cannot be empty.");
                    }

                    var x = int.Parse(robotPosition[0]);
                    var y = int.Parse(robotPosition[1]);
                    var orientation = Enum.Parse<OrientationEnum>(robotPosition[2]);

                    QueueOfRobots.Enqueue(new Robot(
                                    new Position(x, y),
                                    orientation,
                                    robotInstructions!));
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Error parsing robot: {ex.Message}";
                    Console.WriteLine(errorMessage);
                    await File.AppendAllTextAsync("error.log", $"{DateTime.Now}: {errorMessage}\n");
                }
                finally
                {
                    // Read empty line robot splitter
                    reader.ReadLine();
                }
            }

        }
    }
}
