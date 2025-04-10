using System.Dynamic;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Collections.Generic;

public class Program
{
    public static async Task Main(string[] args)
    {
        var lines = await File.ReadAllLinesAsync("input_test.txt");
        var startingCell = FindStartingCell(lines);
        var endingCell = FindEndingCell(lines);

        List<Cell> cellsToReview = new();
        List<Cell> minimumCostCells = new();

        startingCell.Direction = '>';
        startingCell.MinimumScore = 0;
        cellsToReview.Add(startingCell);
        var cellService = new CellService();

        while (!minimumCostCells.Any(c => c.X == endingCell.X && c.Y == endingCell.Y))
        {
            var nextCell = cellService.GetMinimumCellsToReview(cellsToReview);
            minimumCostCells.Add(nextCell);
            var newCellsToReview = cellService.GetCellsToReview(nextCell, minimumCostCells, lines);
            cellsToReview.AddRange(newCellsToReview);
            cellsToReview = cellsToReview.Where(c => !(c.X == nextCell.X && c.Y == nextCell.Y)).ToList();
        }

        var endingCellComplete = minimumCostCells.Single(c => c.X == endingCell.X && c.Y == endingCell.Y);
        Console.WriteLine(endingCellComplete.MinimumScore);

        var maxScore = endingCellComplete.MinimumScore;
        List<Cell> currentPath = new() {startingCell};

        var cells = cellService.GetAllBestPathsRecursive(startingCell, endingCell, currentPath, maxScore, lines);
        cells = cells.Distinct().ToList();
        Console.WriteLine(cells.Count());
    }

    private static Cell FindStartingCell(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++) 
        {
            for (int j = 0; j < lines[i].Length; j++) 
            {
                if (lines[i][j] == 'S') return new Cell(j, i);
            }
        }
        throw new KeyNotFoundException("Can't find S");
    }

    private static Cell FindEndingCell(string[] lines)
    {
        for (int i = 0; i < lines.Length; i++) 
        {
            for (int j = 0; j < lines[i].Length; j++) 
            {
                if (lines[i][j] == 'E') return new Cell(j, i);
            }
        }
        throw new KeyNotFoundException("Can't find E");
    }
}

public class Cell
{
    public int X {get; set; } = 0;
    public int Y {get; set;} = 0;
    public char Direction {get; set;} = '>';
    public int MinimumScore {get; set;} = 0;
    public Cell(int x, int y) 
    {
        X = x;
        Y = y;
    }
    public Cell(int x, int y, int minimumScore,char direction) 
    {
        X = x;
        Y = y;
        Direction = direction;
        MinimumScore = minimumScore;
    }

    public override bool Equals(Object o) 
    {
        if (o != null && o is Cell) return Equals(o as Cell);
        return false;
    }

    public bool Equals(Cell c)
    {
        if (X == c.X && Y == c.Y) return true;
        return false;
    }

    public override string ToString()
    {
        return $"({X},{Y}, {MinimumScore}, {Direction})";
    }

    public override int GetHashCode() 
    {
        return X.GetHashCode() + Y.GetHashCode();
    }
}

public class CellService 
{
    public Cell GetMinimumCellsToReview(List<Cell> cellsToReview)
    {
        return cellsToReview
            .Where(c => c.MinimumScore == cellsToReview
            .Min(_ => _.MinimumScore))
            .First();
    }

    public List<Cell> GetCellsToReview(Cell nextCell, List<Cell> minimumCostCells, string[] lines)
    {
        var cells = new List<Cell>();
        if (lines[nextCell.Y][nextCell.X-1] == '.' || lines[nextCell.Y][nextCell.X-1] == 'E')
        {
            int cost = GetDirectionsCost(nextCell.Direction, '<');
            var newCell = new Cell(nextCell.X-1, nextCell.Y, nextCell.MinimumScore + cost + 1, '<');
            if (!minimumCostCells.Any(c => c.X == newCell.X && c.Y == newCell.Y))
            {
                cells.Add(newCell);
            }
        }
        if (lines[nextCell.Y][nextCell.X+1] == '.' || lines[nextCell.Y][nextCell.X+1] == 'E')
        {
            int cost = GetDirectionsCost(nextCell.Direction, '>');
            var newCell = new Cell(nextCell.X+1, nextCell.Y, nextCell.MinimumScore + cost + 1, '>');
            if (!minimumCostCells.Any(c => c.X == newCell.X && c.Y == newCell.Y))
            {
                cells.Add(newCell);
            }
        }
        if (lines[nextCell.Y-1][nextCell.X] == '.' || lines[nextCell.Y-1][nextCell.X] == 'E')
        {
            int cost = GetDirectionsCost(nextCell.Direction, '^');
            var newCell = new Cell(nextCell.X, nextCell.Y-1, nextCell.MinimumScore + cost + 1, '^');
            if (!minimumCostCells.Any(c => c.X == newCell.X && c.Y == newCell.Y))
            {
                cells.Add(newCell);
            }
        }
        if (lines[nextCell.Y+1][nextCell.X] == '.' || lines[nextCell.Y+1][nextCell.X] == 'E')
        {
            int cost = GetDirectionsCost(nextCell.Direction, 'v');
            var newCell = new Cell(nextCell.X, nextCell.Y+1, nextCell.MinimumScore + cost + 1, 'v');
            if (!minimumCostCells.Any(c => c.X == newCell.X && c.Y == newCell.Y && c.MinimumScore == newCell.MinimumScore))
            {
                cells.Add(newCell);
            }
        }
        return cells;
    }

    public int GetDirectionsCost(char direction, char newDirection)
    {
        int value = Math.Abs(GetValue(direction) - GetValue(newDirection));
        if (value == 0) return 0;
        if (value == 1) return 1000;
        if (value == 2) return 2000;
        if (value == 3) return 1000;
        throw new Exception("Bad GetDirectionsCost value");
    }

    public int GetValue(char direction)
    {
        if (direction == '>') return 0;
        if (direction == '^') return 1;
        if (direction == '<') return 2;
        if (direction == 'v') return 3;
        throw new Exception("Bad GetValue result");
    }

    internal List<Cell> GetAllBestPathsRecursive(Cell currentCell, Cell endingCell, List<Cell> currentPath, int maxScore, string[] lines)
    {
        if (currentCell.X == endingCell.X && currentCell.Y == endingCell.Y) return currentPath;

        var cells = new List<Cell>();
        if (lines[currentCell.Y][currentCell.X-1] == '.' || lines[currentCell.Y][currentCell.X-1] == 'E')
        {
            int cost = GetDirectionsCost(currentCell.Direction, '<');
            var newCell = new Cell(currentCell.X-1, currentCell.Y, currentCell.MinimumScore + cost + 1, '<');
            if (newCell.MinimumScore <= maxScore)
            {
                currentPath.Add(newCell);
                cells.AddRange(GetAllBestPathsRecursive(newCell, endingCell, currentPath, maxScore, lines));
            }
        }
        if (lines[currentCell.Y][currentCell.X+1] == '.' || lines[currentCell.Y][currentCell.X+1] == 'E')
        {
            int cost = GetDirectionsCost(currentCell.Direction, '>');
            var newCell = new Cell(currentCell.X+1, currentCell.Y, currentCell.MinimumScore + cost + 1, '>');
            if (newCell.MinimumScore <= maxScore)
            {
                currentPath.Add(newCell);
                cells.AddRange(GetAllBestPathsRecursive(newCell, endingCell, currentPath, maxScore, lines));
            }
        }
        if (lines[currentCell.Y-1][currentCell.X] == '.' || lines[currentCell.Y-1][currentCell.X] == 'E')
        {
            int cost = GetDirectionsCost(currentCell.Direction, '^');
            var newCell = new Cell(currentCell.X, currentCell.Y-1, currentCell.MinimumScore + cost + 1, '^');
            if (newCell.MinimumScore <= maxScore)
            {
                currentPath.Add(newCell);
                cells.AddRange(GetAllBestPathsRecursive(newCell, endingCell, currentPath, maxScore, lines));
            }
        }
        if (lines[currentCell.Y+1][currentCell.X] == '.' || lines[currentCell.Y+1][currentCell.X] == 'E')
        {
            int cost = GetDirectionsCost(currentCell.Direction, 'v');
            var newCell = new Cell(currentCell.X, currentCell.Y+1, currentCell.MinimumScore + cost + 1, 'v');
            if (newCell.MinimumScore <= maxScore)
            {
                currentPath.Add(newCell);
                cells.AddRange(GetAllBestPathsRecursive(newCell, endingCell, currentPath, maxScore, lines));
            }
        }
        return cells;
    }
}