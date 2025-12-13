// See https://aka.ms/new-console-template for more information
using System.Linq.Expressions;

Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("input-test.txt");
var rolls = 0;
var rollRemoved = false;
var rollsRemoved = 0;
do
{
    rollRemoved = false;
    for(int y = 0; y < lines.Count(); y++)
    {
        for (int x = 0; x < lines[y].Count(); x++)
        {
            if (lines[y][x] != '@') continue;

            var adjacentRolls = 0;
            if (x > 0 && lines[y][x-1] == '@') adjacentRolls++;
            if (y > 0 && lines[y-1][x] == '@') adjacentRolls++;
            if (y < lines.Count()-1 && lines[y+1][x] == '@') adjacentRolls++;
            if (x < lines[y].Length-1 && lines[y][x+1] == '@') adjacentRolls++;
            if (x > 0 && y > 0 && lines[y-1][x-1] == '@') adjacentRolls++;
            if (y < lines.Count()-1 && x < lines[y].Length-1 && lines[y+1][x+1] == '@') adjacentRolls++;

            if (x > 0 && y < lines.Count()-1 && lines[y+1][x-1] == '@') adjacentRolls++;
            if (y > 0 && x < lines[y].Length-1 && lines[y-1][x+1] == '@') adjacentRolls++;

            if (adjacentRolls < 4) 
            {
                rolls++;
                rollRemoved = true;
                rollsRemoved ++;
                var newString = "";
                for (int i = 0; i < lines[y].Length; i++)
                {
                    if (i == x) newString += '.';
                    else
                    {
                        newString += lines[y][i];
                    }
                }
                lines[y] = newString;
            }
        }
    }
} while (rollRemoved);

Console.WriteLine(rolls);


// var lines = await File.ReadAllLinesAsync("input.txt");
// var rolls = 0;
//     for(int y = 0; y < lines.Count(); y++)
//     {
//         for (int x = 0; x < lines[y].Count(); x++)
//         {
//             if (lines[y][x] != '@') continue;

//             var adjacentRolls = 0;
//             if (x > 0 && lines[y][x-1] == '@') adjacentRolls++;
//             if (y > 0 && lines[y-1][x] == '@') adjacentRolls++;
//             if (y < lines.Count()-1 && lines[y+1][x] == '@') adjacentRolls++;
//             if (x < lines[y].Length-1 && lines[y][x+1] == '@') adjacentRolls++;
//             if (x > 0 && y > 0 && lines[y-1][x-1] == '@') adjacentRolls++;
//             if (y < lines.Count()-1 && x < lines[y].Length-1 && lines[y+1][x+1] == '@') adjacentRolls++;

//             if (x > 0 && y < lines.Count()-1 && lines[y+1][x-1] == '@') adjacentRolls++;
//             if (y > 0 && x < lines[y].Length-1 && lines[y-1][x+1] == '@') adjacentRolls++;

//             if (adjacentRolls < 4) 
//             {
//                 rolls++;
//             }
//         }
//     }
