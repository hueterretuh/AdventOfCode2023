using System;
using System.Collections.Concurrent;
using System.Diagnostics;

internal class Program
{
    static ConcurrentDictionary<string, long> cache = new ConcurrentDictionary<string, long>();
    static void Main()
    {
        
        long total = 0;
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\12.txt");

       foreach(string s in strings)//  Parallel.ForEach(strings, s => 

        {
            string[] parts = s.Split();
            string cfg = String.Join('?', Enumerable.Repeat(parts[0], 5));
            int[] nums = Array.ConvertAll(String.Join(',', Enumerable.Repeat(parts[1], 5)).Split(','), int.Parse);
            long tempCount = Count(cfg, nums);
            total += tempCount;
            Console.WriteLine(cfg.Count(o => o == '?') +":" + tempCount);
        }//);

        Console.WriteLine(total);
    }

    static long Count(string cfg, int[] nums)
    {
        string key = cfg +";"+ string.Join(",", nums);
        bool has =  cache.TryGetValue(key, out long value);
        if (has) return value;

        if (cfg == "")
        {
            return nums.Length == 0 ? 1 : 0;
        }
        if (nums.Length == 0)
        {
            return cfg.Contains('#') ? 0 : 1;
        }
        long result = 0;
        if (cfg[0] == '.' || cfg[0] == '?')
        {
            result += Count(cfg.Substring(1), nums);
        }
        if (cfg[0] == '#' || cfg[0] == '?')
        {
            if (nums[0] <= cfg.Length && !cfg.Substring(0, nums[0]).Contains('.') && (nums[0] == cfg.Length || cfg[nums[0]] != '#'))
            {
                if (cfg.Length > nums[0]) result += Count(cfg.Substring(nums[0] + 1), nums[1..]);
                else result += Count("", nums[1..]);
            }
        }
        bool asb = cache.TryAdd(key, result);
        if (!asb && cache[key] != result)
        {
            Console.WriteLine(key + ";" + result + ";" + cache[key]);
        }
        return result;
    }
}
/*
 static void Main2(string[] args)
{
    string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\12.txt");
    List<Line> lines = new List<Line>();

    foreach (string s in strings)
    {
        lines.Add(new Line(s));
    }

    long result = 0;
    foreach (Line line in lines) 
    {
        Console.WriteLine("Count: " + line.stati.Count(o => o == Status.UnKnown));
        Stopwatch stopwatch = Stopwatch.StartNew();
        long tres= line.GetCombs();
        result += tres;
        Console.WriteLine(stopwatch.ElapsedMilliseconds + "-"+ tres);
    }
    Console.WriteLine(result);
}



public enum Status
{
    OK,
    Damged,
    UnKnown,
    UnKownAsDamag
}

public class Line
{
    public Status[] stati { get; set; }
    public int[] groups { get; set; }

    public Line(string line)
    {
        var inp = line.Split(' ');
        inp[0] = String.Join('?',Enumerable.Repeat(inp[0] , 5));
        inp[1] = String.Join(',', Enumerable.Repeat(inp[1], 5));
        stati = new Status[inp[0].Length];
        for (int i = 0; i < inp[0].Length; i++)
        {
            char c = inp[0][i];
            if (c == '.') stati[i] = Status.OK;
            else if (c == '#') stati[i] = Status.Damged;
            else stati[i] = Status.UnKnown;
        }

        groups = inp[1].Split(",").Select(o => int.Parse(o)).ToArray();

    }

    public long GetCombs()
    {
        long fitCounter = 0;
        List<int> undefs = new List<int>();

        for (int i = 0; i < stati.Length; i++)
        {
            if (stati[i] == Status.UnKnown) undefs.Add(i);
        }
        long counTO = (long)1 << undefs.Count;
        long info = 0;

        Parallel.For(0, counTO, i =>//for (UInt128 i = 0; i < (UInt128)1 << undefs.Count; i++)
        {

            //    List<Status> debug = new List<Status>();
            int ind = 0;
            int counter;
            Status LastStatus = stati[0];
            int UnKnCounter = 0;

            if (LastStatus == Status.UnKnown)
            {
                if ((i & ((long)1)) == 0)
                {
                    counter = 0;
                    LastStatus = Status.OK;
                }
                else
                {
                    counter = 1;
                    LastStatus = Status.UnKownAsDamag;
                }
                UnKnCounter++;
            }
            else if (LastStatus == Status.OK) counter = 0;
            else counter = 1;
            Status NextStatus = Status.OK;



     //       debug.Add(LastStatus);
            for (int j = 1; j < stati.Length; j++)
            {
                NextStatus = stati[j];

                if (NextStatus == Status.UnKnown)
                {
                    if ((i & ((long)1) << UnKnCounter) >> UnKnCounter == 0) NextStatus = Status.OK;
                    else NextStatus = Status.UnKownAsDamag;
                    UnKnCounter++;
                }

        //        debug.Add(NextStatus);
                if (NextStatus == Status.OK && LastStatus != Status.OK)
                {
                    if (ind >= groups.Length)
                    {
                        return ;
                    }
                    if (groups[ind] != counter)
                    {
                        return;
                    }
                    ind++;

                    counter = 0;
                }
                else if (NextStatus != Status.OK)
                {
                    counter++;
                }
                LastStatus = NextStatus;
            }

            if (ind == groups.Length - 1 && NextStatus != Status.OK)
            {
                if (groups.Last() != counter)
                {
                    return;
                }
                else
                {
                    ind++;
                }
            }

            if (ind != groups.Length)
            {
                return;
            }


            //  Debug.WriteLine((float) i / (long)counTO);

                Interlocked.Increment(ref fitCounter);


        });
        return fitCounter;
    }

    private static bool Fits(Status[] stati, int[] groups)
    {
        int ind = 0;

        int counter = stati[0] != Status.OK ? 1 : 0;
        for (int i = 1; i < stati.Length; i++)
        {

            if ((stati[i] == Status.OK) && (stati[i - 1] != Status.OK))
            {
                if (ind >= groups.Length)
                {
                    return false;
                }
                if (groups[ind] != counter) return false;
                ind++;

                counter = 0;
            }
            else if(stati[i] != Status.OK)
            {
                counter++;
            }
        }

        if (ind == groups.Length -1 &&stati.Last() != Status.OK)
        {
            if( groups.Last() != counter) return false;
            else
            {
                ind++;
            }
        } 

        if (ind != groups.Length) return false;
        return true;

    }
    */
/*
        public long GetCombs()
{
    long fitCounter = 0;
    List<int> undefs = new List<int>();

    for (int i = 0; i < stati.Length; i++)
    {
        if (stati[i] == Status.UnKnown) undefs.Add(i);
    }
    long counTO = (long)1 << undefs.Count;
    Parallel.For(0, counTO, i =>//for (UInt128 i = 0; i < (UInt128)1 << undefs.Count; i++)
    {
        Status[] statAr = new Status[stati.Length];
        stati.CopyTo(statAr, 0);
        for (int j = 0; j < undefs.Count; j++)
        {

            if ((i & ((long)1) << j) >> j == 0) statAr[undefs[j]] = Status.OK;
            else statAr[undefs[j]] = Status.UnKownAsDamag;

        }
      //  Debug.WriteLine((float) i / (long)counTO);
        if (Fits(statAr, groups))
        {
            Interlocked.Increment(ref fitCounter);
        }

    });
    return fitCounter;
}

private static bool Fits(Status[] stati, int[] groups)
{
    int ind = 0;

    int counter = stati[0] != Status.OK ? 1 : 0;
    for (int i = 1; i < stati.Length; i++)
    {

        if ((stati[i] == Status.OK) && (stati[i - 1] != Status.OK))
        {
            if (ind >= groups.Length)
            {
                return false;
            }
            if (groups[ind] != counter) return false;
            ind++;

            counter = 0;
        }
        else if(stati[i] != Status.OK)
        {
            counter++;
        }
    }

    if (ind == groups.Length -1 &&stati.Last() != Status.OK)
    {
        if( groups.Last() != counter) return false;
        else
        {
            ind++;
        }
    } 

    if (ind != groups.Length) return false;
    return true;

}

}
}       */