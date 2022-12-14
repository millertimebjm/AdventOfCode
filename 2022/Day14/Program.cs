namespace AdventOfCodeCSharp.Day14
{
    public class Program
    {
        public async static Task Main()
        {
            var cavern = new CellType[2000, 500];
            for (int i = 0; i < cavern.GetLength(0); i++)
            {
                for (int j = 0; j < cavern.GetLength(1); j++)
                {
                    cavern[i, j] = CellType.Empty;
                }
            }
            var lowestY = 0;
            var file = await File.ReadAllLinesAsync("input.txt");
            foreach (var line in file)
            {
                string[] cellStrings = line.Split(" -> ");
                var lastCellArray = cellStrings.First().Split(",");
                var lastCell = new Cell(int.Parse(lastCellArray[0]) + 500, int.Parse(lastCellArray[1]));
                if (lastCell.Y > lowestY) lowestY = lastCell.Y;
                for (int i = 1; i < cellStrings.Count(); i++)
                {
                    var cellArray = cellStrings[i].Split(",");
                    var currentCell = new Cell(int.Parse(cellArray[0]) + 500, int.Parse(cellArray[1]));
                    BuildWall(cavern, lastCell, currentCell);
                    if (int.Parse(cellArray[1]) > lowestY) lowestY = int.Parse(cellArray[1]);
                    lastCell = currentCell;
                }
            }

            for (int i = 0; i < cavern.GetLength(0); i++)
            {
                cavern[i, lowestY + 2] = CellType.Wall;
            }

            var sandStart = new Cell(1000, 0);
            var done = false;
            var sandCount = 0;
            while (!done)
            {
                done = DropNextSand(cavern, sandStart, lowestY);
                sandCount++;
            }

            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("sandCount: " + (sandCount));
        }

        public static bool DropNextSand(CellType[,] cavern, Cell sandStart, int lowestY)
        {
            var done = false;
            var sandDone = false;
            var sandCell = new Cell(sandStart.X, sandStart.Y);
            while (!sandDone)
            {
                if (cavern[sandCell.X, sandCell.Y + 1] == CellType.Empty) sandCell.Y += 1;
                else if (cavern[sandCell.X - 1, sandCell.Y + 1] == CellType.Empty)
                {
                    sandCell.X -= 1;
                    sandCell.Y += 1;
                }
                else if (cavern[sandCell.X + 1, sandCell.Y + 1] == CellType.Empty)
                {
                    sandCell.X += 1;
                    sandCell.Y += 1;
                }
                else
                {
                    sandDone = true;
                    cavern[sandCell.X, sandCell.Y] = CellType.Sand;
                    if (sandCell.X == sandStart.X && sandCell.Y == sandStart.Y)
                    {
                        done = true;
                    }
                }

                // if (sandCell.Y > lowestY)
                // {
                //     sandDone = true;
                //     done = true;
                // }
            }
            return done;
        }

        public static void BuildWall(CellType[,] cavern, Cell lastCell, Cell currentCell)
        {
            if (lastCell.X == currentCell.X && lastCell.Y <= currentCell.Y)
            {
                for (int i = lastCell.Y; i <= currentCell.Y; i++)
                {
                    cavern[lastCell.X, i] = CellType.Wall;
                }
            }
            if (lastCell.X == currentCell.X && lastCell.Y >= currentCell.Y)
            {
                for (int i = currentCell.Y; i <= lastCell.Y; i++)
                {
                    cavern[lastCell.X, i] = CellType.Wall;
                }
            }
            if (lastCell.Y == currentCell.Y && lastCell.X <= currentCell.X)
            {
                for (int i = lastCell.X; i <= currentCell.X; i++)
                {
                    cavern[i, currentCell.Y] = CellType.Wall;
                }
            }
            if (lastCell.Y == currentCell.Y && lastCell.X >= currentCell.X)
            {
                for (int i = currentCell.X; i <= lastCell.X; i++)
                {
                    cavern[i, currentCell.Y] = CellType.Wall;
                }
            }
        }
    }

    public enum CellType
    {
        Empty,
        Sand,
        Wall,
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
    }
}


