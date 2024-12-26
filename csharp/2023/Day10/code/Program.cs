using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("Input1.txt");
        var map = Map.Create(input);
        var startingCell = map.FindStart();
        startingCell.DistanceFromStart = 0;
        var mappedCells = new List<Cell>() {startingCell};
        while (mappedCells.Any(_ => !_.Mapped))
        {
            var nextCellToMap = mappedCells
                .Where(_ => _.AdjacentCells.Count < 2)
                .First(_ => _.DistanceFromStart == mappedCells.Where(mc => mc.AdjacentCells.Count < 2).Min(mc => mc.DistanceFromStart));
                
            map.GetAdjacentCells(nextCellToMap);
            Console.WriteLine($"{nextCellToMap} --- {string.Join(" --- ", nextCellToMap.AdjacentCells)}");
            mappedCells.AddRange(nextCellToMap.AdjacentCells);
            nextCellToMap.Mapped = true;
            //Console.ReadLine();
        }

        Console.WriteLine(mappedCells.Max(_ => _.DistanceFromStart));
    }
}

public class Cell
{
    public int X {get; set;}
    public int Y {get; set;}
    public char Pipe {get; set;}
    public long? DistanceFromStart {get; set;}
    public List<Cell> AdjacentCells {get; set;} = new List<Cell>();
    public bool Mapped {get; set;} = false;
    public Cell(int x, int y, char pipe)
    {
        X = x;
        Y = y;
        Pipe = pipe;
    }

    internal void UpdateDistanceFromStartIfLower(long v)
    {
        if (DistanceFromStart == null || v < DistanceFromStart)
        {
            DistanceFromStart = v;
        }
    }

    public override string ToString()
    {
        return $"({X},{Y}) => {Pipe} => {DistanceFromStart.ToString() ?? ""}";
    }
}

public class Map
{
    public List<Cell> Cells {get; set;} = new List<Cell>();

    public void GetAdjacentCells(Cell cell)
    {
        if (cell.Pipe == '7' || cell.Pipe == 'S')
        {
            var testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X - 1 && _.Y == cell.Y);
            if (new List<char> {'-','F','L','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
            testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X && _.Y == cell.Y + 1);
            if (new List<char> {'|','J','L','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
        }
        if (cell.Pipe == '-' || cell.Pipe == 'S')
        {
            var testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X - 1 && _.Y == cell.Y);
            if (new List<char> {'-','F','L','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
            testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X + 1 && _.Y == cell.Y);
            if (new List<char> {'-','J','7','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
        }
        if (cell.Pipe == 'J' || cell.Pipe == 'S')
        {
            var testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X - 1 && _.Y == cell.Y);
            if (new List<char> {'-','F','L','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
            testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X && _.Y == cell.Y - 1);
            if (new List<char> {'|','F','7','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
        }
        if (cell.Pipe == '|' || cell.Pipe == 'S')
        {
            var testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X && _.Y == cell.Y - 1);
            if (new List<char> {'|','F','7','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
            testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X && _.Y == cell.Y + 1);
            if (new List<char> {'|','L','J','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
        }
        if (cell.Pipe == 'L' || cell.Pipe == 'S')
        {
            var testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X && _.Y == cell.Y - 1);
            if (new List<char> {'|','F','7','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
            testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X + 1 && _.Y == cell.Y);
            if (new List<char> {'-','7','J','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
        }
        if (cell.Pipe == 'F' || cell.Pipe == 'S')
        {
            var testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X + 1 && _.Y == cell.Y);
            if (new List<char> {'-','J','7','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
            testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X && _.Y == cell.Y + 1);
            if (new List<char> {'|','J','L','S'}.Contains(testAdjacentCell?.Pipe ?? '0'))
            {
                testAdjacentCell.UpdateDistanceFromStartIfLower(cell.DistanceFromStart.Value + 1);
                cell.AdjacentCells.Add(testAdjacentCell);
            }
        }
    }

    public Cell FindStart()
    {
        return Cells.Single(_ => _.Pipe == 'S');
    }

    public static Map Create(string[] input)
    {
        var map = new Map();
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                map.Cells.Add(new Cell(x, y, input[y][x]));
            }
        }
        return map;
    }
}