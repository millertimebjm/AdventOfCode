public class Program
{

    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("Day03Input1Test.txt");
        var parts = new List<Part>();
        Part currentPart = null;

        for (int y = 0; y < input.Length; y++)
        {
            var line = input[y];
            for (int x = 0; x < line.Length; x++)
            {
                var point = line[x];
                if (Char.IsDigit(point))
                {
                    if (currentPart != null)
                    {
                        currentPart.Points.Add(new Point(x, y, int.Parse(point.ToString())));
                    }
                    else if (currentPart == null)
                    {
                        currentPart = new Part();
                        currentPart.Points.Add(new Point(x, y, int.Parse(point.ToString())));
                    }
                }
                else 
                {
                    if (currentPart != null)
                    {
                        parts.Add(currentPart);
                        currentPart = null;
                    }
                }
            }
            if (currentPart != null)
            {
                parts.Add(currentPart);
                currentPart = null;
            }
        }

        var total = 0;
        foreach (var part in parts)
        {
            part.FoundSymbol = FindPartSymbol(part, input);                
            if (part.FoundSymbol)
            {
                var partNumber = "";
                foreach (var point in part.Points)
                {
                    partNumber += point.Value.ToString();
                }
                total += int.Parse(partNumber);
            }
        }

        var filePath = @"output.txt";
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var part in parts)
            {
                foreach (var point in part.Points)
                {
                    writer.Write($"{point.Value}");
                }
                writer.Write($" ");
                if (part.FoundSymbol)
                    writer.Write("Found");
                writer.Write(" ");
                foreach (var point in part.Points)
                {
                    writer.Write($"{point.X},{point.Y} ");
                }
                writer.WriteLine();
            }

            writer.WriteLine(total);
        }


    }

    private static bool FindPartSymbol(Part part, string[] input) 
    {
        foreach (var point in part.Points)
        {
            part.SymbolPoint = FindPointSymbol(point, input);
            if (part.SymbolPoint != null)
                return true;
        }
        return false;
    }

    private static Point FindPointSymbol(Point point, string[] input) 
    {
        string symbols = @"!\""#$%&'()*+,-/:;<=>?@[\]^_`{|}~";
        if (point.Y > 0 && point.X > 0)
            if (symbols.Contains(input[point.Y - 1][point.X - 1]))
                return new Point(point.X - 1, point.Y - 1, input[point.Y - 1][point.X - 1]);

        if (point.Y > 0)
            if (symbols.Contains(input[point.Y - 1][point.X]))
                return new Point(point.X, point.Y - 1, input[point.Y - 1][point.X]);

        if (point.X > 0)
            if (symbols.Contains(input[point.Y][point.X - 1]))
                return new Point(point.X - 1, point.Y, input[point.Y][point.X - 1]);    
        
        if ((point.Y+1) < input.Length)
            if (symbols.Contains(input[point.Y + 1][point.X]))
                return new Point(point.X, point.Y + 1, input[point.Y + 1][point.X]);

        if ((point.X+1) < input[0].Length)
            if (symbols.Contains(input[point.Y][point.X + 1]))
                return new Point(point.X + 1, point.Y, input[point.Y][point.X + 1]);    

        if ((point.Y+1) < input.Length && (point.X+1) < input[0].Length)
            if (symbols.Contains(input[point.Y + 1][point.X + 1]))
                return new Point(point.X + 1, point.Y + 1, input[point.Y + 1][point.X + 1]);    

        if ((point.Y+1) < input.Length && point.X > 0)
            if (symbols.Contains(input[point.Y + 1][point.X - 1]))
                return new Point(point.X - 1, point.Y + 1, input[point.Y + 1][point.X - 1]);    

        if (point.Y > 0 && (point.X+1) < input[0].Length)
            if (symbols.Contains(input[point.Y - 1][point.X + 1]))
                return new Point(point.X + 1, point.Y - 1, input[point.Y - 1][point.X + 1]);    

        return null;    
    }
}

public class Part
{
    public List<Point> Points {get; set;} = new List<Point>();
    public bool FoundSymbol = false;
    public Point SymbolPoint {get; set;}
}

public class Point
{
    public int X {get; set;}
    public int Y {get; set;}
    public int Value {get; set;}
    public char Symbol {get; set;}

    public Point(int xInput, int yInput, int value)
    {
        this.X = xInput;
        this.Y = yInput;
        Value = value;
        Symbol = ' ';
    }

    public Point(int xInput, int yInput, char symbol)
    {
        this.X = xInput;
        this.Y = yInput;
        Value = 0;
        Symbol = symbol;
    }
}