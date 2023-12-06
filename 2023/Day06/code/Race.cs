public class Race
{
    public List<Timing> Timings {get; set;}
}

public class Timing
{
    public long Time {get; set;}
    public long Distance {get; set;}

    public List<long> CalculateBeatDistance()
    {
        var beatDistances = new List<long>();
        for (int i = 0; i < Time; i++)
        {
            var thisTimeDistance = CalculateSingleDistance(
                Time, waitTime: i);
            if (thisTimeDistance > Distance)
            beatDistances.Add(thisTimeDistance);
        }
        return beatDistances;
    }

    public static long CalculateSingleDistance(
        long time, long waitTime)
    {
        return (time - waitTime) * waitTime;
    }
}