using AoCHelper;

namespace adventofcode2023;

public class Day_07 : BaseDay
{
    List<(string hand, int bid)> hands = new();

    public Day_07()
    {
        hands = File.ReadAllLines(InputFilePath).Select(x => (x.Split(" ")[0], int.Parse(x.Split(" ")[1]))).ToList();
    }

    List<char> strengths = new() { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

    int getType(string hand)
    {
        if (hand.GroupBy(x => x).Count() == 5)
        {
            return 0;
        }
        else if (hand.GroupBy(x => x).Count() == 4)
        {
            return 1;
        }
        else if (hand.GroupBy(x => x).Count() == 3)
        {
            if (hand.GroupBy(x => x).Any(x => x.Count() == 3))
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
        else if (hand.GroupBy(x => x).Count() == 2)
        {
            if (hand.GroupBy(x => x).Any(x => x.Count() == 4))
            {
                return 5;
            }
            else
            {
                return 4;
            }
        }
        else
        {
            return 6;
        }
    }

    int compare(string hand1, string hand2)
    {
        try
        {
            for (int i = 0; i < hand1.Count(); i++)
            {
                if (strengths.IndexOf(hand1[i]) > strengths.IndexOf(hand2[i]))
                {
                    return 1;
                }
                else if (strengths.IndexOf(hand1[i]) < strengths.IndexOf(hand2[i]))
                {
                    return -1;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return 0;
    }

    public override ValueTask<string> Solve_1()
    {
        var typedHands = hands.GroupBy(x => getType(x.hand));
        int rank = 0;
        long result = 0;
        foreach (var handType in typedHands.OrderBy(x => x.Key))
        {
            var sortedHands = handType.ToList();
            try
            {
                sortedHands.Sort((x, y) => compare(x.hand, y.hand));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{handType.Key}");
            }
            foreach (var hand in sortedHands)
            {
                rank++;
                result += rank * hand.bid;
                //Console.WriteLine($"{hand.hand} {hand.bid} {getType(hand.hand)}");
            }
        }
        return new ValueTask<string>(result.ToString());
    }

    
    List<char> strengths2 = new() { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

    int getType2(string hand)
    {
        var j = (char c) => c == 'J';
        var nj = (char c) => c != 'J';
        if (hand.Where(nj).GroupBy(x => x).Count() == 5)
        {
            return 0;
        }
        else if (hand.Where(nj).GroupBy(x => x).Count() == 4)
        {
            return 1;
        }
        else if (hand.Where(nj).GroupBy(x => x).Count() == 3)
        {
            if (hand.Where(nj).GroupBy(x => x).Any(x => x.Count(nj) + hand.Count(j) == 3))
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
        else if (hand.Where(nj).GroupBy(x => x).Count() == 2)
        {
            if (hand.Where(nj).GroupBy(x => x).Any(x => x.Count(nj) + hand.Count(j) == 4))
            {
                return 5;
            }
            else
            {
                return 4;
            }
        }
        else
        {
            return 6;
        }
    }

    int compare2(string hand1, string hand2)
    {
        try
        {
            for (int i = 0; i < hand1.Count(); i++)
            {
                if (strengths2.IndexOf(hand1[i]) > strengths2.IndexOf(hand2[i]))
                {
                    return 1;
                }
                else if (strengths2.IndexOf(hand1[i]) < strengths2.IndexOf(hand2[i]))
                {
                    return -1;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return 0;
    }

    public override ValueTask<string> Solve_2()
    {
        var typedHands = hands.GroupBy(x => getType2(x.hand));
        int rank = 0;
        long result = 0;
        foreach (var handType in typedHands.OrderBy(x => x.Key))
        {
            var sortedHands = handType.ToList();
            sortedHands.Sort((x, y) => compare2(x.hand, y.hand));
            foreach (var hand in sortedHands)
            {
                rank++;
                result += rank * hand.bid;
                //Console.WriteLine($"{hand.hand} {hand.bid} {getType2(hand.hand)}");
            }
        }
        return new ValueTask<string>(result.ToString());
    }
}