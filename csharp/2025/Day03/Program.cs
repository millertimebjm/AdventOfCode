// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = await File.ReadAllLinesAsync("input.txt");
var joltages = new List<string>();
var indexes = new List<int>();

foreach (var line in input)
{
    char[] highestString = line.Substring(line.Length - 12, 12).ToArray();
    indexes.Add(line.Length - 1);
    indexes.Add(line.Length - 2);
    indexes.Add(line.Length - 3);
    indexes.Add(line.Length - 4);
    indexes.Add(line.Length - 5);
    indexes.Add(line.Length - 6);
    indexes.Add(line.Length - 7);
    indexes.Add(line.Length - 8);
    indexes.Add(line.Length - 9);
    indexes.Add(line.Length - 10);
    indexes.Add(line.Length - 11);
    indexes.Add(line.Length - 12);
}

// foreach (var line in input)
// {
//     char[] highestString = line.Substring(line.Length - 2, 2).ToArray();
//     int firstIndex = line.Length -2;
//     int secondIndex = line.Length -1;
//     for (int i = line.Length - 3; i >= 0; i--)
//     {
//         if (line[i] >= highestString[0])
//         {
//             highestString[0] = line[i];
//             firstIndex = i;
//             for (int j = secondIndex; j > firstIndex; j--)
//             {
//                 if (line[j] > highestString[1])
//                 {
//                     highestString[1] = line[j];
//                     secondIndex = j;
//                 }
//             }
//         }
//     }

//     joltages.Add($"{highestString[0]}{highestString[1]}");
// }

Console.WriteLine(joltages.Sum(j => int.Parse(j)));