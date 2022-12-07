using System;
using System.Linq;

namespace AdventOfCodeCSharp.Day07 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var file = await File.ReadAllLinesAsync("input.txt");
            var baseDirectory = new AdventDirectory("/");
            Import(file, baseDirectory);
            var directoryFileSizes = new Dictionary<string, long>();
            var baseDirectorySize = baseDirectory.GetSize();
            DirectoryFileSizesRecursive(baseDirectory.SubDirectories, directoryFileSizes);
            long count = 0;
            foreach (var directory in directoryFileSizes)
            {
                if (directory.Value <= 100000)
                {
                    count += directory.Value;
                }
            }
            Console.WriteLine(count);

            var totalSize = 70000000;
            var availableSize = totalSize - baseDirectorySize;
            var updateSize = 30000000;
            //                4215343
            var neededSize = updateSize - availableSize;
            Console.WriteLine(availableSize);
            Console.WriteLine(neededSize);
            Console.WriteLine(directoryFileSizes.Values.Where(_ => _ > neededSize).Min());
        }

        public static void DirectoryFileSizesRecursive(List<AdventDirectory> directories, Dictionary<string, long> directoryFileSizes)
        {
            foreach (var directory in directories)
            {
                directoryFileSizes.Add(directory.GetFullNameRecursive(), directory.GetSize());
                DirectoryFileSizesRecursive(directory.SubDirectories, directoryFileSizes);
            }
        }



        public static void Import(string[] file, AdventDirectory directory)
        {
            var currentDirectory = directory;
            foreach (var line in file)
            {
                if (line == "$ cd /" || line == "$ ls")
                {
                    continue;
                }
                else if (Char.IsNumber(line.First()))
                {
                    var fileProperties = line.Split(" ");
                    currentDirectory.AdventFiles.Add(new AdventFile(fileProperties[1], long.Parse(fileProperties[0])));
                }
                else if (line.Substring(0, 3) == "dir")
                {
                    var directoryProperties = line.Split(" ");
                    currentDirectory.SubDirectories.Add(new AdventDirectory(directoryProperties[1], currentDirectory));
                }
                else if (line == "$ cd ..")
                {
                    currentDirectory = currentDirectory.ParentDirectory;
                }
                else if (line.StartsWith("$ cd "))
                {
                    var directoryProperties = line.Split(" ");
                    currentDirectory = currentDirectory.SubDirectories.Single(_ => _.Name == directoryProperties[2]);
                }
                else throw new NotImplementedException();
            }
        }
    }
}

public class AdventDirectory
{
    public AdventDirectory(string name, AdventDirectory parentDirectory = null)
    {
        Name = name;
        ParentDirectory = parentDirectory;
    }

    public string Name { get; set; } = string.Empty;
    public List<AdventDirectory> SubDirectories { get; set; } = new List<AdventDirectory>();
    public List<AdventFile> AdventFiles { get; set; } = new List<AdventFile>();
    public AdventDirectory ParentDirectory {get; set; } = null;


    public string GetFullNameRecursive()
    {
        if (ParentDirectory != null)
        {
            return ParentDirectory.GetFullNameRecursive() + "/" + Name;
        }
        return Name;
    }
    public long GetSize() 
    {
        return 
            AdventFiles.Sum(_ => _.Size)
            + SubDirectories.Sum(_ => _.GetSize());
    }
}

public class AdventFile 
{
    public AdventFile(string name, long size) 
    {
        Size = size;
        Name = name;
    }
    public long Size { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
}

