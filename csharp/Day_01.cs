using AoCHelper;

namespace adventofcode2023;

public class Day_01 : BaseDay
{
    private readonly string[] _input;

    public Day_01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        return new ValueTask<string>(_input.Select(l => int.Parse(l.First(x => int.TryParse(x.ToString(), out _)) + l.Last(x => int.TryParse(x.ToString(), out _)).ToString())).Sum().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var sum = 0L;
        foreach (var line in _input)
        {
            var numbers = new List<int>();
            for (int i = 0; i < line.Length; i++)
            {
                numbers.Add(GetNumber(line.Substring(i)));
            }
            sum += int.Parse(numbers.First(x => x > 0).ToString() + numbers.Last(x => x > 0).ToString());
            //Console.WriteLine($"{line} = {numbers.First(x => x > 0)} + {numbers.Last(x => x > 0)}");
        }

        return new ValueTask<string>(sum.ToString());

        // get number from string
        int GetNumber(string line)
        {
            var numbers = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            if (numbers.Any(n => line.StartsWith(n)))
            {
                var number = numbers.First(n => line.StartsWith(n));
                return numbers.IndexOf(number) + 1;
            }
            else if (int.TryParse(line[0].ToString(), out var val))
            {
                return val;
            }
            else
            {
                return 0;
            }
        }
    }
}
