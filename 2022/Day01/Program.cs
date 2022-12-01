using System.Linq;

//var input = await File.ReadAllLinesAsync("Day01-input-test.txt");
var input = await File.ReadAllLinesAsync("Day01-input.txt");
var elfCalories = new List<long>();
var currentElfCalories = 0;
foreach (var line in input)
{
    if (string.IsNullOrWhiteSpace(line))
    {
        elfCalories.Add(currentElfCalories);
        currentElfCalories = 0;
    }
    else 
    {
        currentElfCalories += int.Parse(line);
    }
}
Console.WriteLine($"Part1: {elfCalories.Max()}");
Console.WriteLine($"Part2: {elfCalories.OrderByDescending(_ => _).Take(3).Sum()}");