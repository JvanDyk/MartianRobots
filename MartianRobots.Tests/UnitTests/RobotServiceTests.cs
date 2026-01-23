using FluentAssertions;
using MartianRobots.Services;

namespace MartianRobots.Tests.UnitTests;

public class RobotServiceTests
{
    private readonly RobotService _service = new();

    [Fact]
    public async Task TestRobotService_Given_inputData_ShouldNot_CreateGrid()
    {
        var fileLocation = "Data/invalidGrid.txt";

        await Assert.ThrowsAsync<Exception>(() => _service.ProcessRobotsAsync(fileLocation));
    }

    [Fact]
    public async Task TestRobotService_Given_inputData_Should_CreateGrid()
    {
        var fileLocation = "Data/input.txt";

        await _service.ProcessRobotsAsync(fileLocation);

        RobotService.grid.Width.Should().Be(5);
        RobotService.grid.Height.Should().Be(3);
    }

    [Fact]
    public async Task TestRobotService_Given_invalidRobotData_Should_HandleExceptionsAndLogErrors()
    {
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

        _service.QueueOfRobots.Should().HaveCountLessThan(3);
        _service.QueueOfRobots.Should().HaveCount(2);

    }

    [Fact]
    public async Task TestRobotService_Given_inputData_Should_Queue3Robots()
    {
        var fileRelativeLocation = "Data/input.txt";

        await _service.ProcessRobotsAsync(fileRelativeLocation);

        _service.QueueOfRobots.Should().NotBeEmpty();
        _service.QueueOfRobots.Should().HaveCount(3);
    }

   
}
