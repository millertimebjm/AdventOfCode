using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("Input1Test.txt");
        var map = Map.Create(input);
        var startingCell = map.FindStart();
        startingCell.DistanceFromStart = 0;
        while (map.Cells.Any(_ => _.DistanceFromStart == null))
        {

        }
    }
}

public class Cell
{
    public int X {get; set;}
    public int Y {get; set;}
    public char Pipe {get; set;}
    public long? DistanceFromStart {get; set;}
    public List<Cell> AdjacentCells {get; set;} = new List<Cell>();
    public Cell(int x, int y, char pipe)
    {
        X = x;
        Y = y;
        Pipe = pipe;
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
            if (new List<char> {'-','F','L'}.Contains(testAdjacentCell.Pipe))
            {
                cell.AdjacentCells.Add(testAdjacentCell);
            }
            testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X && _.Y == cell.Y + 1);
            if (new List<char> {'|','J','L'}.Contains(testAdjacentCell.Pipe))
            {
                cell.AdjacentCells.Add(testAdjacentCell);
            }
        }
        if (cell.Pipe == '-' || cell.Pipe == 'S')
        {
            var testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X - 1 && _.Y == cell.Y);
            if (new List<char> {'-','F','L'}.Contains(testAdjacentCell.Pipe))
            {
                cell.AdjacentCells.Add(testAdjacentCell);
            }
            testAdjacentCell = Cells.SingleOrDefault(_ => _.X == cell.X + 1 && _.Y == cell.Y);
            if (new List<char> {'|','J','L'}.Contains(testAdjacentCell.Pipe))
            {
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