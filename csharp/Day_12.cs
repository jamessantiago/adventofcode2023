using System.Text.RegularExpressions;
using AoCHelper;

namespace adventofcode2023;

public class Day_12 : BaseDay
{
    private readonly List<(string record, int[] brokes)> _input;

    public Day_12()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.Split(' ')).Select(x => (x[0], x[1].Split(',').Select(int.Parse).ToArray())).ToList();
    }

    public override ValueTask<string> Solve_1()
    {
        long sum = 0;
        var fromUnknown = new Regex(@"\?");
        foreach (var (record, brokes) in _input)
        {
            //Console.WriteLine("Checking " + record + " " + string.Join(",", brokes));
            var validPattern = @"^\.*" + string.Join("", brokes.Select(x => $@"#{{{x}}}\.+"));
            validPattern = validPattern[..^1] + "*$";
            //Console.WriteLine(validPattern);
            var isValid = new Regex(validPattern);
            var stack = new Stack<string>();
            stack.Push(record);
            var thisSum = 0;
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var hasUnknown = fromUnknown.IsMatch(current);
                if (isValid.IsMatch(current) && !hasUnknown)
                {
                    thisSum++;
                    //Console.WriteLine(current);
                }
                else if (hasUnknown)
                {
                    stack.Push(fromUnknown.Replace(current, ".", 1));
                    stack.Push(fromUnknown.Replace(current, "#", 1));
                }
            }
            sum += thisSum;
            //Console.WriteLine(thisSum);
        }
        //long sum = 0;
        //int i = 0;
        //foreach (var (r, b) in _input)
        //{
        //    _record = r;
        //    //_cache.Clear();

        //    var thisSum = dp(i, 0, 0, b);
        //    i++;
        //    sum += thisSum;
        //    //Console.WriteLine(thisSum);
        //}
        //Console.WriteLine(sum);
        return new ValueTask<string>(sum.ToString());
    }

    private string _record;
    Dictionary<(int stri, int ri, int brokeCount, int hash), long> _cache = new();
    Dictionary<(int a, int l), int> hashMap = new();

    int getHash(int a, int[] array)
    {
        var l = array.Length;
        var hc = l;
        if (hashMap.ContainsKey((a, l))) return hashMap[(a, l)];
        for (var i = 0; i < l; i++)
        {
            hc = unchecked(hc * 314159 + array[i]);
        }
        
        return hashMap[(a, l)] = hc;
    }

    long dp(int stri, int ri, int brokeCount, int[] brokeLeft)
    {
        var hash = getHash(stri, brokeLeft);
        if (_cache.ContainsKey((stri, ri, brokeCount, hash))) return _cache[(stri, ri, brokeCount, hash)];

        // end good
        var empty = ri == _record.Length;
        if (empty && brokeCount == 0 && brokeLeft.Length == 0) return _cache[(stri, ri, brokeCount, hash)] = 1;
        if (empty && brokeCount > 0 && brokeCount == brokeLeft.FirstOrDefault()) return _cache[(stri, ri, brokeCount, hash)] = 1;

        // no solution in path
        if (empty) return 0;
        var left = _record[ri..].Count(x => x is '#' or '?');
        if (brokeCount > 0 && left + brokeCount < brokeLeft.Sum()) return _cache[(stri, ri, brokeCount, hash)] = 0;
        if (brokeCount > 0 && brokeLeft.Length == 0) return _cache[(stri, ri, brokeCount, hash)] = 0;
        if (brokeCount == 0 && left < brokeLeft.Sum()) return _cache[(stri, ri, brokeCount, hash)] = 0;
        if (_record[ri] == '.' && brokeCount > 0 && brokeCount != brokeLeft[0]) return _cache[(stri, ri, brokeCount, hash)] = 0;

        var result = 0L;

        // restart next broke group
        if ((_record[ri] == '.' && brokeCount > 0) || (_record[ri] == '?' && brokeCount > 0 && brokeCount == brokeLeft[0]))
        {
            result += dp(stri, ri + 1, 0, brokeLeft[1..]);
        }
        
        // continue
        if (_record[ri] is '#' or '?') result += dp(stri, ri + 1, brokeCount + 1, brokeLeft);

        // try again
        if (_record[ri] is '.' or '?' && brokeCount == 0) result += dp(stri, ri + 1, 0, brokeLeft);

        //Console.WriteLine($"{_record} {brokeCount} {string.Join(",", brokeLeft)} {result}");
        return _cache[(stri, ri, brokeCount, hash)] = result;
    }

    public override ValueTask<string> Solve_2()
    {
        long sum = 0;
        int i = 0;
        foreach (var (r, b) in _input)
        {
            var record = string.Join("?", Enumerable.Repeat(r, 5));
            var brokes = Enumerable.Repeat(b, 5).SelectMany(x => x).ToArray();

            _record = record;
            //_cache.Clear();

            var thisSum = dp(i, 0, 0, brokes);
            i++;
            sum += thisSum;
            //Console.WriteLine(thisSum);
        }
        return new ValueTask<string>(sum.ToString());
    }
}