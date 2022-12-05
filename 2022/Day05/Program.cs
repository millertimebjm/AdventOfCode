using AdventOfCodeCSharp.Day05;
using System.Linq;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var file = await File.ReadAllLinesAsync("input.txt");
var lines = new List<string>();
for (int i = 0; i < file.Count(); i++)
{
    if (file[i] == "")
    {
        break;
    }
    lines.Add(file[i]);
}

var cargoService = new CargoService();
lines.Reverse();
var count = int.Parse(lines.First().Replace(" ","").Last().ToString());
foreach (var line in lines.Skip(1))
{
    for(int i = 0; i < line.Length; i+=4)
    {
        if (line[i+1] != ' ')
        {
            cargoService.Crates[i/4].Push(line[i+1]);
        }
    }
}

lines = new List<string>();
var fileList = file.ToList();
fileList.Reverse();
for (int i = 0; i < fileList.Count(); i++)
{
    if (fileList[i] == "")
    {
        break;
    }
    lines.Add(fileList[i]);
}
lines.Reverse();

// Part 1 Start
// foreach (var line in lines)
// {
//     var instructions = line.Split(" ");
//     var cargoCount = int.Parse(instructions[1]);
//     var from = int.Parse(instructions[3]);
//     var to = int.Parse(instructions[5]);
//     cargoService.MoveMany(from, to, cargoCount);
// }

 var output = "";
// for (int i = 0; i < count; i++)
// {
//     if (cargoService.Crates[i].Any())
//     {
//         output += cargoService.Crates[i].First();
//     }
//     else 
//     {
//         output += " ";
//     }
// }

//Console.WriteLine(output);
// Part 1 End


// Part 2 Start
foreach (var line in lines)
{
    var instructions = line.Split(" ");
    var cargoCount = int.Parse(instructions[1]);
    var from = int.Parse(instructions[3]);
    var to = int.Parse(instructions[5]);
    cargoService.MoveManyPart2(from, to, cargoCount);
}

output = "";
for (int i = 0; i < count; i++)
{
    if (cargoService.Crates[i].Any())
    {
        output += cargoService.Crates[i].First();
    }
    else 
    {
        output += " ";
    }
}

Console.WriteLine(output);
// Part 2 End
