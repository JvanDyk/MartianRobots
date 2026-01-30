using FluentAssertions;
using MartianRobots.Models;
using MartianRobots.Services;

namespace MartianRobots.Tests.UnitTests;

public class RobotServiceTests
{
    // We create a new instance per test,
    // because the Grid inside this service is static, to be shared among all robots
    // and could interfere with other tests if reused.
    public RobotServiceBase CreateInstance()
    {
        return new RobotService(trueNorth: new Position(0, 1));
    }

    [Fact]
    public async Task TestRobotService_Given_inputData_ShouldNot_CreateGrid()
    {
        RobotServiceBase _service = CreateInstance();
        var fileLocation = "Data/invalidGrid.txt";

        await Assert.ThrowsAsync<Exception>(() => _service.ProcessRobotsAsync(fileLocation));
    }

    [Fact]
    public async Task TestRobotService_Given_inputData_Should_CreateGrid()
    {
        RobotServiceBase _service = CreateInstance();

        var fileLocation = "Data/input.txt";

        await _service.ProcessRobotsAsync(fileLocation);

        RobotService.grid.Width.Should().Be(5);
        RobotService.grid.Height.Should().Be(3);
    }

    [Fact]
    public async Task TestRobotService_Given_invalidRobotData_Should_HandleExceptionsAndLogErrors()
    {
        RobotServiceBase _service = CreateInstance();
        var fileRelativeLocation = "Data/invalidRobot.txt";

        if (File.Exists("error.log"))
        {
            File.Delete("error.log");
        }

        await _service.ProcessRobotsAsync(fileRelativeLocation);

        File.Exists("error.log").Should().BeTrue();
        var errorLogContent = await File.ReadAllTextAsync("error.log");
        errorLogContent.Should().NotBeEmpty();
        errorLogContent.Should().Contain("Error parsing robot");

        _service.QueueOfRobots.GetSnapshot().Should().HaveCountLessThan(3);
        _service.QueueOfRobots.GetSnapshot().Should().HaveCount(2);

    }

    [Fact]
    public async Task TestRobotService_Given_inputData_Should_Queue3Robots()
    {
        RobotServiceBase _service = CreateInstance();
        var fileRelativeLocation = "Data/input.txt";

        await _service.ProcessRobotsAsync(fileRelativeLocation);

        _service.QueueOfRobots.GetSnapshot().Should().NotBeEmpty();
        _service.QueueOfRobots.GetSnapshot().Should().HaveCount(3);
    }

    [Fact]
    public async Task ExecuteRobotsAsync_Given_RobotAtEdge_ShouldBe_lostWithScent()
    {
        RobotServiceBase _service = CreateInstance();
        var fileLocation = "Data/edgeRobot.txt";

        await _service.ProcessRobotsAsync(fileLocation);
        await _service.ExecuteRobotsAsync();

        RobotServiceBase.grid.Scents.Should().NotBeEmpty();

        var output = await File.ReadAllTextAsync("output.txt");
        output.Should().Contain("LOST");
    }

    [Fact]
    public async Task ExecuteRobotsAsync_Given_2RobotAtEdge_1Should_Survive()
    {
        RobotServiceBase _service = CreateInstance();
        var fileLocation = "Data/edgeRobot.txt";

        await _service.ProcessRobotsAsync(fileLocation);
        await _service.ExecuteRobotsAsync();

        RobotServiceBase.grid.Scents.Should().NotBeEmpty();

        var output = await File.ReadAllTextAsync("output.txt");
        var lostCount = output.Split('\n', StringSplitOptions.RemoveEmptyEntries).Count(line => line.Contains("LOST"));
        lostCount.Should().Be(1, "Only one robot should be lost, the other should survive due to scent");
    }


}
