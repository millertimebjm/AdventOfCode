using GardenDomain;

public class Program 
{
    public static async Task Main()
    {
        var garden = new Garden();
        var input = await File.ReadAllLinesAsync("Input1.txt");

        var seedToSoilMap = new List<GardenMap>();
        var soilToFertilizerMap = new List<GardenMap>();
        var fertilizerToWaterMap = new List<GardenMap>();
        var waterToLightMap = new List<GardenMap>();
        var lightToTemperatureMap = new List<GardenMap>();
        var temperatureToHumidityMap = new List<GardenMap>();
        var humidityToLocationMap = new List<GardenMap>();

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i].StartsWith("seeds: "))
            {
                garden.ParseSeeds(input[i]);
            }

            if (input[i] == "seed-to-soil map:")
            {
                i++;
                seedToSoilMap = ParseMapPrivate(garden, i, input);
            }

            if (input[i] == "soil-to-fertilizer map:")
            {
                i++;
                soilToFertilizerMap = ParseMapPrivate(garden, i, input);
            }

            if (input[i] == "fertilizer-to-water map:")
            {
                i++;
                fertilizerToWaterMap = ParseMapPrivate(garden, i, input);
            }

            if (input[i] == "water-to-light map:")
            {
                i++;
                waterToLightMap = ParseMapPrivate(garden, i, input);
            }

            if (input[i] == "light-to-temperature map:")
            {
                i++;
                lightToTemperatureMap = ParseMapPrivate(garden, i, input);
            }

            if (input[i] == "temperature-to-humidity map:")
            {
                i++;
                temperatureToHumidityMap = ParseMapPrivate(garden, i, input);
            }

            if (input[i] == "humidity-to-location map:")
            {
                i++;
                humidityToLocationMap = ParseMapPrivate(garden, i, input);
            }
        }
        
        var minimum = long.MaxValue;
        foreach (var seed in garden.Seeds)
        {
            Console.WriteLine($"SeedId:{seed.SeedId}");
            for (int i = 0; i < seed.Count; i++)
            {
                var currentMapper = GardenMap.ApplyMapping(seed.SeedId + i, seedToSoilMap);
                currentMapper = GardenMap.ApplyMapping(currentMapper, soilToFertilizerMap);
                currentMapper = GardenMap.ApplyMapping(currentMapper, fertilizerToWaterMap);
                currentMapper = GardenMap.ApplyMapping(currentMapper, waterToLightMap);
                currentMapper = GardenMap.ApplyMapping(currentMapper, lightToTemperatureMap);
                currentMapper = GardenMap.ApplyMapping(currentMapper, temperatureToHumidityMap);
                currentMapper = GardenMap.ApplyMapping(currentMapper, humidityToLocationMap);
                
                minimum = long.Min(minimum, currentMapper);
                if (currentMapper < minimum)
                {
                    minimum = currentMapper;
                    Console.WriteLine(minimum);
                }
            }
        }
        Console.WriteLine(minimum);
    }

    private static List<GardenMap> ParseMapPrivate(Garden garden, int i, string[] input)
    {
        var mapList = new List<string>();
        while (i < input.Length && input[i] != "")
        {
            mapList.Add(input[i]);
            i++;
        }
        return garden.ParseMap(mapList.ToArray());
    }
}