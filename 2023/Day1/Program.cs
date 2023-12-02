// See https://aka.ms/new-console-template for more information

// var lines = @"1abc2
// pqr3stu8vwx
// a1b2c3d4e5f
// treb7uchet";

public class Program
{
    public static async Task Main() 
    {
        var total = 0;
        var lines = await File.ReadAllLinesAsync("Day01Input1.txt");
        foreach (var line in lines)
        {
            var first = -1;
            var last = -1;
            //foreach (var character in line)
            for (int i = 0; i < line.Length; i++)
            {
                var character = line[i];
                var substring = line.Substring(i, line.Length - i);
                if (int.TryParse(character.ToString(), out int temp))
                {
                    if (first == -1)
                    {
                        first = temp;
                    }
                    last = temp;
                }
                if (ConvertWordToInt(substring, out int temp2))
                {
                    // Console.WriteLine(temp2);
                    if (first == -1)
                    {
                        first = temp2;
                    }
                    last = temp2;
                }
                // Console.WriteLine(first + " " + last + " " + character + " " + substring);
            }
            if (first != -1 && last != -1)
            {
                total += int.Parse(first.ToString() + last.ToString());
            }
        }
        Console.WriteLine(total);
    }


    private static bool ConvertWordToInt(string input, out int temp)
    {
        temp = -1;

        if (input.StartsWith("zero"))
            temp = 0;
        if (input.StartsWith("one"))
            temp = 1;
        if (input.StartsWith("two"))
            temp = 2;
        if (input.StartsWith("three"))
            temp = 3;
        if (input.StartsWith("four"))
            temp = 4;
        if (input.StartsWith("five"))
            temp = 5;
        if (input.StartsWith("six"))
            temp = 6;
        if (input.StartsWith("seven"))
            temp = 7;
        if (input.StartsWith("eight"))
            temp = 8;
        if (input.StartsWith("nine"))
            temp = 9;

        return temp != -1;
    }
}