
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

public class Program 
{
    public static async Task Main() 
    {
        var lines = await File.ReadAllLinesAsync("input.txt");
        var map = BuildMap(lines);
        Console.WriteLine(map.ToString());
        var antiNodes = map.CalculateUniqueAntiNodesPart2();
        int counter = 1;
        foreach (var antiNode in antiNodes.OrderBy(an => an.Character).ThenBy(an => an.X).ThenBy(an => an.Y))
        {
            Console.WriteLine($"{counter}: ({antiNode.X}, {antiNode.Y}, {antiNode.Character})");
            counter++;
        }
        Console.WriteLine();
        var uniqueAntiNodes = antiNodes
            .Select(an => new Tuple<int, int>(an.X, an.Y))
            .Distinct();
        counter = 1;
        foreach (var antiNode in uniqueAntiNodes.OrderBy(an => an.Item1))
        {
            Console.WriteLine($"{counter}: ({antiNode.Item1}, {antiNode.Item2})");
            counter++;
        }
    }
    
    public static Map BuildMap(string[] lines)
    {
        var map = new Map();
        int y = 0;
        foreach (var line in lines)
        {
            int x = 0;
            var row = new List<Cell>();
            foreach (var character in line)
            {
                row.Add(new Cell(x, y, character));
                x++;
            }
            map.Cells.Add(row);
            y++;
        }
        return map;
    }
}

public class Map
{
    public List<List<Cell>> Cells {get; set;} = [];
    public Cell Get(int x, int y)
    {
        return Cells[y][x];
    }
    public override string ToString()
    {
        var result = "";
        foreach (var row in Cells)
        {
            foreach (var cell in row)
            {
                result += cell.Character;
            }
            result += Environment.NewLine;
        }
        return result;
    }

    public List<Cell> CalculateUniqueAntiNodes() 
    {
        var antiNodes = new List<Cell>();
        var nodes = Cells.SelectMany(r => r.Where(c => c.Character != '.'));
        var charactersDictionary = nodes.GroupBy(c => c.Character);
        foreach (var characterGrouping in charactersDictionary) 
        {
            foreach (var cell1 in characterGrouping)
            {
                foreach (var cell2 in characterGrouping)
                {
                    if (cell1.Equals(cell2)) continue;
                    if ((cell1.X < cell2.X && cell1.Y < cell2.Y)
                        || cell1.X > cell2.X && cell1.Y > cell2.Y)
                        {
                            var xDiff = Math.Abs(cell1.X - cell2.X);
                            var yDiff = Math.Abs(cell1.Y - cell2.Y);
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X - xDiff, cell1.Y - yDiff, cell1.Character));
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X + xDiff, cell1.Y + yDiff, cell1.Character));
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell2.X - xDiff, cell2.Y - yDiff, cell1.Character));
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell2.X + xDiff, cell2.Y + yDiff, cell1.Character));
                        }
                    if ((cell1.X < cell2.X && cell1.Y > cell2.Y)
                        || (cell1.X > cell2.X && cell1.Y < cell2.Y))
                        {
                            var xDiff = Math.Abs(cell1.X - cell2.X);
                            var yDiff = Math.Abs(cell1.Y - cell2.Y);
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X - xDiff, cell1.Y + yDiff, cell1.Character));
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X + xDiff, cell1.Y - yDiff, cell1.Character));
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell2.X - xDiff, cell2.Y + yDiff, cell1.Character));
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell2.X + xDiff, cell2.Y - yDiff, cell1.Character));
                        }
                    if (cell1.Y == cell2.Y 
                        && (cell1.X < cell2.X || cell1.X > cell2.X))
                        {
                            var xDiff = Math.Abs(cell1.X - cell2.X);
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X - xDiff, cell1.Y, cell1.Character));
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X + xDiff, cell1.Y, cell1.Character));
                        }
                    if (cell1.X == cell2.X
                        && (cell1.Y < cell2.Y || cell1.Y > cell2.Y))
                        {
                            var yDiff = Math.Abs(cell1.Y - cell2.Y);
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X, cell1.Y - yDiff, cell1.Character));
                            AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X, cell1.Y + yDiff, cell1.Character));
                        }
                }
            }
        }
        return antiNodes;
    }

    public List<Cell> CalculateUniqueAntiNodesPart2() 
    {
        var antiNodes = new List<Cell>();
        var nodes = Cells.SelectMany(r => r.Where(c => c.Character != '.'));
        var charactersDictionary = nodes.GroupBy(c => c.Character);
        foreach (var characterGrouping in charactersDictionary) 
        {
            foreach (var cell1 in characterGrouping)
            {
                foreach (var cell2 in characterGrouping)
                {
                    if (cell1.Equals(cell2)) continue;
                    if ((cell1.X < cell2.X && cell1.Y < cell2.Y)
                        || cell1.X > cell2.X && cell1.Y > cell2.Y)
                        {
                            var xDiff = Math.Abs(cell1.X - cell2.X);
                            var yDiff = Math.Abs(cell1.Y - cell2.Y);
                            int counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell1.X +(- xDiff * counter), cell1.Y + (- yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                            counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell1.X + (xDiff * counter), cell1.Y + (yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                            counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell2.X +(- xDiff * counter), cell2.Y + (- yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                            counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell2.X + (xDiff * counter), cell2.Y + (yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                        }
                    if ((cell1.X < cell2.X && cell1.Y > cell2.Y)
                        || (cell1.X > cell2.X && cell1.Y < cell2.Y))
                        {
                            var xDiff = Math.Abs(cell1.X - cell2.X);
                            var yDiff = Math.Abs(cell1.Y - cell2.Y);

                            int counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell1.X + (- xDiff * counter), cell1.Y + (yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                            counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell1.X + (xDiff * counter), cell1.Y +(- yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                            counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell2.X +(- xDiff * counter), cell2.Y + (yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                            counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell2.X + (xDiff * counter), cell2.Y +(- yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                        }
                    if (cell1.Y == cell2.Y 
                        && (cell1.X < cell2.X || cell1.X > cell2.X))
                        {
                            var xDiff = Math.Abs(cell1.X - cell2.X);

                            int counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell1.X +(- xDiff * counter), cell1.Y, cell1.Character)))
                            {
                                counter++;
                            }
                            counter = 1;
                            while(AddAntiNodeIfValidPart2(antiNodes, cell1, cell2, new Cell(cell1.X + (xDiff * counter), cell1.Y, cell1.Character)))
                            {
                                counter++;
                            }
                        }
                    if (cell1.X == cell2.X
                        && (cell1.Y < cell2.Y || cell1.Y > cell2.Y))
                        {
                            var yDiff = Math.Abs(cell1.Y - cell2.Y);

                            int counter = 1;
                            while(AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X, cell1.Y +(- yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                            counter = 1;
                            while(AddAntiNodeIfValid(antiNodes, cell1, cell2, new Cell(cell1.X, cell1.Y + (yDiff * counter), cell1.Character)))
                            {
                                counter++;
                            }
                        }
                }
            }
        }
        return antiNodes;
    }

    public bool AddAntiNodeIfValid(List<Cell> antiNodes, Cell cell1, Cell cell2, Cell newCell)
    {
        if (!newCell.Equals(cell1)
            && !newCell.Equals(cell2)
            && newCell.X >= 0 
            && newCell.X < Cells.First().Count
            && newCell.Y >= 0
            && newCell.Y < Cells.Count
            && !antiNodes.Any(an => an.Equals(newCell)))
            {
                antiNodes.Add(newCell);
                return true;
            }
        return false;
    }

    public bool AddAntiNodeIfValidPart2(List<Cell> antiNodes, Cell cell1, Cell cell2, Cell newCell)
    {
        if (newCell.X >= 0 
            && newCell.X < Cells.First().Count
            && newCell.Y >= 0
            && newCell.Y < Cells.Count
            && !antiNodes.Any(an => an.Equals(newCell)))
            {
                antiNodes.Add(newCell);
                return true;
            }
        return false;
    }
}

public class Cell
{
    public int X {get; set;}
    public int Y {get; set;}
    public char Character {get; set;}
    public Cell(int x, int y, char character)
    {
        X = x;
        Y = y;
        Character = character;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not null && obj is Cell) Equals(obj as Cell);
        return base.Equals(obj);
    }
    public bool Equals(Cell c)
    {
        return Character == c.Character
            && X == c.X
            && Y == c.Y;
    }
}