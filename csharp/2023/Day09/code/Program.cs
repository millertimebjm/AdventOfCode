using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

public class Program
{
    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("Input1.txt");
        var patterns = new List<PatternPart2>();
        long total = 0;
        foreach (var line in input)
        {
            var pattern = new PatternPart2();
            pattern.Items.Add(line.Split(" ").Select(_ => long.Parse(_)).ToList());
            var current = PatternPart2.GenerateNextItem(pattern.Items.First());
            Console.WriteLine($"Current = {current}");
            total += current;

        }

        Console.WriteLine(total);
    }
}

public class Pattern 
{
    public List<List<long>> Items {get; set;} = new List<List<long>>();
    public static long GenerateNextItem(List<long> oldItems)
    {
        var newItems = new List<long>();
        for(int i = 0; i < oldItems.Count - 1; i++)
        {
            newItems.Add(oldItems[i+1] - oldItems[i]);
        }
        
        if (newItems.All(_ => _ == 0))
        {
            return oldItems.Last();
        } 
        else 
        {
            var newNumber = GenerateNextItem(newItems);
            Console.WriteLine($"{newNumber} {oldItems.Last()}");
            return newNumber + oldItems.Last();
        }
    }
}

public class PatternPart2
{
    public List<List<long>> Items {get; set;} = new List<List<long>>();
    public static long GenerateNextItem(List<long> oldItems)
    {
        var newItems = new List<long>();
        for(int i = 0; i < oldItems.Count - 1; i++)
        {
            newItems.Add(oldItems[i+1] - oldItems[i]);
        }
        
        if (newItems.All(_ => _ == 0))
        {
            return oldItems.First();
        } 
        else 
        {
            var newNumber = GenerateNextItem(newItems);
            Console.WriteLine($"{newNumber} {oldItems.First()}");
            return oldItems.First() - newNumber;
        }
    }
}