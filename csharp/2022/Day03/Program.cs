// See https://aka.ms/new-console-template for more information
using AdventOfCodeCSharp.Day03;

Console.WriteLine("Hello, World!");

var file = await File.ReadAllLinesAsync("input.txt");
var priority = 0;
foreach (var line in file)
{
    for (int i = 0; i < line.Length/2; i++)
    {
        if (line.Substring(line.Length/2, line.Length/2).Contains(line[i]))
        {
            priority += PriorityService.GetPriority(line[i]);
            break;
        }
    }
    
}
Console.WriteLine(priority);

priority = 0;
for (int i = 0; i < file.Length; i+=3)
{
    foreach (var character in file[i])
    {
        if (file[i+1].Contains(character)
            && file[i+2].Contains(character))
            {
                priority += PriorityService.GetPriority(character);
                break;
            }
    }
}
Console.WriteLine(priority);