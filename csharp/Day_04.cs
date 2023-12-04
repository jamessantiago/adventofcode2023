using System.Text.RegularExpressions;
using AoCHelper;

namespace adventofcode2023;

public class Day_04 : BaseDay
{
    private readonly string[] _input;
    private readonly List<(List<int> winners, List<int> haves)> cards = new List<(List<int> winners, List<int> haves)>();

    public Day_04()
    {
        _input = File.ReadAllLines(InputFilePath);
        foreach (var line in _input)
        {
            var cardLine = Regex.Replace(line, @"Card +\d+: ", "");
            cards.Add((cardLine.Split("|")[0].Split(" ").Where(x => x != "").Select(int.Parse).ToList(), cardLine.Split("|")[1].Split(" ").Where(x => x != "").Select(int.Parse).ToList()));
        }
    }

    public override ValueTask<string> Solve_1()
    {
        double total = 0;
        foreach (var card in cards)
        {
            var wins = card.haves.Count(card.winners.Contains);
            total += wins switch
            {
                1 => 1,
                0 => 0,
                _ => Math.Pow(2, wins - 1)
            };
        }
        return new ValueTask<string>(total.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var cardCount = new int[_input.Length];
        for (int i = 0; i < cardCount.Length; i++)
        {
            cardCount[i] = 1;
        }

        for (int i = 0; i < cardCount.Length; i++)
        {
            var card = cards[i];
            for (int r = 0; r < cardCount[i]; r++)
            {
                var wins = card.haves.Count(card.winners.Contains);
                for (int w = i + 1; w < i + 1 + wins; w++)
                {
                    cardCount[w]++;
                }
                if (i != 0 && cardCount[i] == 1)
                {
                    break;
                }
            }
            
        }
        return new ValueTask<string>(cardCount.Sum().ToString());
    }
}