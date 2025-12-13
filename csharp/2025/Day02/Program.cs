// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = await File.ReadAllLinesAsync("input.txt");
var line = input.First();



var ranges = line
    .Split(",")
    .Select(id => 
        new IdRange(long.Parse(id.Split("-")[0]), long.Parse(id.Split("-")[1])));

Console.WriteLine(string.Join(",", ranges.Select(r => $"{r.From}-{r.To}")));

var validIds = new List<long>();
foreach (var range in ranges)
{
    for (var i = range.From; i <= range.To; i++)
    {
        if (IdCheckerService.IsValidPart2(i.ToString()))
        {
            validIds.Add(i);
            Console.WriteLine(i);
        }
    }
}

Console.WriteLine(validIds.Sum());

public record IdRange (long From, long To);

public static class IdCheckerService
{
    public static bool IsValidPart1(string id)
    {
        if (id.Length % 2 == 1) return false;
        return id.Substring(0, id.Length/2) == id.Substring(id.Length/2, id.Length/2);
    }

    public static bool IsValidPart2(string id)
    {
        for (int i = 2; i <= id.Length; i++)
        {
            if (id.Length % i == 0)
            {
                if (string.Concat(Enumerable.Repeat(id.Substring(0, id.Length/i), i)) == id) return true;
            }
        }
        return false;
    }
}