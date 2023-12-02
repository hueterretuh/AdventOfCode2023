using System.Diagnostics;
using System.Text.RegularExpressions;

internal class Program
{
    private static string[] numStrings = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
    private static void Main(string[] args)
    {

        //Console.Write(getSum("248twofbkfpxtheightwovng"));


        Stopwatch sw = Stopwatch.StartNew();
       string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\AdventOfCode1.txt");
        int result = 0;
        foreach (string s in strings)
        {
            result += getSum(s);
        }

        Console.WriteLine(result);
        Console.WriteLine(sw.ElapsedMilliseconds);
        
    }

    private static int getSum(string line) { 

        int highestIndexd = -1;
        int lowestIned = int.MaxValue ;
        char first = ' ';
        char last = ' ';
        
        for(int i = 0;i < numStrings.Length;i++) 
          
        {
            string s = numStrings[i];
            int firstIned = line.IndexOf(s);
            int lastIndex = line.LastIndexOf(s);
            if (firstIned != -1)
            {
                if (firstIned < lowestIned)
                {
                    lowestIned = firstIned;
                    first = (i+1).ToString()[0];
                    Debug.Print(first.ToString());
                }
                if (lastIndex > highestIndexd)
                {
                    highestIndexd = lastIndex;
                    last = (i + 1).ToString()[0];
                    Debug.Print(last.ToString());
                }
            }

        }

        for(int i = 0;i < lowestIned;i++)
        {

            char c = line[i];
            if (char.IsDigit(c))
            {
                first = c;
                break;
            }
        }
        for (int i = line.Length-1; i > highestIndexd; i--) 
        {
            char c = line[i];
            if (char.IsDigit(c))
            {
                last = c;
                break;
            }
        }

        return int.Parse((first.ToString() + last));
    }
}