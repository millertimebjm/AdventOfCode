
public class Program
{
    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("Day02Input1.txt");

        var games = await Parse(input);

        var total = 0;
        foreach (var game in games)
        {
            if (game.GameResults.All(_ => _.Red <= 12)
                && game.GameResults.All(_ => _.Green <= 13)
                && game.GameResults.All(_ => _.Blue <= 14))
                {
                    //Console.WriteLine($"{game.GameId} count:{game.GameResults.Count()}");
                    //Console.WriteLine($"{game.GameId.ToString()} count:{game.GameResults.Count()} Red:{game.GameResults.Max(_ => _.Red)} Green:{game.GameResults.Max(_ => _.Green)} Blue:{game.GameResults.Max(_ => _.Blue)}");
                    total += game.GameId;
                }
            // else 
            // {
            //     Console.WriteLine($"{game.GameId.ToString()} count:{game.GameResults.Count()} Red:{game.GameResults.Max(_ => _.Red)} Green:{game.GameResults.Max(_ => _.Green)} Blue:{game.GameResults.Max(_ => _.Blue)}");
            // }
        }
        Console.WriteLine(total);
    }

    public static async Task<IEnumerable<Game>> Parse(string[] input)
    {
        var games = new List<Game>();
        foreach (var line in input)
        {
            var newLine = line.Replace("Game ", "");
            var game = new Game();
            game.GameId = int.Parse(newLine.Split(':')[0]);
            foreach (var gameResultString in newLine.Split(';').Select(_ => _.Trim()))
            {
                var gameResult = await ParseGameResult(gameResultString);
                game.GameResults.Add(gameResult);
            }
            Console.WriteLine($"{game.GameId} {game.GameResults.First().Red} {game.GameResults.First().Green} {game.GameResults.First().Blue}");
            games.Add(game);
        }
        return games;
    }

    private static async Task<GameResult> ParseGameResult(string gameResultString)
    {
        var gameResult = new GameResult();
        foreach (var colorString in gameResultString.Split(',').Select(_ => _.Trim()))
        {
            var colorSplit = colorString.Split(" ");
            var color = colorSplit[1];
            var count = colorSplit[0];
            if (color == "blue")
                gameResult.Blue = int.Parse(count);
            else if (color == "green")
                gameResult.Green = int.Parse(count);
            else if (color == "red")
                gameResult.Red = int.Parse(count);
        }
        return gameResult;
    }
}

public class Game
{
    public int GameId { get; set; }
    public List<GameResult> GameResults { get; set; }

    public Game()
    {
        GameResults = new List<GameResult>();
    }
}

public class GameResult
{
    public int Green { get; set; } = 0;
    public int Blue { get; set; } = 0;
    public int Red { get; set; } = 0;
}



// using System.Text.RegularExpressions;

// public class Program 
// {
//     public static async Task Main()
//     {
//         var part1Answer = 0;
//         var part2Answer = 0;
//         Dictionary<string, int> MAX_ITEMS = new Dictionary<string, int>();
//         MAX_ITEMS["red"] = 12;
//         MAX_ITEMS["green"] = 13;
//         MAX_ITEMS["blue"] = 14;
//         var puzzleInput = await File.ReadAllLinesAsync("Day02Input1.txt");
//         foreach (string line in puzzleInput)
//         {
//             int gameID = int.Parse(line[line.IndexOf(' ')..line.IndexOf(':')]);
            
//             //Do you lke making assumptions about how data looks? 
//             List<(int cubeCount, string cubeColor)> splits = 
//                 Regex.Matches(line[(line.IndexOf(':')+1)..], @"(\d+.(?:red|green|blue))")
//                     .Select(x => x.Value.Split(' '))
//                     .Select(x => (int.Parse(x[0]), x[1]))
//                     .ToList(); 

//             int maxRed = splits.Where(w => w.cubeColor == "red").Max(x => x.cubeCount);
//             int maxGreen = splits.Where(w => w.cubeColor == "green").Max(x => x.cubeCount);
//             int maxBlue = splits.Where(w => w.cubeColor == "blue").Max(x => x.cubeCount);
//             int theMax = splits.Max(x => x.cubeCount);

//             if (MAX_ITEMS["red"] >= maxRed && MAX_ITEMS["green"] >= maxGreen && MAX_ITEMS["blue"] >= maxBlue)
//             {
//                 Console.WriteLine($"{line}");
//                 part1Answer += gameID;  
//             }
            

//             part2Answer += (maxRed * maxGreen * maxBlue);
            
//         }
//         Console.WriteLine(part1Answer);
//     }
// }