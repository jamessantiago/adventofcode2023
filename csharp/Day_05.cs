using System.Text.RegularExpressions;
using AoCHelper;

namespace adventofcode2023;

public class Day_05 : BaseDay
{
    private readonly string[] _input;
    private readonly long[] _seeds;
    private readonly List<(long level, long source, long dest, long len)> _mapping = new();
    private readonly long _maxLevel = 6;
    
    public Day_05()
    {
        _input = File.ReadAllLines(InputFilePath);
        _seeds = Regex.Matches(_input[0], @"\d+").Select(x => long.Parse(x.Value)).ToArray();
        long level = _maxLevel;

        foreach (var line in _input[2..].Reverse())
        {
            if (line.EndsWith(":"))
            {
                level--;
                continue;
            } 
            else if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var map = Regex.Matches(line, @"\d+").Select(x => long.Parse(x.Value)).ToArray();
            var mapping = (level, map[1], map[0], map[2]);
            _mapping.Add(mapping);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        long getLocation(long seed)
        {
            var dest = seed;
            for (long i = 0; i <= _maxLevel; i++)
            {
                var source = dest;
                var level = i;
                var map = _mapping.FirstOrDefault(m => m.level == level && source >= m.source && source <= m.source + m.len);
                if (map != default)
                {
                    var offset = source > map.source ? source - map.source : source + map.source;
                    dest = map.dest + offset;
                }
            }
            return dest;
        }

        return new ValueTask<string>(_seeds.Min(getLocation).ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        List<long[]> GetRanges(long startA, long endA, long startB, long endB)
        {
            var results = new List<long[]>();
            if (startA >= startB && endA <= endB)
            {
                results.Add(new[] { startA, endA });
            }
            else if (startA <= startB)
            {
                results.Add(new[] { startB, endA });
                results.Add(new[] { startA, startB - 1 });
            }
            else if (startA > startB)
            {
                results.Add(new[] { startA, endB });
                results.Add(new[] { endB + 1, endA });
            }

            return results;
        }

        
        (long dest, long len, long level)[] getLocation(long seed, long seedlen, long level)
        {
            if (level > _maxLevel)
            {
                return new[] { (seed, seedlen, level) };
            }

            var result = new List<(long dest, long len, long level)>();
            var source = seed;
            var sourceEnd = source + seedlen - 1;
            bool found = false;
            foreach (var map in _mapping.Where(m => m.level == level && ((source >= m.source && source <= m.source + m.len - 1) || (sourceEnd >= m.source && sourceEnd <= m.source + m.len - 1))))
            {
                var offset = source == map.source ? 0 : source > map.source ? source - map.source : map.source - source;
                var ranges = GetRanges(source, sourceEnd, map.source, map.source + map.len - 1);

                result.AddRange(getLocation(map.dest + offset, ranges[0][1] - ranges[0][0] + 1, level + 1));
                //Console.WriteLine($"found: {level}, source: {source}, dest: {map.dest + offset}, destLen: {ranges[0][1] - ranges[0][0] + 1}");
                if (ranges.Count > 1)
                {
                    result.AddRange(getLocation(ranges[1][0], ranges[1][1] - ranges[1][0] + 1, level == _maxLevel ? level + 1 : level));
                    //Console.WriteLine($"left: {level}, source: {source}, dest: {ranges[1][0]}, destLen: {ranges[1][1] - ranges[1][0] + 1}");
                }
                found = true;
            }

            if (!found)
            {
                result.AddRange(getLocation(seed, seedlen, level + 1));
                //Console.WriteLine($"notfound: {level}, source: {source}, dest: {seed}, destLen: {seedlen}");
            }

            return result.ToArray();
        }

        var minLocation = long.MaxValue;
        for (long i = 0; i < _seeds.Length; i += 2)
        {
            var locations = getLocation(_seeds[i], _seeds[i + 1], 0);
            minLocation = Math.Min(minLocation, locations.Min(x => x.dest));
        }

        return new ValueTask<string>(minLocation.ToString());
    }
}