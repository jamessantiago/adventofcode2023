using System.Text.RegularExpressions;
using AoCHelper;

namespace adventofcode2023;

public class Day_06 : BaseDay
{
    private readonly string[] _input;
    private readonly List<(long time, long distance)> _races = new();
    private readonly (long time, long distance) _race2 = new();

    public Day_06()
    {
        _input = File.ReadAllLines(InputFilePath);
        var times = Regex.Matches(_input[0], @"\d+").Select(x => long.Parse(x.Value)).ToArray();
        var distances = Regex.Matches(_input[1], @"\d+").Select(x => long.Parse(x.Value)).ToArray();
        for (var i = 0; i < times.Length; i++)
        {
            _races.Add((times[i], distances[i]));
        }
        var times2 = Regex.Matches(_input[0].Replace(" ", ""), @"\d+").Select(x => long.Parse(x.Value)).ToArray();
        var distances2 = Regex.Matches(_input[1].Replace(" ", ""), @"\d+").Select(x => long.Parse(x.Value)).ToArray();
        _race2 = (times2[0], distances2[0]);
    }

    private double quad(long a, long b, long c)
    {
        var preRoot = b * b - 4 * a * c;
        return (b + Math.Sqrt(preRoot)) / (2 * a);
    }

    private long quad2good(long time, long distance)
    {
        var min = (long)((time - Math.Sqrt(time * time - 4 * distance)) / 2);
        //Console.WriteLine($"Min: {min}");
        var max = (time + Math.Sqrt(time * time - 4 * distance)) / 2;
        //Console.WriteLine($"Max: {max}");
        return max != (long)max ? (long)(max - min) : (long)(max - min) - 1;
    }

    public override ValueTask<string> Solve_1()
    {
        long res = 1;
        foreach (var race in _races)
        {
            var r = quad2good(race.time, race.distance);
            //Console.WriteLine($"Race results: {r}");
            res *= r;
        }

        return new ValueTask<string>(res.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        long res = quad2good(_race2.time, _race2.distance);

        return new ValueTask<string>(res.ToString());
    }
}