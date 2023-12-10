using AoCHelper;

namespace adventofcode2023;

public class Day_10 : BaseDay
{
    private readonly string[] _input;

    public Day_10()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    private Stack<(int y, int x, int s)> getStart()
    {
        for (var y = 0; y < _input.Length; y++)
        {
            for (var x = 0; x < _input[y].Length; x++)
            {
                if (_input[y][x] == 'S')
                {
                    visted.Add((y, x, 0));
                    var stack = new Stack<(int y, int x, int s)>();
                    if (y > 0 && (_input[y - 1][x] == '|' || _input[y - 1][x] == '7' || _input[y - 1][x] == 'F'))
                    {
                        stack.Push((y - 1, x, 1));
                    }

                    if (y < _input.Length - 1 && (_input[y + 1][x] == '|' || _input[y + 1][x] == 'J' || _input[y + 1][x] == 'L'))
                    {
                        stack.Push((y + 1, x, 1));
                    }

                    if (x > 0 && (_input[y][x - 1] == '-' || _input[y][x - 1] == 'L' || _input[y][x - 1] == 'F'))
                    {
                        stack.Push((y, x - 1, 1));
                    }

                    if (x < _input[y].Length - 1 && (_input[y][x + 1] == '-' || _input[y][x + 1] == 'J' || _input[y][x + 1] == '7'))
                    {
                        stack.Push((y, x + 1, 1));
                    }

                    return stack;
                }
            }
        }

        throw new Exception("No start found");
    }
    
    HashSet<(int y, int x, int s)> visted = new HashSet<(int y, int x, int s)>();
    int result = 0;
    public override ValueTask<string> Solve_1()
    {
        var stack = getStart();
        var steps = 0;
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (_input[current.y][current.x] == 'S' || _input[current.y][current.x] == '.') continue;

            //if (visted.Any(v => v.x == current.x && v.y == current.y))
            //{
            //    Console.WriteLine($"Loop detected at {current}");
            //    break;
            //}
            //else
            //{
            //Console.WriteLine(current);
            //}

            steps = current.s + 1;
            visted.Add(current);

            var isVisted = (int y, int x) => visted.Any(v => v.x == x && v.y == y);

            if (current.y > 0 && !isVisted(current.y - 1, current.x) && (_input[current.y][current.x] == '|' || _input[current.y][current.x] == 'J' ||
                                                                         _input[current.y][current.x] == 'L'))
            {
                if (_input[current.y - 1][current.x] == '|' || _input[current.y - 1][current.x] == 'F' ||
                    _input[current.y - 1][current.x] == '7')
                {
                    stack.Push((current.y - 1, current.x, steps));
                }
            }

            if (current.y < _input.Length - 1 && !isVisted(current.y + 1, current.x) &&
                (_input[current.y][current.x] == '|' || _input[current.y][current.x] == '7' ||
                 _input[current.y][current.x] == 'F'))
            {
                if (_input[current.y + 1][current.x] == '|' || _input[current.y + 1][current.x] == 'L' ||
                    _input[current.y + 1][current.x] == 'J')
                {
                    stack.Push((current.y + 1, current.x, steps));
                }
                
            }

            if (current.x > 0 && !isVisted(current.y, current.x - 1) && (_input[current.y][current.x] == '-' || _input[current.y][current.x] == 'J' ||
                                                                         _input[current.y][current.x] == '7'))
            {
                if (_input[current.y][current.x - 1] == '-' || _input[current.y][current.x - 1] == 'L' ||
                    _input[current.y][current.x - 1] == 'F')
                {
                    stack.Push((current.y, current.x - 1, steps));
                }
            }

            if (current.x < _input[current.y].Length - 1 && !isVisted(current.y, current.x + 1) && (_input[current.y][current.x] == '-' ||
                                                                                                    _input[current.y][current.x] == 'L' ||
                                                                                                    _input[current.y][current.x] == 'F'))
            {
                if (_input[current.y][current.x + 1] == '-' || _input[current.y][current.x + 1] == 'J' ||
                    _input[current.y][current.x + 1] == '7')
                {
                    stack.Push((current.y, current.x + 1, steps));
                }
            }
        }

        result = (visted.Select(x => x.s).Max() + 1) / 2;
        return new ValueTask<string>(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        //visted = new HashSet<(int y, int x, int s)>(visted.Select(v => (v.y, v.x, v.s == 0 ? 0 : v.s + 1)));
        var isVisted = (int y, int x) => visted.Any(v => v.x == x && v.y == y);
        var visitedStep = (int y, int x) => visted.First(v => v.x == x && v.y == y).s;
        var countDown = (int y, int x) => visted.Count(v => v.x == x && v.y > y);
        var countUp = (int y, int x) => visted.Count(v => v.x == x && v.y < y);
        var countRight = (int y, int x) => visted.Count(v => v.y == y && v.x > x && (_input[y][x] == '|' || _input[y][x] == 'J' || _input[y][x] == 'L'));
        var internals = 0;
        var upPipes = new Dictionary<int, int>();
        var pipeLength = result * 2;
        for (int y = 0; y < _input.Length; y++)
        {
            var leftPipes = 0;
            for (int x = 0; x < _input[y].Length; x++)
            {
                if (isVisted(y, x))
                {
                    //upPipes[x] = upPipes.GetValueOrDefault(x, 0) + 1;
                    if (_input[y][x] == '|' || _input[y][x] == 'J' || _input[y][x] == 'L') leftPipes++;
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(_input[y][x]);
                    continue;
                }
                
                if (leftPipes % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(_input[y][x]);
                    
                }
                else if (countUp(y, x) > 0)
                {
                    internals++;
                    //Console.WriteLine($"Postion {x},{y} is internal");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('I');
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(_input[y][x]);
                }
                //if (isVisted(y, x) && isVisted(y + 1, x))
                //{
                //    if (visitedStep(y + 1, x) == (visitedStep(y, x) + 1) % pipeLength) leftPipes++;
                //    else if ((visitedStep(y + 1, x) + 1) % pipeLength == visitedStep(y, x)) leftPipes--;
                //}

                //internals += !isVisted(y, x) && leftPipes != 0 ? 1 : 0;
            }
            
            Console.WriteLine();
        }

        return new ValueTask<string>(internals.ToString());
    }
}