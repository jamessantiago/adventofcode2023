using System.Text.RegularExpressions;
using AoCHelper;

namespace adventofcode2023;

public class Day_02 : BaseDay
{
    private readonly string[] _input;
    private readonly List<List<int[]>> _gameSets = new List<List<int[]>>();

    public Day_02()
    {
        _input = File.ReadAllLines(InputFilePath);
        var colors = new List<string>() { "green", "blue", "red" };
        foreach (var line in _input)
        {
            var setsLine = Regex.Replace(line, @"Game \d+: ", "");
            var set = new List<int[]>();
            foreach (var setGroup in setsLine.Split(";"))
            {
                var newSet = new int[3];
                foreach (var match in setGroup.Split(",").Select(x => Regex.Match(x, @"(\d+) (\w+)")))
                {
                    newSet[colors.IndexOf(match.Groups[2].Value)] = int.Parse(match.Groups[1].Value);
                }
                set.Add(newSet);
            }
            _gameSets.Add(set);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        //_gameSets.ForEach(x => Console.WriteLine($"{string.Join(";", x.Select(s => string.Join(",", s)))}"));
        var initial = new int[] { 13, 14, 12 };
        var sum = 0;
        foreach (var game in _gameSets)
        {
            bool gamePossible = true;
            for (int g = 0; g < game.Count; g++)
            {
                var set = game[g];
                for (int i = 0; i < 3; i++)
                {
                    if (set[i] > initial[i])
                    {
                        gamePossible = false;
                        break;
                    }
                }

                if (!gamePossible)
                {
                    break;
                }
            }

            if (gamePossible)
            {
                sum += _gameSets.IndexOf(game) + 1;
            }
        }
        

        return new ValueTask<string>($"{sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        long sum = 0;
        foreach (var game in _gameSets)
        {
            var minSet = new int[] { 0, 0, 0 };
            for (int g = 0; g < game.Count; g++)
            {
                var set = game[g];
                for (int i = 0; i < 3; i++)
                {
                    if (set[i] != 0 && set[i] > minSet[i])
                    {
                        minSet[i] = set[i];
                    }
                }
            }
            //Console.WriteLine($"{string.Join(",", minSet)}");
            sum += minSet.Aggregate(1L, (x, y) => x * y);
        }
        return new ValueTask<string>($"{sum}");
    }
}
