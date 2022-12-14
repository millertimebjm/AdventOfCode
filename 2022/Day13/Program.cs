namespace AdventOfCodeCSharp.Day13
{
    public class Program
    {
        public static async Task Main()
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            var file = await File.ReadAllLinesAsync("input-test.txt");
            var IntegerOrArraySets = new List<Tuple<IntegerOrArray, IntegerOrArray>>();
            for (int i = 0; i < file.Length; i += 3)
            {
                IntegerOrArraySets.Add(new Tuple<IntegerOrArray, IntegerOrArray>(CreateIntegerOrArray(file[i]), CreateIntegerOrArray(file[i+1])));
            }

            
    // If both values are integers, the lower integer should come first. If the left integer is lower than the right integer, the inputs are in the right order. If the left integer is higher than the right integer, the inputs are not in the right order. Otherwise, the inputs are the same integer; continue checking the next part of the input.
    // If both values are lists, compare the first value of each list, then the second value, and so on. If the left list runs out of items first, the inputs are in the right order. If the right list runs out of items first, the inputs are not in the right order. If the lists are the same length and no comparison makes a decision about the order, continue checking the next part of the input.
    // If exactly one value is an integer, convert the integer to a list which contains that integer as its only value, then retry the comparison. For example, if comparing [0,0,0] and 2, convert the right value to [2] (a list containing 2); the result is then found by instead comparing [0,0,0] and [2].

            //foreach (var integerOrArraySet in IntegerOrArraySets)
            var count = 0;
            for (int i = 0; i < IntegerOrArraySets.Count(); i++)
            {
                if (i==3)
                {
                    int asdf = 0;
                }
                var result = NestedProcessing(IntegerOrArraySets[i].Item1.Children, 
                    IntegerOrArraySets[i].Item2.Children);
                Console.WriteLine(i + ": " + result.ToString());
                if (result.Value) count += i + 1;
            }
            Console.WriteLine(count);
        }

        public static bool? NestedProcessing(List<IntegerOrArray> left, List<IntegerOrArray> right)
        {
            if (left is null && right is not null) return true;
            if (left is not null && right is null) return false;

            bool? result = null;
            for(int i = 0; i < left.Count() || i < right.Count(); i++)
            {
                if (left.Count() > i && right.Count() <= i) return false;
                else if (left.Count() <= i && right.Count() > i) return true;
                else if (left[i].Integer is not null
                    && right[i].Integer is not null)
                    {
                        if (left[i].Integer < right[i].Integer) return true;
                        if (left[i].Integer > right[i].Integer) return false;
                    }
                else if (left[i].Integer is not null
                    && right[i].Children is not null)
                    {
                        left[i].Children = new List<IntegerOrArray>() { new IntegerOrArray(left[i].Integer.Value), };
                        left[i].Integer = null;
                        i--;
                    }
                else if (left[i].Children is not null
                    && right[i].Integer is not null)
                    {
                        right[i].Children = new List<IntegerOrArray>() { new IntegerOrArray(right[i].Integer.Value), };
                        right[i].Integer = null;
                        i--;
                    }
                else if (left[i].Children is not null
                    && right[i].Children is not null)
                    {
                        bool? nestedResult = NestedProcessing(left[i].Children, right[i].Children);
                        if (nestedResult is not null) return nestedResult.Value;
                    }
                else if (left[i].Children is null
                    && right[i].Children is not null) return true;
                else if (left[i].Children is not null
                    && right[i].Children is null) return false;
            }
            return null;
        }

        public static IntegerOrArray CreateIntegerOrArray(string line)
        {
            var integerOrArray = new IntegerOrArray();
            var currentIntegerOrArray = integerOrArray;
            for(int i = 1; i < line.Length-1; i++)
            {
                var nextCommandIndex = line.IndexOfAny(new [] {'[',']',','}, i);
                var nextCommandCharacters = line.Substring(i, nextCommandIndex - i);
                if (int.TryParse(line.Substring(i, nextCommandIndex - i), out int integer))
                {
                    if (currentIntegerOrArray.Children is null)
                    {
                        currentIntegerOrArray.Children = new List<IntegerOrArray>();
                    }
                    currentIntegerOrArray.Children.Add(new IntegerOrArray(integer));
                }
                else if (line.Substring(i, nextCommandIndex - i + 1) == "[")
                {
                    if (currentIntegerOrArray.Children is null)
                    {
                        currentIntegerOrArray.Children = new List<IntegerOrArray>();
                    }
                    var nextIntegerOrArray = new IntegerOrArray()
                    {
                        Parent = currentIntegerOrArray,
                    };
                    currentIntegerOrArray.Children.Add(nextIntegerOrArray);
                    currentIntegerOrArray = nextIntegerOrArray;
                }
                else if (line.Substring(i, nextCommandIndex - i + 1) == "]")
                {
                    currentIntegerOrArray = currentIntegerOrArray.Parent;
                }
                else if (line.Substring(i, nextCommandIndex - i + 1) == ",")
                {

                }
            }
            return integerOrArray;
        }
    }

    public class IntegerOrArray
    {
        public int? Integer {get; set;}
        public List<IntegerOrArray>? Children {get; set;}
        public IntegerOrArray? Parent {get; set;}

        public IntegerOrArray() {}

        public IntegerOrArray(int integer)
        {
            Integer = integer;
        }
    }
}
