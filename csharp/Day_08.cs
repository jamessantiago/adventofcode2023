using AoCHelper;

namespace adventofcode2023;

public class Day_08 : BaseDay
{
    private readonly string[] _input;
    private string path;
    Dictionary<string, (string left, string right)> instructions = new();

    public Day_08()
    {
        _input = File.ReadAllLines(InputFilePath);
        path = _input[0];
        var delims = new[] { ' ', '=', ')', '(', ',' };
        instructions = _input[2..].Select(x => x.Split(delims).Where(s => s != "").ToArray()).ToDictionary(x => x[0], x => (x[1], x[2]));
        //Console.WriteLine($"Path: {path}");
        //foreach (var (key, value) in instructions)
        //{
        //    Console.WriteLine($"{key} => {value.left} {value.right}");
        //}
    }

    public override ValueTask<string> Solve_1()
    {
        int i = 0;
        int steps = 0;
        var elem = "AAA";
        while (true)
        {
            steps++;
            elem = path[i] == 'L' ? instructions[elem].left : instructions[elem].right;
            //Console.WriteLine($"Step {steps}: {path[i]} {elem}");
            if (elem == "ZZZ") return new ValueTask<string>(steps.ToString());
            i = (i + 1) % path.Length;
        }

        return new ValueTask<string>();
    }

    static long gcf(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long lcm(long a, long b)
    {
        return (a / gcf(a, b)) * b;
    }

    public override ValueTask<string> Solve_2()
    {
        int i = 0;
        long steps = 0;
        var elems = instructions.Keys.Where(x => x[2] == 'A').ToArray();
        var complete = new List<long>();
        while (true)
        {
            steps++;
            var next = new List<string>();
            for (var j = 0; j < elems.Length; j++)
            {
                elems[j] = path[i] == 'L' ? instructions[elems[j]].left : instructions[elems[j]].right;
                if (elems[j][2] == 'Z') complete.Add(steps);
                else next.Add(elems[j]);
            }

            if (!next.Any()) break;
            elems = next.ToArray();
            //Console.WriteLine($"Step {steps}: {path[i]} {string.Join(", ", elems)}");
            i = (i + 1) % path.Length;
        }

        steps = complete.OrderByDescending(x => x).Aggregate(lcm);

        return new ValueTask<string>(steps.ToString());
    }
}