using System.Diagnostics;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\19.txt");

        List<Rule> rules = new List<Rule>();
        int i = 0;
        do

        {
            string mString = strings[i++];
            if (String.IsNullOrWhiteSpace(mString)) break;
            rules.Add(new Rule(mString));
        }while (i < strings.Length);
        
        foreach (Rule rule in rules) { rule.SetConditions(rules); }
        Rule start = rules.First(o => o.Name == "in");
        int counter = 0;
        Stopwatch sw = Stopwatch.StartNew();
        do
        {
            string mString = strings[i++];
            if (start.IsAccepted(mString).Value)
            {
                int nums = Regex.Matches(mString,@"\d+").Sum(o => int.Parse(o.Value));
                counter += nums;
            }
        }while (i < strings.Length);
        Console.WriteLine(sw.ElapsedMilliseconds);
        Console.WriteLine(counter.ToString());
    }
}

public class Rule
{
  public  string Name { get; set; }
    private string inputString;
  public Condition[] conditions { get; set; }

    public Rule(string input)
    {
        this.inputString = input;
        Name = inputString.Split('{')[0];
    }

    public void SetConditions(List<Rule> rules)
    {
        string input = inputString.Split('{')[1];
        input = input.Substring(0,input.Length-1);
        string[] conditionStrings = input.Split(',');
        
        conditions = new Condition[conditionStrings.Length];
        for (int i = 0; i < conditionStrings.Length; i++)
        {
            string condition = conditionStrings[i];
            conditions[i] = new Condition(condition, rules);
        }
    }


    public bool? IsAccepted(string input)
    {
        foreach(Condition condition in conditions)
        {
            bool? res = condition.Procces(input);
            if(res.HasValue) return res.Value;
        }   
        return null;
    }
}

public class Condition
{
    public bool? AlwaysReject { get; set; } = null;
    public Rule AlwaysRule { get; set; } = null;
    char VariabelName { get; set; }
    string Check { get; set; }
    bool? RejectedIFTrue { get; set; } = null;
    Rule IfAccepted { get; set; }


    public Condition(string input,List<Rule> Rules)
    {
        if (!input.Contains(':'))
        {
            if (input == "A") AlwaysReject = false;
            else if (input == "R") AlwaysReject = true;
            else AlwaysRule = Rules.First(o => o.Name == input);
        }
        else
        {
            VariabelName = input.First();
            Check = input.Substring(1).Split(':')[0];

            string IfYes = input.Split(':')[1];

            if (IfYes == "A") RejectedIFTrue = false;
            else if (IfYes == "R") RejectedIFTrue = true;
            else IfAccepted = Rules.First(o => o.Name == IfYes);
        }

    }

    public bool? Procces(string input)
    {
        if (AlwaysReject.HasValue && AlwaysReject.Value) return false;
        else if(AlwaysReject.HasValue && !AlwaysReject.Value) return true;
        else if(!AlwaysReject.HasValue && AlwaysRule != null) return AlwaysRule.IsAccepted(input);
        else
        {
            string constring = input.Substring(1,input.Length -2);
            string[] cons = constring.Split(",");
            string con = cons.First(o => o[0] == VariabelName);

            bool isTrue = false;
            if (Check[0] == '<')
            {
                isTrue = int.Parse(con.Substring(2)) < int.Parse(Check.Substring(1));
            }else if (Check[0] == '>')
            {
                isTrue = int.Parse(con.Substring(2))  > int.Parse(Check.Substring(1));
            }
            else
            {
                isTrue = int.Parse(con.Substring(2)) == int.Parse(Check.Substring(1));
            }
            if(!isTrue) return null;
            if(RejectedIFTrue.HasValue && RejectedIFTrue.Value) return false;
            else if(RejectedIFTrue.HasValue && !RejectedIFTrue.Value) return true;
            else return IfAccepted.IsAccepted(input);
        }
    }
}