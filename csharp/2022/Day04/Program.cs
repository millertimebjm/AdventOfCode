using AdventOfCodeCSharp.Day04;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var file = await File.ReadAllLinesAsync("input.txt");

var count = 0;
foreach (var line in file) 
{
    var jobs = line.Split(",");
    if (JobService.DetermineSectionEncompassAll(jobs[0], jobs[1]))
    {
        count++;
    }
    //Console.WriteLine(count);
}
Console.WriteLine(count);

count = 0;
foreach (var line in file) 
{
    var jobs = line.Split(",");
    if (JobService.DetermineSectionEncompassAny(jobs[0], jobs[1]))
    {
        count++;
    }
    //Console.WriteLine(count);
}
Console.WriteLine(count);