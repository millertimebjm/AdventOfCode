namespace AdventOfCodeCSharp.Day16
{
    public class Program
    {
        public async static Task Main()
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            var file = await File.ReadAllLinesAsync("input-test.txt");
            var caves = new List<Cave>();
            foreach (var line in file)
            {
                // Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
                var parts = line
                    .Replace("Valve ", "")
                    .Replace("has flow rate=", "")
                    .Replace("; tunnels lead to valves", "")
                    .Replace(",", "");
                var partsArray = parts.Split(" ");
                var cave = new Cave()
                {
                    Valve = partsArray[0],
                    Value = int.Parse(partsArray[1]),
                };

                for (int i = 2; i < partsArray.Length; i++)
                {
                    cave.ConnectedCaves.Add(partsArray[i]);
                    cave.DistanceToCaves.Add(new Tuple<string, int>(partsArray[i], 1));
                }
                caves.Add(cave);
            }

            while (caves.Any(_ => _.DistanceToCaves.Count() != caves.Count()))
            {
                foreach (var cave in caves)
                {

                }
            }

            for (int i = 1; i <= 30; i++)
            {

            }
        }
    }

    public class Cave
    {
        public string Valve { get; set; } = "";
        public int Value { get; set; } = 0;
        public List<string> ConnectedCaves { get; set; } = new List<string>();
        public List<Tuple<int, int>> BestValue { get; set; } = new List<Tuple<int, int>>();
        public List<Tuple<string, int>> DistanceToCaves { get; set; } = new List<Tuple<string, int>>();
    }
}


