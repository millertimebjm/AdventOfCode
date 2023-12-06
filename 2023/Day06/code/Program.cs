using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;

public class Program
{
    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("Input1Test.txt");
        
        var line = input[0];
        var timeString = line.Replace("Time:", "");
        // var timeArray = timeString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var timeArray = new[] { timeString.Replace(" ", "") };

        line = input[1];
        var distanceString = line.Replace("Distance:", "");
        // var distanceArray = distanceString.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        var distanceArray = new[] { distanceString.Replace(" ", "") };
        

        var race = new Race();
        for(int i = 0; i < timeArray.Length; i++)
        {
            race.Timings.Add(new Timing(long.Parse(timeArray[i]), long.Parse(distanceArray[i])));
        }
        
        var result = 1;
        foreach (var timing in race.Timings)
        {
            result *= timing.CalculateBeatDistance().Count;
        }
        Console.WriteLine(result);
    }
}