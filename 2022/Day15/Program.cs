namespace AdventOfCodeCSharp.Day15
{
    public class Program
    {
        public async static Task Main()
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            var file = await File.ReadAllLinesAsync("input.txt");
            var tuples = new List<Tuple<Cell, Cell>>();
            foreach (var line in file)
            {
                var cellStrings = line
                    .Replace("Sensor at ", "")
                    .Replace(" closest beacon is at ", "")
                    .Replace(" ", "")
                    .Replace("x=", "")
                    .Replace("y=", "")
                    .Split(":");
                var cellString1 = cellStrings[0].Split(",");
                var cellString2 = cellStrings[1].Split(",");
                tuples.Add(new Tuple<Cell, Cell>(new Cell(int.Parse(cellString1[0]), int.Parse(cellString1[1])), new Cell(int.Parse(cellString2[0]), int.Parse(cellString2[1]))));
            }

            //var magicY = 10;
            var magicY = 2000000;
            var result = new List<Cell>();
            var beacons = tuples.Select(_ => _.Item2).ToList();
            var signals = tuples.Select(_ => _.Item1).ToList();
            foreach (var set in tuples)
            {
                var distance = Cell.GetDistance(set.Item1, set.Item2);
                for (int i = -distance; i <= distance; i++)
                {
                    var currentY = set.Item1.Y + i;
                    if (currentY == magicY)
                    {
                        var jLength = distance - Math.Abs(i);
                        for (int j = -jLength; j <= jLength; j++)
                        {
                            var currentX = set.Item1.X + j;

                            var checkCell = new Cell(currentX, currentY);
                            if (!result.Contains(checkCell)
                                && !beacons.Contains(checkCell)
                                && !signals.Contains(checkCell))
                            {
                                result.Add(checkCell);
                            }

                        }
                    }

                }
            }
            Console.WriteLine(result.Count());
            result = result.OrderBy(_ => _.X).ToList();
        }
    }

    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public static int GetDistance(Cell cell1, Cell cell2)
        {
            return Math.Abs(cell1.X - cell2.X) + Math.Abs(cell1.Y - cell2.Y);
        }
        public bool Equals(Cell c)
        {
            if (X == c.X && Y == c.Y) return true;
            return false;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not null && obj is Cell)
            {
                return Equals((Cell)obj);
            }
            return base.Equals(obj);
        }
    }
}


