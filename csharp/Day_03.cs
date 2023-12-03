using System.Text.RegularExpressions;
using AoCHelper;

namespace adventofcode2023;

public class Day_03 : BaseDay
{
    private readonly string[] _input;

    public Day_03()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        bool isSym(char c) => Regex.IsMatch(c.ToString(), @"[^\d\.]");
        bool isNum(char c) => Regex.IsMatch(c.ToString(), @"\d");
        var nums = new List<int>();
        var covered = new List<(int, int)>();

        int getNum(int x, int y)
        {
            var number = "";
            var nx = 0; var ny = 0;
            while (x >= 0 && x < _input[0].Length && y >= 0 && y < _input.Length)
            {
                if (isNum(_input[y][x]))
                {
                    nx = x;
                    ny = y;
                }
                else
                {
                    break;
                }
                x--;
            }

            while (nx >= 0 && nx < _input[0].Length && ny >= 0 && ny < _input.Length)
            {
                if (covered.Contains((nx, ny)))
                {
                    return 0;
                }
                if (isNum(_input[ny][nx]))
                {
                    number += _input[ny][nx];
                    covered.Add((nx, ny));
                }
                else
                {
                    break;
                }
                nx++;
            }
            if (number == "") return 0;
            //Console.WriteLine($"{number} at {x},{y}");
            return int.Parse(number);
        }

        for (int y = 0; y < _input.Length; y++)
        for (int x = 0; x < _input[0].Length; x++)
        {
            var c = _input[y][x];
            if (isSym(c))
            {
                var up = getNum(x, y - 1);
                if (up > 0) nums.Add(up);
                var down = getNum(x, y + 1);
                if (down > 0) nums.Add(down);
                var left = getNum(x - 1, y);
                if (left > 0) nums.Add(left);
                var leftup = getNum(x - 1, y - 1);
                if (leftup > 0) nums.Add(leftup);
                var leftdown = getNum(x - 1, y + 1);
                if (leftdown > 0) nums.Add(leftdown);
                var right = getNum(x + 1, y);
                if (right > 0) nums.Add(right);
                var rightup = getNum(x + 1, y - 1);
                if (rightup > 0) nums.Add(rightup);
                var rightdown = getNum(x + 1, y + 1);
                if (rightdown > 0) nums.Add(rightdown);
            }
        }
        
        return new ValueTask<string>(nums.Sum().ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        bool isGear(char c) => Regex.IsMatch(c.ToString(), @"[^\d\.]");
        bool isNum(char c) => Regex.IsMatch(c.ToString(), @"\d");
        var covered = new List<(int, int)>();

        int getNum(int x, int y)
        {
            var number = "";
            var nx = 0; var ny = 0;
            while (x >= 0 && x < _input[0].Length && y >= 0 && y < _input.Length)
            {
                if (isNum(_input[y][x]))
                {
                    nx = x;
                    ny = y;
                }
                else
                {
                    break;
                }
                x--;
            }

            while (nx >= 0 && nx < _input[0].Length && ny >= 0 && ny < _input.Length)
            {
                if (covered.Contains((nx, ny)))
                {
                    return 0;
                }
                if (isNum(_input[ny][nx]))
                {
                    number += _input[ny][nx];
                    covered.Add((nx, ny));
                }
                else
                {
                    break;
                }
                nx++;
            }
            if (number == "") return 0;
            //Console.WriteLine($"{number} at {x},{y}");
            return int.Parse(number);
        }

        long gearSum = 0;

        for (int y = 0; y < _input.Length; y++)
        for (int x = 0; x < _input[0].Length; x++)
        {
            var c = _input[y][x];
            if (isGear(c))
            {
                var tempNums = new List<int>();
                var up = getNum(x, y - 1);
                if (up > 0) tempNums.Add(up);
                var down = getNum(x, y + 1);
                if (down > 0) tempNums.Add(down);
                var left = getNum(x - 1, y);
                if (left > 0) tempNums.Add(left);
                var leftup = getNum(x - 1, y - 1);
                if (leftup > 0) tempNums.Add(leftup);
                var leftdown = getNum(x - 1, y + 1);
                if (leftdown > 0) tempNums.Add(leftdown);
                var right = getNum(x + 1, y);
                if (right > 0) tempNums.Add(right);
                var rightup = getNum(x + 1, y - 1);
                if (rightup > 0) tempNums.Add(rightup);
                var rightdown = getNum(x + 1, y + 1);
                if (rightdown > 0) tempNums.Add(rightdown);
                if (tempNums.Count == 2)
                {
                    //Console.WriteLine($"{string.Join(",", tempNums)} at {x},{y}");
                    gearSum += tempNums[0] * tempNums[1];
                }
            }
        }
        
        return new ValueTask<string>(gearSum.ToString());
    }
}