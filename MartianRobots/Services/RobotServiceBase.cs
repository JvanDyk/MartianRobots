using MartianRobots.Infrastructure.Queues;
using MartianRobots.Models;

namespace MartianRobots.Services;

public abstract class RobotServiceBase(Position trueNorth)
{
    protected const int MaxGridSize = 50;
    protected readonly Position _trueNorth = trueNorth;

    public RobotQueue QueueOfRobots { get; private set; } = new();
    public static Grid? grid { get; protected set; } = default;

    public abstract Task ProcessRobotsAsync(string fileLocation);

    public abstract Task ExecuteRobotsAsync();
}
