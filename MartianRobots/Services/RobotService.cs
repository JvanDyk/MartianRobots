using MartianRobots.Infrastructure.Queues;
using MartianRobots.Models;
using MartianRobots.Models.Enums;

namespace MartianRobots.Services;

public class RobotService(Position trueNorth) : RobotServiceBase(trueNorth)
{
    public override async Task ProcessRobotsAsync(string fileLocation)
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

            if (gridWidth > MaxGridSize) gridWidth = MaxGridSize;
            if (gridHeight > MaxGridSize) gridHeight = MaxGridSize;

            grid = new Grid(gridWidth, gridHeight, new HashSet<Position>(), trueNorth);

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

            QueueOfRobots.CompleteAdding();
        }
    }

    public override async Task ExecuteRobotsAsync()
    {
        using var writer = new StreamWriter("output.txt");
        foreach (var robot in QueueOfRobots.SequenceOfRobots)
        {

            robot.ExecuteInstructions(grid!);
            writer.WriteLine(robot.ToString());
            Console.WriteLine($"Processed: {robot}");
        }
        await writer.FlushAsync();
    }
}
