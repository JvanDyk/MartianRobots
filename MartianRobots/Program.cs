using MartianRobots.Models;
using MartianRobots.Services;
using System;

namespace MartianRobots;

internal class Program
{

    public static async Task Main(string[] args)
    {
        var fileLocation = "Data/input.txt";
        RobotServiceBase robotService = new RobotService(trueNorth: new Position(0, 1));

        // Producer task - adds robots to queue
        var producerTask = Task.Run(async () =>
        {
            await robotService.ProcessRobotsAsync(fileLocation);
        });

        // Consumer task - processes robots sequentially and writes to output file
        var consumerTask = Task.Run(async () =>
        {
            await robotService.ExecuteRobotsAsync();
        });

        await Task.WhenAll(producerTask, consumerTask);

        Console.WriteLine("Robot simulation completed. Results written to output.txt");
        Console.ReadLine();
    }
}
