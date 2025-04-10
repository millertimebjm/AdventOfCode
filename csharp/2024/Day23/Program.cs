using System.Security.Cryptography.X509Certificates;

public class Program
{
    public static void Main(string[] args)
    {
        var lines = File.ReadAllLines("input_test.txt");
        var connections = new List<ConnectionModel>();
        foreach (var line in lines) 
        {
            var connection = new ConnectionModel();
            var computerStrings = line.Split("-");
            connection.Computers.Add(computerStrings[0]);
            connection.Computers.Add(computerStrings[1]);
            connections.Add(connection);
        }

        var service = new ConnectionService();
        var newConnections = service.FindAnotherLevelOfConnections(connections);
        var newConnections2 = service.FindMultiplayer(newConnections, connections);
        Console.WriteLine(newConnections.Count());

        Console.WriteLine(newConnections.Where(c => c.Computers.Any(com => com.StartsWith("t"))).Count());

        var strings = newConnections.Select(c => string.Join("-", c.Computers.OrderBy(_ => _))).OrderBy(_ => _);
        foreach (var connectionString in strings)
        {
            Console.WriteLine(connectionString);
        }
    }
}

public class ConnectionModel
{
    public List<string> Computers { get; set; } = new List<string>();
}

public class ConnectionService
{
    public List<ConnectionModel> FindLevels(int maxLevel, int currentLevel, List<string> computers)
    {
        
    }
}