


public class Program 
{
    public static void Main(string[] args)
    {
        IMapService mapService = new FileMapService();
        var line = File.ReadAllLines("input.txt").First();
        var map = ReadMap(mapService, line);
        foreach (var cell in map.Cells)
        {
            Console.Write(cell.FileId == -1 ? "." : cell.FileId);
        }
        Console.WriteLine("asdf1");
        Console.WriteLine("asdf2");
        Console.WriteLine("asdf3");
        Console.WriteLine("asdf4");

        mapService.Defrag(map);
        foreach (var cell in map.Cells)
        {
            Console.Write(cell.FileId == -1 ? "." : cell.FileId);
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
    public int CountFreeSpace(MapModel map, int startingIndex);
    public int CountFileSize(MapModel map, int endingIndex);
    public void MoveFileToFreeSpace(MapModel map, int emptyStartingSpace, int lastFileIdSpace);


}

public class FileMapService : IMapService
{
    public void Defrag(MapModel map)
    {
        int firstOpenSpace = 0;
        int lastTakenSpace = map.Cells.Count - 1;
        long currentFileId = -1;
        while (lastTakenSpace > 0)
        {
            if (lastTakenSpace == firstOpenSpace)
            {
                while ((map.Cells[lastTakenSpace].FileId == currentFileId || map.Cells[lastTakenSpace].FileId == -1) && lastTakenSpace > 0) lastTakenSpace--;
                firstOpenSpace = 0;
                currentFileId = map.Cells[lastTakenSpace].FileId;
                continue;
            }
            if (map.Cells[firstOpenSpace].FileId >= 0) 
            {
                firstOpenSpace++;
                continue;
            }
            if (map.Cells[firstOpenSpace].FileId == -1) 
            {
                var freeSpace = CountFreeSpace(map, firstOpenSpace);
                var fileSize = CountFileSize(map, lastTakenSpace);
                if (freeSpace >= fileSize) 
                {
                    MoveFileToFreeSpace(map, firstOpenSpace, lastTakenSpace);
                    while ((map.Cells[lastTakenSpace].FileId == currentFileId || map.Cells[lastTakenSpace].FileId == -1) && lastTakenSpace > 0) lastTakenSpace--;
                    firstOpenSpace = 0;
                    currentFileId = map.Cells[lastTakenSpace].FileId;
                    continue;
                }
            }
            firstOpenSpace++;
        }
    }

    public void MoveFileToFreeSpace(MapModel map, int emptyStartingSpace, int lastFileIdSpace)
    {
        var currentFileId = map.Cells[lastFileIdSpace].FileId;
        while (map.Cells[lastFileIdSpace].FileId == currentFileId)
        {
            map.Cells[emptyStartingSpace].FileId = currentFileId;
            map.Cells[lastFileIdSpace].FileId = -1;
            emptyStartingSpace++;
            lastFileIdSpace--;
        }
    }

    public int CountFileSize(MapModel map, int endingIndex)
    {
        if (map.Cells[endingIndex].FileId == -1) throw new ArgumentException("Cell at index is not a file.");
        var currentIndex = endingIndex;
        var currentFileId = map.Cells[endingIndex].FileId;
        while (endingIndex > 0 && map.Cells[currentIndex].FileId == currentFileId)
        {
            currentIndex--;
        }
        return endingIndex - currentIndex;
    }

    public int CountFreeSpace(MapModel map, int startingIndex)
    {
        var currentIndex = startingIndex;
        while (startingIndex < map.Cells.Count && map.Cells[currentIndex].FileId == -1)
        {
            currentIndex++;
        }
        return currentIndex - startingIndex;
    }

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


    public long GetNextFileId(MapModel map)
    {
        var currentFileId = map.MaxFileId;
        map.MaxFileId++;
        return currentFileId;
    }
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

    public int CountFreeSpace(MapModel map, int startingIndex)
    {
        throw new NotImplementedException();
    }

    public int CountFileSize(MapModel map, int endingIndex)
    {
        throw new NotImplementedException();
    }

    public void MoveFileToFreeSpace(MapModel map, int emptyStartingSpace, int lastFileIdSpace)
    {
        throw new NotImplementedException();
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

