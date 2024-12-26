using System.Linq;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var file = await File.ReadAllLinesAsync("input.txt");
var window = 4;
var line = file.First();
for (int i = 0; i < line.Length-window; i++)
{
    var substring = line.Substring(i, window);
    var uniqueCharacters = substring.Distinct().ToList();
    if (substring.Length == uniqueCharacters.Count())
    {
        Console.WriteLine(i+window);
        break;
    }
}

// Part 2
window = 14;
for (int i = 0; i < line.Length-window; i++)
{
    var substring = line.Substring(i, window);
    var uniqueCharacters = substring.Distinct().ToList();
    if (substring.Length == uniqueCharacters.Count())
    {
        Console.WriteLine(i+window);
        break;
    }
}