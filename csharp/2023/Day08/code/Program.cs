using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

public class Program 
{
    public static async Task Main()
    {
        var input = await File.ReadAllLinesAsync("Input1.txt");
        var nodes = new List<Node>();
        var directions = input[0];
        foreach (var line in input.Skip(2))
        {
            var lineSplit = line.Split("=");
            nodes.Add(new Node(lineSplit[0].Trim()));
        }        

        foreach (var line in input.Skip(2))
        {
            var lineSplit = line.Split("=");
            var node = nodes.Single(_ => _.Name == lineSplit[0].Trim());
            var pathSplit = lineSplit[1]
                .Replace("(", "")
                .Replace(")", "")
                .Split(",");
            var leftNode = nodes.Single(_ => _.Name == pathSplit[0].Trim());
            var rightNode = nodes.Single(_ => _.Name == pathSplit[1].Trim());
            node.Path = new Tuple<Node, Node>(leftNode, rightNode);
        }

        var currentNodes = nodes.Where(_ => _.Name.EndsWith('A')).ToList();
        var nodePaths = new List<NodePath>();
        for (int j = 0; j < currentNodes.Count; j++)
        {
            nodePaths.Add(new NodePath() { Nodes = new List<Node> { new Node(currentNodes[j].Name) } });
        }
        int i = 0;
        var currentFoundRepititions = 0;
        while (!nodePaths.All(_ => _.RepititionCount.Count > 1))
        {
            Node.NextNode(nodes, currentNodes, directions[i%directions.Length]);
            for (int j = 0; j < currentNodes.Count; j++)
            {
                nodePaths[j].Nodes.Add(new Node(currentNodes[j].Name));
                if (!(nodePaths[j].RepititionCount.Count > 1)
                    && currentNodes[j].Name.EndsWith("Z"))
                {
                    nodePaths[j].RepititionCount.Add(i);
                }
            }
            if (nodePaths.Count(_ => _.RepititionCount.Count > 1) > currentFoundRepititions)
            {
                currentFoundRepititions = nodePaths.Count(_ => _.RepititionCount.Count > 1);
                Console.WriteLine($"{currentFoundRepititions}/{nodePaths.Count}");
            }
            i++;
        }

        foreach (var nodePath in nodePaths)
        {
            Console.WriteLine(string.Join(" ", nodePath.RepititionCount.ToList()));    
        }

        var currentCount = new List<long>();
        foreach (var nodePath in nodePaths)
        {
            currentCount.Add(nodePath.RepititionCount.Last());
        }
        var addAmount = new List<long>();
        foreach (var nodePath in nodePaths)
        {
            addAmount.Add(nodePath.RepititionCount[1] - nodePath.RepititionCount[0]);
        }
        while (currentCount.GroupBy(_ => _).First().Count() != 6)
        {
            var minimumCount = currentCount[0];
            var indexMinimumCount = 0;
            for(int j = 1; j < currentCount.Count; j++)
            {

                if (currentCount[j] < minimumCount)
                indexMinimumCount = j;
            }
            currentCount[indexMinimumCount] += addAmount[indexMinimumCount];
            Console.WriteLine(string.Join(" ", currentCount.Select(_ => _)));
        }

        Console.WriteLine(currentCount[0]);

        
        // var nodePathCounts = nodePaths.Select(_ => _.RepititionCount.w).ToList();
        // long total = nodePathCounts.Max();
        // while (!nodePathCounts.All(_ => total % _ == 0))
        // {
        //     total += nodePathCounts.Max();
        //     Console.WriteLine(total);
        // }

        // Console.WriteLine(total);
    }


    public static async Task Task1()
    {
        var input = await File.ReadAllLinesAsync("Input1.txt");
        var nodes = new List<Node>();
        var directions = input[0];
        foreach (var line in input.Skip(2))
        {
            var lineSplit = line.Split("=");
            nodes.Add(new Node(lineSplit[0].Trim()));
        }        

        foreach (var line in input.Skip(2))
        {
            var lineSplit = line.Split("=");
            var node = nodes.Single(_ => _.Name == lineSplit[0].Trim());
            var pathSplit = lineSplit[1]
                .Replace("(", "")
                .Replace(")", "")
                .Split(",");
            var leftNode = nodes.Single(_ => _.Name == pathSplit[0].Trim());
            var rightNode = nodes.Single(_ => _.Name == pathSplit[1].Trim());
            node.Path = new Tuple<Node, Node>(leftNode, rightNode);
        }

        Console.WriteLine(directions.Length);
        Console.WriteLine(Node.DelveNodes(nodes, directions));
    }
}

public class Node
{
    public string Name {get; set;}
    public Tuple<Node, Node> Path {get; set;}
    public Node(string name)
    {
        Name = name;
    }

    public static long DelveNodes(List<Node> nodes, string directions)
    {
        var currentNode = nodes.Single(_ => _.Name == "AAA");
        int i = 0;
        while (currentNode.Name != "ZZZ")
        {
            if (directions[i%directions.Length] == 'L')
            {
                Console.WriteLine($"{currentNode.Name} : L : {currentNode.Path.Item1.Name}");
                currentNode = nodes.Single(_ => _.Name == currentNode.Path.Item1.Name);
            }
            else if (directions[i%directions.Length] == 'R')
            {
                Console.WriteLine($"{currentNode.Name} : R : {currentNode.Path.Item2.Name}");
                currentNode = nodes.Single(_ => _.Name == currentNode.Path.Item2.Name);
            }
            else
                throw new NotImplementedException();
            
            i++;
        }
        return i;
    }

    public static void NextNode(List<Node> nodes, List<Node> currentNodes, char direction)
    {
        for (int i = 0; i < currentNodes.Count; i++)
        {
            if (direction == 'L')
            {
                //Console.WriteLine($"{currentNodes[i].Name} : L : {currentNodes[i].Path.Item1.Name}");
                currentNodes[i] = nodes.Single(_ => _.Name == currentNodes[i].Path.Item1.Name);
            }
            else if (direction == 'R')
            {
                //Console.WriteLine($"{currentNodes[i].Name} : R : {currentNodes[i].Path.Item2.Name}");
                currentNodes[i] = nodes.Single(_ => _.Name == currentNodes[i].Path.Item2.Name);
            }
            else
                throw new NotImplementedException();
        }
        
    }   
}

public class NodePath
{
    public List<Node> Nodes {get;set;}
    public List<long> RepititionCount {get; set;} = new List<long>();

    public long FindRepetition()
    {
        if (RepititionCount.Count > 1)
        {
            return RepititionCount.Last();
        }
        var firstNode = Nodes.First();
        var verifiableRepititionCount = 3;
        var foundRepititions = 0;
        for (int i = 1; i < Nodes.Count; i++)
        {
            if (Nodes[i].Name == firstNode.Name)
            {
                while(foundRepititions < verifiableRepititionCount)
                {
                    int j = 0;
                    for (; j < i; j++)
                    {
                        if (Nodes[j+i].Name != Nodes[j].Name)
                        {
                            break;
                        }
                    }
                    if (j == i)
                    {
                        foundRepititions++;
                    }
                    else 
                    {
                        break;
                    }
                }
                if (foundRepititions == verifiableRepititionCount)
                {
                    RepititionCount.Add((i+1) / 3);
                }
            }
        }

        return -1;
    }
}