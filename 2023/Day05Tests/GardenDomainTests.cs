namespace Tests;
using GardenDomain;

public class GardenDomainTests
{
    [Fact]
    public void ParseSeedTest()
    {
        var garden = new Garden();
        garden.ParseSeeds("seeds: 12 24 36");
        Assert.Contains(12, garden.Seeds);
        Assert.Contains(24, garden.Seeds);
        Assert.Contains(36, garden.Seeds);
        Assert.Equal(3, garden.Seeds.Count());
    }

    [Fact]
    public void ParseMapTest()
    {
        var mapData = new string[]
        {
            "0 1 2",
            "5 4 3",
            "90 85 5"
        };

        var garden = new Garden();
        var maps = garden.ParseMap(mapData);
        //Console.WriteLine(GardenMap.ApplyMapping(1), maps);
        Assert.Equal(0, GardenMap.ApplyMapping(1, maps));
        Assert.Equal(7, GardenMap.ApplyMapping(6, maps));
        Assert.Equal(200, GardenMap.ApplyMapping(200, maps));
    }
}