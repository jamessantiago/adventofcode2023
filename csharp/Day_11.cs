using System.Numerics;
using AoCHelper;

namespace adventofcode2023;

public class Day_11 : BaseDay
{
    private List<string> _input;

    public Day_11()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
    }

    private void expandUniverse()
    {
        var rowsToExpand = new List<int>();
        var colsToExpand = new List<int>();
        for (var i = 0; i < _input.Count; i++)
        {
            if (!_input[i].Contains('#')) rowsToExpand.Add(i);
        }

        for (var i = 0; i < _input[0].Length; i++)
        {
            var add = true;
            foreach (var row in _input)
            {
                if (row[i] == '#')
                {
                    add = false;
                    break;
                }
            }

            if (add) colsToExpand.Add(i);
        }

        rowsToExpand.Reverse();
        colsToExpand.Reverse();

        var blankLine = new string('.', _input[0].Length);
        foreach (var row in rowsToExpand)
        {
            _input.Insert(row, blankLine);
        }

        for (var i = 0; i < _input.Count; i++)
        {
            foreach (var col in colsToExpand)
            {
                _input[i] = _input[i].Insert(col, ".");
            }
        }
    }

    private List<(int y, int x)> getGalaxies()
    {
        var galaxies = new List<(int y, int x)>();
        for (var y = 0; y < _input.Count; y++)
        {
            for (var x = 0; x < _input[y].Length; x++)
            {
                if (_input[y][x] == '#') galaxies.Add((y, x));
            }
        }

        return galaxies;
    }

    public override ValueTask<string> Solve_1()
    {
        expandUniverse();
        var galaxies = getGalaxies();
        var galaxyPairs = new HashSet<(int y, int x, int y2, int x2)>();
        foreach (var galaxy in galaxies)
        {
            foreach (var galaxy2 in galaxies)
            {
                if (galaxy == galaxy2) continue;

                if (galaxy.y * _input.Count + galaxy.x > galaxy2.y * _input.Count + galaxy2.x)
                {
                    galaxyPairs.Add((galaxy.y, galaxy.x, galaxy2.y, galaxy2.x));
                }
                else
                {
                    galaxyPairs.Add((galaxy2.y, galaxy2.x, galaxy.y, galaxy.x));
                }
            }
        }

        //Console.WriteLine(galaxyPairs.Count());
        //foreach (var galaxyPair in galaxyPairs.OrderBy(x => x.x))
        //{
        //    Console.WriteLine($"{galaxyPair.y} {galaxyPair.x} {galaxyPair.y2} {galaxyPair.x2}");
        //}
        var distances = galaxyPairs.Select(g => Math.Abs(g.y - g.y2) + Math.Abs(g.x - g.x2)).ToList();
        

        return new ValueTask<string>(distances.Sum().ToString());
    }

    private (List<int> rows, List<int> cols) getExpansion()
    {
        var rowsToExpand = new List<int>();
        var colsToExpand = new List<int>();
        for (var i = 0; i < _input.Count; i++)
        {
            if (!_input[i].Contains('#')) rowsToExpand.Add(i);
        }

        for (var i = 0; i < _input[0].Length; i++)
        {
            var add = true;
            foreach (var row in _input)
            {
                if (row[i] == '#')
                {
                    add = false;
                    break;
                }
            }

            if (add) colsToExpand.Add(i);
        }

        return (rowsToExpand, colsToExpand);
    }

    public override ValueTask<string> Solve_2()
    {
        _input = File.ReadAllLines(InputFilePath).ToList();
        var (rowsToExpand, colsToExpand) = getExpansion();
        var galaxies = getGalaxies();
        var galaxyPairs = new HashSet<(int y, int x, int y2, int x2)>();
        foreach (var galaxy in galaxies)
        {
            foreach (var galaxy2 in galaxies)
            {
                if (galaxy == galaxy2) continue;

                if (galaxy.y * _input.Count + galaxy.x > galaxy2.y * _input.Count + galaxy2.x)
                {
                    galaxyPairs.Add((galaxy.y, galaxy.x, galaxy2.y, galaxy2.x));
                }
                else
                {
                    galaxyPairs.Add((galaxy2.y, galaxy2.x, galaxy.y, galaxy.x));
                }
            }
        }

        BigInteger totalDistance = 0;
        var expansion = 999999;
        foreach (var galaxyPair in galaxyPairs)
        {
            long distance = Math.Abs(galaxyPair.y - galaxyPair.y2) + Math.Abs(galaxyPair.x - galaxyPair.x2);
            var maxX = Math.Max(galaxyPair.x, galaxyPair.x2);
            var minX = Math.Min(galaxyPair.x, galaxyPair.x2);
            var maxY = Math.Max(galaxyPair.y, galaxyPair.y2);
            var minY = Math.Min(galaxyPair.y, galaxyPair.y2);
            var rowExpansion = rowsToExpand.Where(y => y > minY && y < maxY).ToList();
            var colExpansion = colsToExpand.Where(x => x > minX && x < maxX).ToList();
            distance += (rowExpansion.Count * expansion) + (colExpansion.Count * expansion);
            totalDistance += distance;
        }

        return new ValueTask<string>(totalDistance.ToString());
    }
}