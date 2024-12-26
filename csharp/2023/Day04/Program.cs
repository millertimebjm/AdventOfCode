using System.Linq;

var lines = await File.ReadAllLinesAsync("Day04Input1.txt");
var linesArray = new int[lines.Length];
// linesArray[0] = 1;
for (int i = 0; i < lines.Length; i++)
{
    linesArray[i] = 1;
}
var total = 0;
for(int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < linesArray[i]; j++)
    {
        //var cardNumber = int.Parse(line.Replace("Card ", "").Split(":")[0]);
        var winningNumbers = lines[i].Split(":")[1].Split("|")[0];
        var playerNumbers = lines[i].Split(":")[1].Split("|")[1];
        var winningNumbersList = winningNumbers.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(_ => int.Parse(_.Trim()));
        var playerNumbersList = playerNumbers.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(_ => int.Parse(_.Trim()));
        Console.Write(string.Join(" ", winningNumbersList.OrderBy(_ => _)));
        Console.Write("  |  ");
        Console.Write(string.Join(" ", playerNumbersList.OrderBy(_ => _)));
        Console.WriteLine();
        var count = playerNumbersList.Count(_ => winningNumbersList.Contains(_));
        for (int k = i + 1; k < (count + i + 1); k++)
        {
            linesArray[k]++;
        }
        Console.WriteLine($"i:{i} j:{j} currentLineArrayIndex:{linesArray[i]} foundCount:{count}");
    }
    
    // Console.WriteLine($"{count} {Math.Pow(2, count == 0 ? 0 : count-1)}");
    // if (count > 0)
    //     total += (int)Math.Pow(2, count == 0 ? 0 : count-1);
    
}
Console.WriteLine(linesArray.Sum());
