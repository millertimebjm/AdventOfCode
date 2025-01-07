
public class Program 
{
    public static async Task Main() 
    {
        var lines = await File.ReadAllLinesAsync("input.txt");
        //var calibrations = BuildCalibrations(lines);
        var calibrations = BuildCalibrationsWithConcat(lines);

        foreach (var calibration in calibrations) 
        {
            Console.WriteLine(calibration);
        }

        var total = calibrations.Where(c => c.IsValid).Sum(c => (decimal)c.Total);
        Console.WriteLine($"Total: {total}");
    } 

    public static List<Calibration> BuildCalibrations(string[] lines) 
    {
        List<Calibration> calibrations = [];
        foreach (var line in lines) 
        {
            var calibrationParts = line.Split(": ");
            calibrations.Add(new Calibration() 
            {
                Total = ulong.Parse(calibrationParts[0]),
                Values = calibrationParts[1].Split(" ").Select(v => ulong.Parse(v)).ToList(),
            });
        }
        return calibrations;
    }

    public static List<CalibrationWithConcat> BuildCalibrationsWithConcat(string[] lines) 
    {
        List<CalibrationWithConcat> calibrations = [];
        foreach (var line in lines) 
        {
            var calibrationParts = line.Split(": ");
            calibrations.Add(new CalibrationWithConcat() 
            {
                Total = ulong.Parse(calibrationParts[0]),
                Values = calibrationParts[1].Split(" ").Select(v => ulong.Parse(v)).ToList(),
            });
        }
        return calibrations;
    }
}


public class Calibration
{
    public ulong Total { get; set; } = 0;
    public List<ulong> Values { get; set; } = [];
    public bool IsValid
    {
        get 
        {
            _isValid ??= IsValidInternal();
            return _isValid.Value;
        }
    }
    private bool? _isValid = null;
    public override string ToString()
    {
        return $"{(IsValid ? "Valid" : "Invalid")} {Total}: {string.Join(" ", Values)}";
    }

    private bool IsValidInternal()
    {
        List<ulong?> currentCounts = [Values.First()];
        
        for (int i = 1; i < Values.Count; i++)
        {
            List<ulong?> newCounts = [];
            foreach (var currentCount in currentCounts)
            {
                var add = currentCount + Values[i];
                if (add <= Total) newCounts.Add(add);
                var mul = currentCount * Values[i];
                if (mul <= Total) newCounts.Add(mul);
            }
            currentCounts = newCounts;
        }
        if (currentCounts.FirstOrDefault(cc => cc == Total) != null) return true;
        return false;
    }
}

public class CalibrationWithConcat
{
    public ulong Total { get; set; } = 0;
    public List<ulong> Values { get; set; } = [];
    public bool IsValid
    {
        get 
        {
            _isValid ??= IsValidInternal();
            return _isValid.Value;
        }
    }
    private bool? _isValid = null;
    public override string ToString()
    {
        return $"{(IsValid ? "Valid" : "Invalid")} {Total}: {string.Join(" ", Values)}";
    }

    private bool IsValidInternal()
    {
        List<ulong?> currentCounts = [Values.First()];
        
        for (int i = 1; i < Values.Count; i++)
        {
            List<ulong?> newCounts = [];
            foreach (var currentCount in currentCounts)
            {
                var add = currentCount + Values[i];
                if (add <= Total) newCounts.Add(add);
                var mul = currentCount * Values[i];
                if (mul <= Total) newCounts.Add(mul);
                var concat = ulong.Parse(currentCount.ToString() + Values[i].ToString());
                if (concat <= Total) newCounts.Add(concat);
            }
            currentCounts = newCounts;
        }
        if (currentCounts.FirstOrDefault(cc => cc == Total) != null) return true;
        return false;
    }
}