
public class Program
{
    public static void Main(string[] args) 
    {
        string input = "872027 227 18 9760 0 4 67716 9245696";
        List<Tuple<long, int, long>> memos = new();
        var numbers = input.Split(" ").Select(_ => long.Parse(_));

        long total = 0;
        foreach (var number in numbers)
        {
            total += ProcessNumber(memos, 1, 75, number);
        }
        Console.WriteLine(total);
    }

    public static long ProcessNumber(List<Tuple<long, int, long>> memos, int blink, int totalBlinks, long numberToProcess)
    {
        if (blink == totalBlinks+1) 
        {
            return 1;
        }

        List<long> numbers = new();
        if (numberToProcess == 0) numbers.Add(1);
        else if (numberToProcess.ToString().Length % 2 == 0)
        {
            var numberToProcessString = numberToProcess.ToString();
            numbers.Add(long.Parse(numberToProcessString.Substring(0, numberToProcessString.Length / 2)));
            numbers.Add(long.Parse(numberToProcessString.Substring(numberToProcessString.Length / 2, numberToProcessString.Length / 2)));
        }
        else 
        {
            numbers.Add(numberToProcess * 2024);
        }

        long total = 0;
        foreach (var number in numbers)
        {
            var currentMemo = memos.SingleOrDefault(m => m.Item1 == number && m.Item2 == totalBlinks - blink);
            if (currentMemo != null)
            {
                total += currentMemo.Item3;
            }
            else 
            {
                var processResult = ProcessNumber(memos, blink+1, totalBlinks, number);
                var isExistingMemo = memos.SingleOrDefault(m => m.Item1 == number && m.Item2 == totalBlinks - blink);
                if (isExistingMemo == null)
                {
                    memos.Add(new Tuple<long, int, long>(number, totalBlinks - blink, processResult));
                    Console.WriteLine($"Adding memo for {number}/{totalBlinks-blink}: {processResult}");
                }
                total += processResult;
            }
        }
        return total;
    }
}

