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
                    .Replace("; tunnel leads to valve", "")
                    .Replace(",", "");
                var partsArray = parts.Split(" ");
                var cave = new Cave()
                {
                    Valve = partsArray[0],
                    Value = int.Parse(partsArray[1].Replace(";", "")),
                };

                for (int i = 2; i < partsArray.Length; i++)
                {
                    cave.ConnectedCaves.Add(partsArray[i], 1);
                    cave.DistanceToCaves.Add(new Tuple<string, int>(partsArray[i], 1));
                }
                caves.Add(cave);
            }

            foreach (var cave in caves)
            {
                foreach (var connectedCave in cave.ConnectedCaves.ToList())
                {
                    RecursiveConnectedCavesPathing(cave, connectedCave, 1, caves);
                }
                Console.WriteLine(cave);
            }

            int minutes = 30;
            var firstCave = caves.Single(_ => _.Valve == "AA");

            var visitedCaves = new List<string>()
            {
                firstCave.Valve,
            };
            var maxValue = RecursiveConnectedCavesSolving(visitedCaves, minutes, 0, caves);
            Console.WriteLine("Max Valve value: " + maxValue);
        }

        public static int RecursiveConnectedCavesSolving(
            List<string> visitedCaves,
            int minutesRemaining,
            int currentValue,
            List<Cave> caves)
        {
            if (string.Join(" ", visitedCaves) == "AA DD BB JJ HH EE")
            {

            }
            var newValue = 0;
            var currentCaveString = visitedCaves.Last();
            var currentCave = caves.Single(_ => _.Valve == currentCaveString);
            var connectedCaves = currentCave
                .ConnectedCaves
                .Where(_ =>
                    _.Value + 1 < minutesRemaining
                    && caves.Single(c => c.Valve == _.Key).Value > 0
                    && !visitedCaves.Contains(_.Key))
                .OrderByDescending(_ => (minutesRemaining - (_.Value + 1)) * caves.Single(c => c.Valve == _.Key).Value)
                .ToList();
            foreach (var connectedCave in connectedCaves)
            {
                var visitedCavesNew = visitedCaves.ToList();
                visitedCavesNew.Add(connectedCave.Key);
                var minutesRemainingNew = minutesRemaining - (connectedCave.Value + 1);
                var currentValueNew = currentValue + minutesRemainingNew * caves.Single(c => c.Valve == connectedCave.Key).Value;
                var newNewValue = RecursiveConnectedCavesSolving(visitedCavesNew, minutesRemainingNew, currentValueNew, caves);
                if (newValue < newNewValue)
                {
                    newValue = newNewValue;
                    Console.WriteLine($"Count: {newValue} | {string.Join(" ", visitedCavesNew)}");
                }
            }
            return Math.Max(newValue, currentValue);
        }

        public static void RecursiveConnectedCavesPathing(
            Cave cave,
            KeyValuePair<string, int> connectedCave,
            int level,
            IEnumerable<Cave> caves)
        {
            if (cave.Valve == connectedCave.Key) return;
            if (cave.ConnectedCaves.ContainsKey(connectedCave.Key)
                && cave.ConnectedCaves[connectedCave.Key] <= level
                && level != 1) return;

            if (level != 1) cave.ConnectedCaves[connectedCave.Key] = level;
            foreach (var newConnectedCave in caves.Single(_ => _.Valve == connectedCave.Key).ConnectedCaves)
            {
                RecursiveConnectedCavesPathing(cave, newConnectedCave, level + 1, caves);
            }
        }
    }

    public class Cave
    {
        public string Valve { get; set; } = "";
        public int Value { get; set; } = 0;
        public Dictionary<string, int> ConnectedCaves { get; set; } = new Dictionary<string, int>();
        public List<Tuple<int, int>> BestValue { get; set; } = new List<Tuple<int, int>>();
        public List<Tuple<string, int>> DistanceToCaves { get; set; } = new List<Tuple<string, int>>();

        public override string ToString()
        {
            return $"Valve: {this.Valve} | Value: {this.Value}{Environment.NewLine}    {string.Join($"{Environment.NewLine}    ", ConnectedCaves.Select(_ => $"ConnectedCave: {_.Key} | Distance: {_.Value}"))}";
        }
    }
}


