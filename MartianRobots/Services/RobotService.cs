using MartianRobots.Models;
using MartianRobots.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobots.Services;

public class RobotService
{
    const int MAX_GRID_SIZE = 50;

    uint gridWidth = default;
    uint gridHeight = default;
    
    public static Grid? grid { get; private set; } = default;

    public async Task processRobotsAsync(string fileRelativeLocation)
    {
        // Read grid dimensions from first line
        using (var reader = new StreamReader(fileRelativeLocation + "/input.txt"))
        {
            var gridSize = reader.ReadLine()?.Trim().Split();
            if (gridSize == null || gridSize.Length < 2)
            {
                throw new Exception("Invalid grid size in input file.");
            }
            gridWidth = uint.Parse(gridSize[0]);
            gridHeight = uint.Parse(gridSize[1]);

            if (gridWidth > MAX_GRID_SIZE) gridWidth = MAX_GRID_SIZE;
            if (gridHeight > MAX_GRID_SIZE) gridHeight = MAX_GRID_SIZE;

            grid = new Grid(gridWidth, gridHeight);

        }
    }
}
