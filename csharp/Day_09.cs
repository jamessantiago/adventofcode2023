using AoCHelper;

namespace adventofcode2023;

public class Day_09 : BaseDay
{
    private readonly List<List<long>> _input;

    public Day_09()
    {
        _input = File.ReadAllLines(InputFilePath).Select(x => x.Split().Select(long.Parse).ToList()).ToList();
    }

    long getHistory(List<long> history)
    {
        var stack = new Stack<List<long>>();
        stack.Push(history);

        while (true)
        {
            var current = stack.Peek();
            if (current.Sum() == 0) break;
            var diffs = current.Zip(current.Skip(1), (a, b) => b - a).ToList();
            stack.Push(diffs);
        }

        long result = 0;
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            result += current.Last();
        }

        return result;
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0L;
        foreach (var line in _input)
        {
            result += getHistory(line);
            //Console.WriteLine(history);
        }
        return new ValueTask<string>(result.ToString());
    }

    long getHistory2(List<long> history)
    {
        var stack = new Stack<List<long>>();
        stack.Push(history);

        while (true)
        {
            var current = stack.Peek();
            if (current.Sum() == 0) break;
            var diffs = current.Zip(current.Skip(1), (a, b) => b - a).ToList();
            stack.Push(diffs);
        }

        long result = 0;
        while (stack.Count > 0)
        {
            var current = stack.Pop().First();
            result = (result - current) * -1;
        }

        return result;
    }

    public override ValueTask<string> Solve_2()
    {
        var result = 0L;
        foreach (var line in _input)
        {
            var history = getHistory2(line);
            //Console.WriteLine(history);
            result += history;
        }
        return new ValueTask<string>(result.ToString());
    }
}