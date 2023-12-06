
using System.Dynamic;

namespace GardenDomain 
{
    public class Seed 
    {
        public long SeedId {get; set;}
        public long Count {get; set;}
        public Seed (long seedId, long count)
        {
            SeedId = seedId;
            Count = count;
        }
    }

    public class Garden
    {
        //public List<long> Seeds {get; set;}

        // public void ParseSeeds(string input)
        // {
        //     var seedString = input.Replace("seeds: ", "");
        //     Seeds = seedString
        //         .Split(" ")
        //         .Select(_ => long.Parse(_))
        //         .ToList();
        // }

        public List<Seed> Seeds {get; set;} = new List<Seed>();
        public void ParseSeeds(string input)
        {
            var seedString = input.Replace("seeds: ", "");
            var seedArray = seedString
                .Split(" ")
                .Select(_ => long.Parse(_))
                .ToArray();
            for (int i = 0; i < seedArray.Length; i+=2)
            {
                Seeds.Add(new Seed(seedArray[i], seedArray[i+1]));
            }
        }

        public List<GardenMap> ParseMap(string[] lines)
        {
            var gardenMapList = new List<GardenMap>();
            foreach (var line in lines)
            {
                var gardenMap = new GardenMap();
                var lineArray = line
                    .Split(" ")
                    .Select(_ => long.Parse(_))
                    .ToArray();
                gardenMap.Source = lineArray[1];
                gardenMap.Destination = lineArray[0];
                gardenMap.Count = lineArray[2];
                gardenMapList.Add(gardenMap);
            }
            return gardenMapList;
        }
    }

    public class GardenMap
    {
        public long Source {get; set;}
        public long Destination {get; set;}
        public long Count {get; set;}

        public static long ApplyMapping(long input, List<GardenMap> gardenMaps)
        {
            var result = 0;
            
            foreach (var gardenMap in gardenMaps)
            {
                if (input >= gardenMap.Source && input < gardenMap.Source + gardenMap.Count)
                {
                    var difference = gardenMap.Destination - gardenMap.Source;
                    return input + difference;
                }
            }
            
            return input;
        }
    }
}