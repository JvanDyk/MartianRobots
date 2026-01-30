# Martian Robots

## Run the Application
```bash
dotnet run --project ./MartianRobots/MartianRobots.csproj
```

## Run Tests
```bash
dotnet test ./MartianRobots.Tests/MartianRobots.Tests.csproj
```

## Notes
For extended behaviour of robot commands, 
I would adjust the input to consider the type of command its reading and execute behavior according to type of command and execution instructions.
movement:LFFRFF
head:LCR
lefthand:...
righthand:...
etc...

I would then append these actions to a sequence of different commands.
Like its current instructions, to be adjusted to type of command to be executed in serial.