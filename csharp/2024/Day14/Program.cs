
public class Program {
    public static void Main(string[] args) 
    {

        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");
        var robots = LoadLines("input.txt");
        int maxX = 101;
        int maxY = 103;
        // var robots = LoadLines("input_test.txt");
        // int maxX = 1;
        // int maxY = 7;


        Part1(robots, maxX, maxY);
        Part2(robots, maxX, maxY);
    }

    public static void DisplayRobots(List<RobotModel> robots, int maxX, int maxY)
    {
        var robotsOrdered = robots.OrderBy(robot => robot.Y).ThenBy(robot => robot.X).ToList();
        int robotIndex = 0;

        for (int i = 0; i < maxY; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                bool anyFound = false;
                while (robotIndex < robotsOrdered.Count 
                    && robotsOrdered[robotIndex].X == j && robotsOrdered[robotIndex].Y == i)
                {
                    if (!anyFound) Console.Write("*");
                    robotIndex++;
                    anyFound = true;
                }
                if (!anyFound)
                {
                    Console.Write(".");
                }
                
            }
            Console.WriteLine();
        }
    }

    public static void Part2(List<RobotModel> robots, int maxX, int maxY)
    {
        int index = 1;
        IRobotService robotService = new RobotService();
        while(true)
        {
            foreach (var robot in robots)
            {
                robotService.NextPosition(robot, maxX, maxY);
            }
            Console.WriteLine(index);
            
            var robotsOrdered = robots.OrderBy(robot => robot.Y).ThenBy(robot => robot.X).ToList();
            bool duplicateFound = false;
            for (int i = 0; i < robotsOrdered.Count-2; i++)
            {
                int x1 = robotsOrdered[i].X;
                int x2 = robotsOrdered[i+1].X;
                int y1 = robotsOrdered[i].Y;
                int y2 = robotsOrdered[i+1].Y;

                if (x1 == x2 && y1 == y2)
                    duplicateFound = true;
            }
            if (!duplicateFound)
            {
                DisplayRobots(robots, maxX, maxY);
                Console.ReadLine();
            }
            index++;
        }
    }

    public static void Part1(List<RobotModel> robots, int maxX, int maxY) 
    {
        foreach (var robot in robots) 
        {
            Console.WriteLine($"{robot.X},{robot.Y} {robot.VelocityX},{robot.VelocityY}");
        }

        IRobotService robotService = new RobotService();

        var positions = robotService.GetAllPositions(robots, maxX, maxY);
        Console.WriteLine($"--{positions[0]},{positions[1]},{positions[2]},{positions[3]}--");

        for (int i = 0; i < 100; i++) 
        {
            foreach (var robot in robots)
            {
                robotService.NextPosition(robot, maxX, maxY);
            }
        }

        foreach (var robot in robots) 
        {
            Console.WriteLine($"{robot.X},{robot.Y} {robot.VelocityX},{robot.VelocityY}");
        }

        positions = robotService.GetAllPositions(robots, maxX, maxY);
        Console.WriteLine($"--{positions[0]},{positions[1]},{positions[2]},{positions[3]}--");
        Console.WriteLine($"{positions[0]*positions[1]*positions[2]*positions[3]}");
    }

    public static List<RobotModel> LoadLines(string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        List<RobotModel> robots = new();
        foreach (var line in lines) 
        {
            var robot = new RobotModel();
            var robotString = line.Split(" ");
            var locationString = robotString[0].Split(",");
            robot.X = int.Parse(locationString[0].Replace("p=", ""));
            robot.Y = int.Parse(locationString[1]);
            var velocityString = robotString[1].Split(",");
            robot.VelocityX = int.Parse(velocityString[0].Replace("v=", ""));
            robot.VelocityY = int.Parse(velocityString[1]);
            robots.Add(robot);
        }
        return robots;
    }
}

public class RobotModel
{
    public int X { get; set; } = 0;
    public int Y { get; set; } = 0;
    public int VelocityX { get; set; } = 0;
    public int VelocityY { get; set; } = 0;
}

public interface IRobotService 
{
    public void NextPosition(RobotModel robot, int maxX, int maxY);
    public List<int> GetAllPositions(List<RobotModel> robots, int maxX, int maxY);
}

public class RobotService : IRobotService
{
    public void NextPosition(RobotModel robot, int maxX, int maxY)
    {
        robot.X += robot.VelocityX;
        robot.Y += robot.VelocityY;
        if (robot.X >= maxX) robot.X = robot.X % maxX;
        if (robot.X < 0) robot.X = robot.X % maxX + maxX;
        if (robot.Y >= maxY) robot.Y = robot.Y % maxY;
        if (robot.Y < 0) robot.Y = robot.Y % maxY + maxY;
    }

    public List<int> GetAllPositions(List<RobotModel> robots, int maxX, int maxY)
    {
        List<int> positions = new() {0,0,0,0};
        foreach (var robot in robots)
        {
            var position = GetPosition(robot, maxX, maxY);
            if (position >= 0) positions[position.Value]++;
        }
        return positions;
    }

    public int? GetPosition(RobotModel robot, int maxX, int maxY)
    {
        if (robot.X < maxX / 2 && robot.Y < maxY / 2) return 0;
        if (robot.X < maxX / 2 && robot.Y >= maxY / 2 + 1) return 1;
        if (robot.X >= maxX / 2 + 1 && robot.Y < maxY / 2) return 2;
        if (robot.X >= maxX / 2 + 1 && robot.Y >= maxY / 2 + 1) return 3;
        return null;
    }
}

