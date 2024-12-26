using AdventOfCodeCSharp.Day02;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//var file = await File.ReadAllLinesAsync("input-test.txt");
var file = await File.ReadAllLinesAsync("input.txt");
var score = 0;
foreach (var line in file)
{
    if (!string.IsNullOrWhiteSpace(line))
    {
        var playersResponses = line.Split(" ");
        score += RockPaperScissorsService.GetScore(playersResponses[0],playersResponses[1]);
    }
}
Console.WriteLine(score);

score = 0;
foreach (var line in file)
{
    if (!string.IsNullOrWhiteSpace(line))
    {
        var playersResponses = line.Split(" ");
        var player2Choice = RockPaperScissorsService.ChoosePlayer2Play(playersResponses[0],playersResponses[1]);
        score += RockPaperScissorsService.GetScore(playersResponses[0],player2Choice);
    }
}
Console.WriteLine(score);
