// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("input.txt");
var velocities = new List<Velocity>();
foreach (var line in lines)
{
    velocities.Add(new Velocity(
        DirectionEnumFactory.Create(line[0]), 
        int.Parse(line.Substring(1))
    ));
}

var dial = 50;
var zeroes = 0;
foreach (var velocity in velocities)
{
    // dial = DialMoverService.MovePart1(dial, velocity);
    // if (dial == 0)
    // {
    //     zeroes++;
    // }
    (var newDial, var crossedCount) = DialMoverService.MovePart2(dial, velocity);
    Console.WriteLine($"{dial} - {velocity.Direction.ToString().Substring(0, 1)}{velocity.Amount} - {newDial} - {crossedCount}");
    zeroes += crossedCount;
    dial = newDial;
}

Console.WriteLine(zeroes);

public static class DialMoverService
{
    public static int MovePart1(int dial, Velocity velocity)
    {
        if (velocity.Direction == DirectionEnum.Left) dial -= velocity.Amount;
        if (velocity.Direction == DirectionEnum.Right) dial += velocity.Amount;

        if (dial >= 100) dial %= 100;
        while (dial < 0) dial += 100;

        return dial;
    } 

    public static (int, int) MovePart2(int dial, Velocity velocity)
    {
        int crossed = 0;
        var oldDial = dial;
        if (velocity.Direction == DirectionEnum.Left) dial -= velocity.Amount;
        if (velocity.Direction == DirectionEnum.Right) dial += velocity.Amount;

        var looped = false;

        while (dial >= 100) {
            dial -= 100;
            crossed++;
            looped = true;
        }
        while (dial < 0) {
            dial += 100;
            if (looped || oldDial != 0) crossed++;
            looped = true;
        }

        if (dial == 0 && !looped) crossed++;

        return (dial, crossed);
    } 
}

public record Velocity(DirectionEnum Direction, int Amount);

public enum DirectionEnum
{
    Left,
    Right,
}

public static class DirectionEnumFactory
{
    public static DirectionEnum Create(char directionChar)
    {
        switch (directionChar)
        {
            case 'L':
                return DirectionEnum.Left;
            case 'R':
                return DirectionEnum.Right;
            default:
                throw new NotImplementedException();
        }
    }
}