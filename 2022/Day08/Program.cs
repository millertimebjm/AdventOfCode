namespace AdventOfCodeCSharp.Day08
{
    class Program 
    {
        static async Task Main(string[] args)
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            

            var file = await File.ReadAllLinesAsync("input.txt");
            var treeGrid = new List<List<Tree>>();
            
            foreach (var line in file)
            {
                var row = new List<Tree>();
                foreach (var treeHeight in line)
                {
                    row.Add(new Tree() {Height = int.Parse(treeHeight.ToString())});
                }
                treeGrid.Add(row);
            }

            foreach (var row in treeGrid)
            {
                var highest = -1;
                for (int i = 0; i < row.Count; i++)
                {
                    if (row[i].Height > highest)
                    {
                        row[i].Visible = true;
                        highest = row[i].Height;
                    }
                }

                highest = -1;
                for (int i = row.Count-1; i >= 0; i--)
                {
                    if (row[i].Height > highest)
                    {
                        row[i].Visible = true;
                        highest = row[i].Height;
                    }
                }
            }

            for (int i = 0; i < treeGrid.First().Count; i++)
            {
                var highest = -1;
                foreach (var row in treeGrid)
                {
                    if (row[i].Height > highest)
                    {
                        row[i].Visible = true;
                        highest = row[i].Height;
                    }
                }

                treeGrid.Reverse();
                highest = -1;
                foreach (var row in treeGrid)
                {
                    if (row[i].Height > highest)
                    {
                        row[i].Visible = true;
                        highest = row[i].Height;
                    }
                }
                treeGrid.Reverse();
            }

            int count = 0;
            foreach (var row in treeGrid)
            {
                foreach (var tree in row)
                {
                    if (tree.Visible)
                    {
                        count++;
                    }
                }
            }

            Console.WriteLine(count);

            var rowCount = 0;
            foreach (var row in treeGrid)
            {
                var columnCount = 0;
                foreach (var column in row)
                {
                    column.ScenicScore = GetScenicScore(treeGrid, rowCount, columnCount);
                    columnCount++;
                }
                rowCount++;
            }

            var maxScenicScore = 0;
            foreach (var row in treeGrid)
            {
                foreach (var column in row)
                {
                    if (column.ScenicScore > maxScenicScore)
                    {
                        maxScenicScore = column.ScenicScore;
                    }
                }
            }

            Console.WriteLine(maxScenicScore);
        }

        public static int GetScenicScore(List<List<Tree>> treeGrid, int row, int column)
        {
            var result = 1;
            int i;
            for (i = 1; i < treeGrid[row].Count() - column; i++)
            {
                var current = treeGrid[row][column].Height;
                var distantTree = treeGrid[row][column + i].Height;
                if (treeGrid[row][column + i].Height >= treeGrid[row][column].Height)
                {
                    result *= i;
                    break;
                }
            }
            if (i == treeGrid[row].Count() - column)
            {
                result *= i-1;
            }
            for (i = 1; i <= column; i++)
            {
                var current = treeGrid[row][column].Height;
                var distantTree = treeGrid[row][column - i].Height;
                if (treeGrid[row][column - i].Height >= treeGrid[row][column].Height)
                {
                    result *= i;
                    break;
                }
            }
            if (i > column)
            {
                result *= i-1;
            }
            for (i = 1; i < treeGrid.Count() - row; i++)
            {
                var current = treeGrid[row][column].Height;
                var distantTree = treeGrid[row + i][column].Height;
                if (treeGrid[row + i][column].Height >= treeGrid[row][column].Height)
                {
                    result *= i;
                    break;
                }
            }
            if (i >= treeGrid.Count() - row)
            {
                result *= i-1;
            }
            for (i = 1; i <= row; i++)
            {
                var current = treeGrid[row][column].Height;
                var distantTree = treeGrid[row - i][column].Height;
                if (treeGrid[row - i][column].Height >= treeGrid[row][column].Height)
                {
                    result *= i;
                    break;
                }
            }
            if ( i > row)
            {
                result *= i-1;
            }
            return result;
        }
    }

    public class Tree
    {
        public int Height { get; set; } = -1;
        public bool Visible { get; set; } = false;
        public int ScenicScore { get; set; } = -1;
    }
}

