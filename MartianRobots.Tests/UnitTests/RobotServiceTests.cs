using FluentAssertions;
using MartianRobots.Services;

namespace MartianRobots.Tests.UnitTests;

public class RobotServiceTests
{
    private readonly RobotService _service = new();

    [Fact]
    public async Task TestRobotService_Given_inputData_Should_CreateGrid()
    {
        var fileRelativeLocation = "Data";

        await _service.processRobotsAsync(fileRelativeLocation);

        RobotService.grid.Width.Should().Be(5);
        RobotService.grid.Height.Should().Be(3);
    }
}
