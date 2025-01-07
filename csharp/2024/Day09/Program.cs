
public class Program 
{
    public static void Main(string[] args)
    {
        // See https://aka.ms/new-console-template for more information
        Console.WriteLine("Hello, World!");

        IMapService mapService = new BlockMapService();
        var line = File.ReadAllLines("input_test.txt").First();
        var map = ReadMap(mapService, line);
        foreach (var cell in map.Cells)
        {
            Console.Write(cell.FileId);
        }
        Console.WriteLine();
        mapService.Defrag(map);
        foreach (var cell in map.Cells)
        {
            Console.Write(cell.FileId);
        }
        Console.WriteLine();
        long mapValue = mapService.GetMapValue(map);
        Console.WriteLine(mapValue);
    }

    private static MapModel ReadMap(IMapService mapService, string line)
    {
        var map = new MapModel();
        var isFileBlock = true;
        foreach (var character in line)
        {
            var count = int.Parse(character.ToString());
            if (isFileBlock)
            {
                var fileId = mapService.GetNextFileId(map);
                for (int i = 0; i < count; i++)
                {
                    map.Cells.Add(new CellModel(fileId));
                }
            }
            else 
            {
                for (int i = 0; i < count; i++)
                {
                    map.Cells.Add(new CellModel(-1));
                }
            }
            
            isFileBlock = !isFileBlock;
        }
        return map;
    }
}

public interface IMapService
{
    void Defrag(MapModel map);
    long GetMapValue(MapModel map);
    long GetNextFileId(MapModel map);
}

public class FileMapService : IMapService
{
    
}

public class BlockMapService : IMapService
{
    public long GetMapValue(MapModel map)
    {
        long mapValue = 0;
        for (int i = 0; i < map.Cells.Count; i++)
        {
            if (map.Cells[i].FileId != -1)
            {
                mapValue += (long)map.Cells[i].FileId * (long)i;
            }
        }
        return mapValue;
    }

    public void Defrag(MapModel map)
    {
        int firstOpenSpace = 0;
        int lastTakenSpace = map.Cells.Count - 1;
        while(firstOpenSpace < lastTakenSpace)
        {
            if (map.Cells[firstOpenSpace].FileId >= 0) 
            {
                firstOpenSpace++;
                continue;
            }
            if (map.Cells[lastTakenSpace].FileId < 0)
            {
                lastTakenSpace--;
                continue;
            }
            map.Cells[firstOpenSpace].FileId = map.Cells[lastTakenSpace].FileId;
            map.Cells[lastTakenSpace].FileId = -1;
        }
    }

    public long GetNextFileId(MapModel map)
    {
        var currentFileId = map.MaxFileId;
        map.MaxFileId++;
        return currentFileId;
    }
}

public class MapModel
{
    public int MaxFileId { get; set; } = 0;
    public List<CellModel> Cells { get; set; } = new List<CellModel>();
}

public class CellModel
{
    public long FileId { get; set; } = 0;
    
    public CellModel(long fileId)
    {
        FileId = fileId;
    }
}