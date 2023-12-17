using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static async Task Main()
    {
        string[] input = await File.ReadAllLinesAsync("Input1Test.txt");

    }
}

public class Map
{
    public List<Cell> Cells {get; set;} = new List<Cell>();
    public int XOriginal {get; set;}
    public int YOriginal {get; set;}
    public int XNew {get; set;}
    public int YNew{get;set;}
    public Map(int xOriginal, int yOriginal)
    {
        XOriginal = xOriginal;
        YOriginal = yOriginal;
    }

    public static Map InitializeMap(string[] input)
    {
        var map = new Map(input[0].Length, input.Length);
        for(int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                map.Cells.Add(new Cell(x, y, input[y][x]));
            }
        }
        return map;
    }

    public void ExpandMap()
    {
        List<int> yExpand = new List<int>();
        List<int> xExpand = new List<int>();

        foreach (var x in Cells
            .Where(_ => _.Symbol == '.')
            .GroupBy(_ => _.X)
            .Where(_ => _.Count() == XNew)
            .Select(_ => _.Key))
        {
            ExpandMapX(x);
        }

        foreach (var y in Cells
            .Where(_ => _.Symbol == '.')
            .GroupBy(_ => _.Y)
            .Where(_ => _.Count() == YNew)
            .Select(_ => _.Key))
        {
            ExpandMapY(y);
        }
    }

    public void ExpandMapX(int x)
    {
        for (int i = XNew; i > x; i++)
        {
            foreach (var cell in Cells.Where(_ => _.X == i-1).ToList())
            {
                Cells.Add(new Cell(i, cell.Y, cell.Symbol));
            }
        }
        XNew++;
    }

    public void ExpandMapY(int y)
    {
        for (int i = YNew; i > y; i++)
        {
            foreach (var cell in Cells.Where(_ => _.Y == i-1).ToList())
            {
                Cells.Add(new Cell(cell.X, i, cell.Symbol));
            }
        }
        YNew++;
    }

    
}

public class Cell
{
    public int X {get; set;}
    public int Y {get; set;}
    public char Symbol {get; set;}

    public Cell(int x, int y, char symbol)
    {
        X = x;
        Y = y;
        Symbol = symbol;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Cell)
            return Equals((Cell)obj);
        return base.Equals(obj);
    }

    public bool Equals(Cell cell)
    {
        return this.X == cell.X && this.Y == cell.Y;
    }
}