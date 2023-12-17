using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\7.txt");
        List<Hand> hands = new List<Hand>();
        foreach (string s in strings)
        {
            hands.Add(new Hand(s));
        }

        hands.Sort();
        int res = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            Hand h = hands[i];
            res += h.bid * (i + 1);
        }
        Console.WriteLine(res);
    }
}

public class Hand : IComparable<Hand>   
{
    public string cards { get; set;}

    public int bid { get; set;}
    public HandVal handVal { get; set;}
    public Hand(string input)
    {
        this.cards = input.Substring(0,5);
        this.bid = int.Parse( Regex.Match(input, @" \d+").Value);
        getVal();

    }

    private void getVal()
    {
        List<HandVal> hands = new List<HandVal>();
        var dist = cards.Distinct();
        if (dist.Any(o => o == 'J'))
        {
            foreach(var c in dist)
            {
                hands.Add(getVal(cards.Replace('J', c)));
            }
            handVal = hands.Max();
        }
        else
        {
              handVal = getVal(cards);
        }
    
    }
    private HandVal getVal(string cards)
    {
        HandVal handVal;
        if (cards.Distinct().Count() == 1) handVal = HandVal.Five;
        else if (cards.Distinct().Count() == 2)
        {
            char c1 = cards.Distinct().First();
            char c2 = cards.Distinct().Last();

            if (cards.Count(o => o == c1) == 1 || (cards.Count(o => o == c1) == 4)) handVal = HandVal.Four;
            else handVal = HandVal.FH;
        }
        else if (cards.Distinct().Count() == 3)
        {
            char[] cs = new char[3];
            cs[0] = cards.Distinct().First();
            cs[1]  = cards.Distinct().ElementAt(1);
            cs[2] = cards.Distinct().ElementAt(2);

            List<int> counts = new List<int>();
            counts.Add(cards.Count(o => o == cs[0]));
            counts.Add(cards.Count(o => o == cs[1]));
            counts.Add(cards.Count(o => o == cs[2]));


            if(counts.Any(o => o == 3)) handVal = HandVal.Three;
            else handVal = HandVal.tPair;

        }
        else if (cards.Distinct().Count() == 4) handVal = HandVal.oPair;
        else
        {
            handVal = HandVal.HC;
        }
        return handVal;
    }

    public int CompareTo(Hand? other)
    {
        if(other == null) return 0;
        if(other == this) return 0;
        if(other.handVal > handVal) return -1;
        if(other.handVal < handVal) return 1;

        for (int i = 0; i < cards.Length; i++) {
            char c = cards[i];
            if (getCardVal(c) > getCardVal(other.cards[i])) return 1;
            else if(getCardVal(c) < getCardVal(other.cards[i])) return -1;
        }
        return 0;
    }

    private int getCardVal(char c)
    {
        if(int.TryParse(c.ToString(),out int num))
        {
            return num;
        }
        else
        {
            if (c == 'T') return 10;
            if (c == 'J') return 1;
            if (c == 'Q') return 12;
            if (c == 'K') return 13;
            if (c == 'A') return 14;
        }
        throw new Exception();
    }
    public enum HandVal
    {
        HC,
        oPair,
        tPair,
        Three,
        FH,
        Four,
        Five
    }
}