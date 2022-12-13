namespace AdventOfCodeCSharp.Day11
{
    public class Program
    {
        public async static Task Main()
        {
            //See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            var file = await File.ReadAllLinesAsync("input.txt");
            var xLength = file.First().Length;
            Console.WriteLine("xLength: " + xLength);
            var yLength = file.Length;
            Console.WriteLine("yLength: " + yLength);
            Cell[,] grid = new Cell[xLength, yLength];
            Cell startLocation = null;
            Cell endLocation = null;
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    grid[x,y] = new Cell(x, y);
                    grid[x,y].Value = file[y][x];
                    if (file[y][x] == 'S')
                    {
                        startLocation = grid[x,y];
                        startLocation.Value = 'a';
                    }
                    if (file[y][x] == 'E')
                    {
                        endLocation = grid[x,y];
                        endLocation.Value = 'z';
                    }
                }
            }
            
            startLocation.ShortestPath = 0;
            startLocation.Visit(grid);
            while(endLocation.ShortestPath == int.MaxValue)
            {
                var cell = Cell.GetLowestDistanceCell(grid);
                cell.Visit(grid);
            }
            Console.WriteLine(endLocation.ShortestPath);
        }
    }

    public class Cell
    {
        public int X {get; set;}
        public int Y {get; set;}
        public char Value {get; set;}
        public bool Visited {get; set;} = false;
        public int ShortestPath {get; set;} = int.MaxValue;
        public Cell ShortestPathCell {get; set;} = null;

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public bool Equals(Cell cell)
        {
            if (cell.X == X
                && cell.Y == Y)
                {
                    return true;
                }
            return false;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Cell)
            {
                return Equals((Cell)obj);
            }
            return base.Equals(obj);
        }

        public static List<Cell> Clone(List<Cell> cells)
        {
            var newList = new List<Cell>();
            foreach (var cell in cells)
            {
                newList.Add(new Cell(cell.X, cell.Y));
            }
            return newList;
        }

        public static Cell GetLowestDistanceCell(Cell[,] grid)
        {
            Cell nextCell = null;
            var shortestPath = int.MaxValue;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x,y].ShortestPath < shortestPath && !grid[x,y].Visited)
                    {
                        nextCell = grid[x,y];
                        shortestPath = grid[x,y].ShortestPath;
                    }
                }
            }
            return nextCell;
        }

        public void Visit(Cell[,] grid)
        {
            List<Cell> nextCellCoords = new List<Cell>()
            {
                new Cell(X + 1, Y),
                new Cell(X - 1, Y),
                new Cell(X, Y+1),
                new Cell(X, Y-1),
            };

            foreach (var nextCellCoord in nextCellCoords)
            {
                if (nextCellCoord.X < grid.GetLength(0)
                    && nextCellCoord.X >= 0
                    && nextCellCoord.Y < grid.GetLength(1)
                    && nextCellCoord.Y >= 0)
                    {
                        var nextCell = grid[nextCellCoord.X, nextCellCoord.Y];
                        if ((int)(nextCell.Value) <= ((int)Value + 1)
                            || (nextCell.Value) == 'a'
                            || (nextCell.Value) == 'b'
                            || ((nextCell.Value) == 'E' && (int)'z' <= ((int)Value + 1)))
                            {
                                if (ShortestPath + 1 < nextCell.ShortestPath)
                                {
                                    nextCell.ShortestPath = ShortestPath + 1;
                                    nextCell.ShortestPathCell = this;
                                }
                            }
                    }
            }
            Visited = true;

            // if (nextCell.X < grid.GetLength(0)
            //     && nextCell.X >= 0
            //     && nextCell.Y < grid.GetLength(1)
            //     && nextCell.Y >= 0
            //     && !currentPath.Contains(nextCell)
            //     && ((int)grid[nextCell.X, nextCell.Y] <= ((int)grid[currentCell.X, currentCell.Y] + 1)
            //         || grid[nextCell.X, nextCell.Y] == 'a'
            //         || grid[nextCell.X, nextCell.Y] == 'b'
            //         || grid[nextCell.X, nextCell.Y] == 'E')
            //         && currentPath.Count() < (solution?.Count() ?? int.MaxValue)
            //         && currentPath.Count() < 500)
            //         {

            //         }
        }
    }


    // public class Program
    // {
    //     public async static Task Main()
    //     {
    //         // See https://aka.ms/new-console-template for more information
    //         Console.WriteLine("Hello, World!");

    //         var file = await File.ReadAllLinesAsync("input.txt");
    //         var xLength = file.First().Length;
    //         var yLength = file.Length;
    //         char[,] grid = new char[xLength, yLength];

    //         Cell startLocation = null;
    //         Cell endLocation = null;
    //         for (int y = 0; y < yLength; y++)
    //         {
    //             for (int x = 0; x < xLength; x++)
    //             {
    //                 grid[x,y] = file[y][x];
    //                 if (file[y][x] == 'S')
    //                 {
    //                     startLocation = new Cell(x, y);
    //                 }
    //                 if (file[y][x] == 'E')
    //                 {
    //                     endLocation = new Cell(x, y);
    //                 }
    //             }
    //         }
    //         Console.WriteLine("StartLocation: " + startLocation);
    //         Console.WriteLine("EndLocation: " + endLocation);

    //         var currentPath = new List<Cell>()
    //         {
    //             startLocation,
    //         };
    //         var solution = PathRecursive(currentPath, grid, null, endLocation);
    //         Console.WriteLine(solution.Count());
    //     }

    //     public static List<Cell> PathRecursive(List<Cell> currentPath, char[,] grid, List<Cell> solution, Cell endLocation)
    //     {
    //         var currentCell = currentPath.Last();
    //         if (currentCell.X == 4 && currentCell.Y == 2)
    //         {
    //             int asdf = 0;
    //         }

    //         List<Cell> nextCells = new List<Cell>()
    //         {
    //             new Cell(currentCell.X + 1, currentCell.Y),
    //             new Cell(currentCell.X - 1, currentCell.Y),
    //             new Cell(currentCell.X, currentCell.Y+1),
    //             new Cell(currentCell.X, currentCell.Y-1),
    //         };
    //         nextCells.OrderBy(_ => CellDistance(_, endLocation));
    //         foreach (var nextCell in nextCells)
    //         {
    //             solution = CallNextPathRecursive(currentPath, grid, currentCell, nextCell, solution, endLocation);
    //             if (solution != null)
    //             {
    //                 break;
    //             }
    //         }
    //         return solution;
    //     }

    //     public static int CellDistance(Cell from, Cell to)
    //     {
    //         return Math.Abs(from.X - to.X) + Math.Abs(to.Y - to.Y);
    //     }

    //     public static List<Cell> CallNextPathRecursive(List<Cell> currentPath, char[,] grid, Cell currentCell, Cell nextCell, List<Cell> solution, Cell endLocation)
    //     {
    //         if (nextCell.X < grid.GetLength(0)
    //             && nextCell.X >= 0
    //             && nextCell.Y < grid.GetLength(1)
    //             && nextCell.Y >= 0
    //             && !currentPath.Contains(nextCell)
    //             && ((int)grid[nextCell.X, nextCell.Y] <= ((int)grid[currentCell.X, currentCell.Y] + 1)
    //                 || grid[nextCell.X, nextCell.Y] == 'a'
    //                 || grid[nextCell.X, nextCell.Y] == 'b'
    //                 || grid[nextCell.X, nextCell.Y] == 'E')
    //                 && currentPath.Count() < (solution?.Count() ?? int.MaxValue)
    //                 && currentPath.Count() < 500)
    //         {
    //             var nextPath = Cell.Clone(currentPath);
    //             nextPath.Add(nextCell);
    //             if (grid[nextPath.Last().X, nextPath.Last().Y] == 'E'
    //                 && (grid[currentPath.Last().X, currentPath.Last().Y] == 'z'
    //                     || grid[currentPath.Last().X, currentPath.Last().Y] == 'y'))
    //             {
    //                 return nextPath.Count() < (solution?.Count() ?? int.MaxValue) ? nextPath : solution;
    //             }
    //             solution = PathRecursive(nextPath, grid, solution, endLocation);
    //         }
    //         return solution;
    //     }
    // }

    // public class Cell
    // {
    //     public int X {get; set;}
    //     public int Y {get; set;}

    //     public Cell(int x, int y)
    //     {
    //         X = x;
    //         Y = y;
    //     }

    //     public override string ToString()
    //     {
    //         return $"({X}, {Y})";
    //     }

    //     public bool Equals(Cell cell)
    //     {
    //         if (cell.X == X
    //             && cell.Y == Y)
    //             {
    //                 return true;
    //             }
    //         return false;
    //     }

    //     public override bool Equals(object? obj)
    //     {
    //         if (obj is Cell)
    //         {
    //             return Equals((Cell)obj);
    //         }
    //         return base.Equals(obj);
    //     }

    //     public static List<Cell> Clone(List<Cell> cells)
    //     {
    //         var newList = new List<Cell>();
    //         foreach (var cell in cells)
    //         {
    //             newList.Add(new Cell(cell.X, cell.Y));
    //         }
    //         return newList;
    //     }
    // }
}


