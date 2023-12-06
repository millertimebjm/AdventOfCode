using Microsoft.VisualBasic;

namespace tests;

public class RaceTests
{
    [Theory]
    [InlineData(4, 1, 3)]
    [InlineData(4, 2, 4)]
    [InlineData(4, 3, 3)]
    [InlineData(10, 1, 9)]
    [InlineData(10, 5, 25)]
    [InlineData(100, 50, 2500)]
    public void CalculateSingleBeatDistanceTest(
        long timing, long waitTime, long distance)
    {
        var singleDistance = Timing.CalculateSingleDistance(timing, waitTime);
        Assert.Equal(singleDistance, distance);
    }

    [Fact]
    public void CalculateBeatDistance()
    {
        var timing = new Timing(7, 9);
        var beatTimings = timing.CalculateBeatDistance();
        Assert.Contains(2, beatTimings);
        Assert.Contains(3, beatTimings);
        Assert.Contains(4, beatTimings);
        Assert.Contains(5, beatTimings);
        Assert.Equal(4, beatTimings.Count);
    }
}