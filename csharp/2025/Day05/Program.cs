// See https://aka.ms/new-console-template for more information
using System.Numerics;

Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("input.txt");
var identifiers = new List<Identifier>();
var identifiersToCheck = new List<BigInteger>();
foreach (var line in lines)
{
    if (line.Contains("-"))
    {
        var lineArray = line.Split("-");
        identifiers.Add(new Identifier(BigInteger.Parse(lineArray[0]), BigInteger.Parse(lineArray[1])));
        continue;
    }
    if (string.IsNullOrWhiteSpace(line))
    {
        continue;
    }
    identifiersToCheck.Add(BigInteger.Parse(line));
}

var freshCount = 0;
foreach (var identifierToCheck in identifiersToCheck)
{
    foreach (var identifier in identifiers)
    {
        if (identifier.Start <= identifierToCheck && identifier.End >= identifierToCheck)
        {
            freshCount++;
            break;
        }
    }
}

Console.WriteLine(freshCount);

public record Identifier(BigInteger Start, BigInteger End);
