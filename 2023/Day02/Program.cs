
public class Program
{
    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("Day02Input1Test.txt");

        var games = Parse(input);
    }

    public static async Task<IEnumerable<Game>> Parse(string[] input)
    {
        foreach (var line in input)
        {
            var newLine = line.Replace(line, "Game ");
            var game = new Game();
            game.GameId = int.Parse(newLine.Split(':')[0]);
            foreach (var gameResultString in newLine.Split(';').Select(_ => _.Trim()))
            {
                var gameResult = await ParseGameResult(gameResultString);
                game.GameResults.Add(gameResult);
            }
        }
        return null;
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
// See https://aka.ms/new-console-template for more information

public class Game
{
    public int GameId { get; set; }
    public List<GameResult> GameResults { get; set; }
}

public class GameResult
{
    public int Green { get; set; }
    public int Blue { get; set; }
    public int Red { get; set; }
}



