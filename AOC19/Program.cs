using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Security.Cryptography;
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
        foreach (Rule rule in rules) { rule.SetParent(rules); }
        Rule start = rules.First(o => o.Name == "in");
        int counter = 0;
        //   var Ends = rules.Select(o => o.conditions.Select(o => o.AlwaysRule is null ? (o.IfAccepted is null ? null : o.IfAccepted) : o.AlwaysRule).Where(o => o != null).Select(o => o.Name));
        Console.WriteLine(rules.Count(o => o.Parent == null));
        Console.WriteLine(rules.Count(o => o.Childs.Count == 0));
        Console.WriteLine(rules.Sum(o => o.conditions.Count(o => o.CanReject)));

        List<Condition> CanReject = rules.SelectMany(o => o.conditions.Where(o => o.CanReject)).ToList();
        
        List<List<Check>> checkList = new List<List<Check>>();
        foreach (Condition condition in CanReject)
        {
            List<Check> checks = new List<Check>();
            Condition cond = condition;
            Rule Parent = cond.Parent;
            do
            {
                if (cond.CheckString != null) checks.Add(cond.Check);

                if (Parent.Parent == null) break;
              
                
                cond = Parent.Parent.conditions.First(o => o.Child == Parent);
               var notCons =  Parent.Parent.conditions.TakeWhile(o => o != cond);
                if(notCons.Count() > 0)
                {
                    checks.AddRange(notCons.Select(o => { var mcheck = o.Check; mcheck.Inverse(); return mcheck; }));
                }
                Parent = cond.Parent;
            } while (true);
         //   Console.WriteLine(String.Join(" && ", checks.Select(o => o.VariabelName.ToString() + o.Opperator + o.Value)));

            var GroupedChecks = checks.GroupBy(o => o.VariabelName);
            List<Check> newChecks = new List<Check>();
            foreach (var checkGroup in GroupedChecks)
            {
                if (checkGroup.Count() < 2)
                {
                    newChecks.AddRange(checkGroup);
                    continue;
                }

                var OpGroups = checkGroup.GroupBy(o => o.Operator);
                foreach (var opGroup in OpGroups)
                {
                    if (opGroup.Count() > 1)
                    {
                        if (opGroup.Key == '>')
                        {
                            newChecks.Add(new Check(checkGroup.Key, '>', opGroup.Max(o => o.Value)));
                        }
                        else
                        {
                            newChecks.Add(new Check(checkGroup.Key, '<', opGroup.Min(o => o.Value)));
                        }
                    }
                    else
                    {
                        newChecks.AddRange(opGroup);
                        continue;
                    }
                }
            }
   //         Console.WriteLine(String.Join(" && ", newChecks.Select(o => o.VariabelName.ToString() + o.Opperator + o.Value)));
            List<Check> newerChecks = new List<Check>();

            var VarGroups = newChecks.GroupBy(o => o.VariabelName);

            foreach (var checkGroup in VarGroups)
            {
                if (checkGroup.Count() < 2)
                {
                    newerChecks.AddRange(checkGroup);
                }
                else
                {
                    if (checkGroup.Count() > 2) throw new Exception();
                    int smallerAsVal = checkGroup.First(o => o.Operator == '<').Value;
                    int greaterAsVal = checkGroup.First(o => o.Operator == '>').Value;
                    if (smallerAsVal < greaterAsVal)
                    {
                        throw new Exception();
                    }
                    newerChecks.Add(new Check(checkGroup.Key, checkGroup.Min(o => o.Value), checkGroup.Max(o => o.Value)));
                }

            }

            Console.WriteLine(String.Join(" && ", newerChecks.Select(o => o.VariabelName.ToString() + (o.Value2.HasValue ? o.Value : "" )+ o.Operator + (o.Value2.HasValue ? o.Value2: o.Value))));
            checkList.Add(newerChecks);

        }

      
        long res = (long)Math.Pow(3999, 4);
        foreach(var checks in checkList)
        {
            long tempRes = (long)Math.Pow(3999, 4-checks.Count);

            foreach (var check in checks)
            {
                if(check.Operator == '>')
                {
                    tempRes *= 4000 - check.Value-1;
                }else if(check.Operator == '<') {
                    tempRes *= check.Value-1;
                }
                else
                {
                    tempRes *= check.Value2.Value - check.Value -1;
                }
            }
            res -= tempRes;
        }

        Console.WriteLine(res);

        Int128 res2 = GetCombs(start, new Intervals());
        Console.WriteLine(res2);

       
        /*
    do
    {
        string mString = strings[i++];
        if (start.IsAccepted(mString).Value)
        {
            int nums = Regex.Matches(mString,@"\d+").Sum(o => int.Parse(o.Value));
            counter += nums;
        }
    }while (i < strings.Length);

    Console.WriteLine(counter.ToString());
*/
    }

    static Int128 GetCombs(Rule start,Intervals intervals)
    {
        Int128 res = 0;
        Intervals localInvervals = (Intervals) intervals.Clone();
        foreach(var cond in start.conditions)
        {
            if (cond.AlwaysReject)
            {
                //res += localInvervals.Count;
                return res;
            }
            else if (cond.CanReject)
            {
                localInvervals.RemoveNumbers(cond.Check);
             //   res += GetCombs(cond.Child, intervals);
            }else if(cond.AlwaysAccept)
            {
                res += localInvervals.Count;
                return res;
            }else if (cond.CanAccepts)
            {
                res += localInvervals.RemoveNumbers(cond.Check);

            }
            else if (cond.AlwaysRule != null)
            {
                res += GetCombs(cond.AlwaysRule, localInvervals);
            }else
            {
                Intervals childIntervals = (Intervals)localInvervals.Clone();
                childIntervals.RemoveNumbers(cond.Check,true);
                res +=  GetCombs(cond.Child, childIntervals);
                localInvervals.RemoveNumbers(cond.Check);
            }
        }
        return res;
    }
}
public class Intervals : ICloneable
{
    public List<int> Xs { get; set; }
     public List<int> Ms { get; set; }
     public List<int> As { get; set; }
     public List<int> Ss { get; set; }
    
    public Int128 Count {get {  return (Int128)Xs.Count * (Int128)Ms.Count * (Int128)As.Count * (Int128)Ss.Count; } }
    public Intervals()
    {
        Xs = Enumerable.Range(1,4000).ToList();
        Ms = Enumerable.Range(1,4000).ToList() ;
        As = Enumerable.Range(1, 4000).ToList();
        Ss = Enumerable.Range(1, 4000).ToList();
    }
    public Int128 RemoveNumbers(Check check,bool not = false)
    {
        
        Int128 beginCount = Count;
        List<int> toBeRemovedFrom = new List<int>();
        if (check.VariabelName == 'x') toBeRemovedFrom = Xs;
        else if (check.VariabelName == 'm') toBeRemovedFrom = Ms;
        else if (check.VariabelName == 'a') toBeRemovedFrom = As;
        else toBeRemovedFrom = Ss;
        List<int> nums;
        if (check.Operator == '<')
        {
            if(!not) nums = toBeRemovedFrom.Where(o => o < check.Value).ToList();
            else nums = toBeRemovedFrom.Where(o => o > check.Value-1).ToList();

        }
        else
        {
            if (!not) nums = toBeRemovedFrom.Where(o => o > check.Value).ToList();
            else nums = toBeRemovedFrom.Where(o => o < check.Value+1).ToList();
        }
        if (nums.Count() > 0)
        {
            foreach (int i in nums)
            {
                if (toBeRemovedFrom.Contains(i)) toBeRemovedFrom.Remove(i);
            }
        }
        return beginCount - Count;
    }
    public object Clone()
    {
       return new Intervals { As = new List<int>(this.As), Ms = new List<int>(this.Ms),Ss = new List<int>(this.Ss), Xs = new List<int>(Xs)  };
    }
}
public class Check
{
   // public bool Not {  get; set; } = false;
    public char VariabelName { get; set; }
    public char Operator { get; set; }
    public int Value { get; set; }
    public int? Value2 { get; set; }

    public Check(char VariabelName, char Opperator, int Value)
    {
        this.VariabelName = VariabelName;
        this.Operator = Opperator;
        this.Value = Value;
    }

    public Check(char VariabelName, int Value,int Value2)
    {
        this.VariabelName = VariabelName;
        this.Operator = '-';
        this.Value = Value ;
        this.Value2 = Value2 ;
    }

    public void Inverse()
    {
        if(Operator == '<')
        {
            Value--;
            Operator = '>';
        }else if(Operator == '>')
        {
            Value++;
            Operator = '<';
        }else if(Operator == '-')
        {
            Operator = '|';
        }
        else
        {
            Operator = '-';
        }
    }
}
public class Rule
{
  public  string Name { get; set; }
    private string inputString;
  public Condition[] conditions { get; set; }

    public Rule Parent { get; set; }

    public List<Rule> Childs { get
        {
            List<Rule> list = new List<Rule>();
           foreach(var cond in conditions)
            {
                list.Add(cond.Child);
            }
           return list;
        } }

    public Rule(string input)
    {
        this.inputString = input;
        Name = inputString.Split('{')[0];
    }

    public void SetParent(List<Rule> rules)
    {
        if (Name == "in") return;

        var Parents =rules.Where(o => o.conditions.Any(o => o.Child?.Name == Name));
        if(Parents.Count() > 1)  throw new Exception(); 
        if(Parents.Count()  == 0) throw new Exception();
        
        Parent = Parents.First();
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
            conditions[i] = new Condition(condition, rules,this);
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
    public bool AlwaysReject { get { return AlwaysRejectNullable.HasValue &&  AlwaysRejectNullable.Value; } }
    public bool AlwaysAccept { get { return AlwaysRejectNullable.HasValue && !AlwaysRejectNullable.Value; } }
    public bool CanReject { get { return AlwaysReject || RejectedIFTrue.HasValue && RejectedIFTrue.Value; } }
    public bool CanAccepts { get { return AlwaysAccept || RejectedIFTrue.HasValue && !RejectedIFTrue.Value; } }
    public bool? AlwaysRejectNullable { get; set; } = null;
    public Rule AlwaysRule { get; set; } = null;
    public char VariabelName { get; set; }
    public string CheckString { get; set; }
    public bool? RejectedIFTrue { get; set; } = null;
    public Rule IfAccepted { get; set; }

    public Check Check { get
        {
            return new Check(VariabelName, CheckString[0], int.Parse(CheckString.Substring(1)));
        } }
    public Rule Parent { get; set; }
    public Rule Child { get
        {
            List<Rule> result = new List<Rule>();
            if(AlwaysRule != null) result.Add(AlwaysRule);
            if(IfAccepted != null) result.Add(IfAccepted);
            if (result.Count > 1) throw new Exception();
            return result.Count == 0 ? null : result.First();
        } }
    


    public Condition(string input,List<Rule> Rules,Rule Parent)
    {
        this.Parent = Parent;
        if (!input.Contains(':'))
        {
            if (input == "A") AlwaysRejectNullable = false;
            else if (input == "R") AlwaysRejectNullable = true;
            else AlwaysRule = Rules.First(o => o.Name == input);
        }
        else
        {
            VariabelName = input.First();
            CheckString = input.Substring(1).Split(':')[0];

            string IfYes = input.Split(':')[1];

            if (IfYes == "A") RejectedIFTrue = false;
            else if (IfYes == "R") RejectedIFTrue = true;
            else IfAccepted = Rules.First(o => o.Name == IfYes);
        }

    }

    public bool? Procces(string input)
    {
        if (AlwaysRejectNullable.HasValue && AlwaysRejectNullable.Value) return false;
        else if(AlwaysRejectNullable.HasValue && !AlwaysRejectNullable.Value) return true;
        else if(!AlwaysRejectNullable.HasValue && AlwaysRule != null) return AlwaysRule.IsAccepted(input);
        else
        {
            string constring = input.Substring(1,input.Length -2);
            string[] cons = constring.Split(",");
            string con = cons.First(o => o[0] == VariabelName);

            bool isTrue = false;
            if (CheckString[0] == '<')
            {
                isTrue = int.Parse(con.Substring(2)) < int.Parse(CheckString.Substring(1));
            }else if (CheckString[0] == '>')
            {
                isTrue = int.Parse(con.Substring(2))  > int.Parse(CheckString.Substring(1));
            }
            else
            {
                isTrue = int.Parse(con.Substring(2)) == int.Parse(CheckString.Substring(1));
            }
            if(!isTrue) return null;
            if(RejectedIFTrue.HasValue && RejectedIFTrue.Value) return false;
            else if(RejectedIFTrue.HasValue && !RejectedIFTrue.Value) return true;
            else return IfAccepted.IsAccepted(input);
        }
    }
}