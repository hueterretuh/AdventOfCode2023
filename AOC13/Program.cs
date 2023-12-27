using Microsoft.VisualBasic;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\13.txt");

        List<string[]> inputsNorm = new List<string[]>();
        List<string[]> inputsRot = new List<string[]>();

        List<string> inputs = new List<string>();
        foreach (string s in strings)
        {

            if (String.IsNullOrWhiteSpace(s))
            {
                AddInput(inputs,inputsNorm,inputsRot);
                Print(inputsNorm.Last());
               Print(inputsRot.Last());
                inputs = new List<string>();
            }
            else
            {
                inputs.Add(s);
            }
        }
        AddInput(inputs, inputsNorm, inputsRot);
        Print(inputsNorm.Last());
        Print(inputsRot.Last());

        long res = 0;
        for(int i = 0; i < inputsNorm.Count; i++)
        {
            long ressave = res;
            res += getRes(inputsNorm[i]) * 100;
            res += getRes(inputsRot[i]);
            if(res == ressave)
            {
                Console.WriteLine("asdf");
            }
        }

        Console.WriteLine(res);
    }

    private static long getRes(string[] strings)
    {
        long res = 0;
        for (int i = 1; i < strings.Length; i++)
        {

            
                int DifCount = GetDifCount(strings[i - 1], strings[i]);
                if(DifCount == 0 || DifCount == 1)
                {
                    if (CheckRemaining(strings, i, DifCount)) return i;
                }
                
            
        }
        return 0;
    }

    private static int GetDifCount(string s1, string s2) {
 
        int diff = 0;
        for (int i = 0;i < s1.Length;i++) {

            if (s1[i] != s2[i]) diff++;
        }

        return diff;
    }
       
    private static bool CheckRemaining(string[] strings, int i,int startdif)
    {
        int diff = startdif;
        for (int j = i+1; j < strings.Length && 2 * i - j - 1 >= 0; j++)
        {
            diff += GetDifCount(strings[2 * i - j - 1], strings[j]);
            if (diff > 1) return false;
        
        }

        if (diff == 1) return true;
        return false;
    }

    private static bool CheckRemaining(string[] strings, int i)
    {
        int diff = 0;
        for (int j = i; j < strings.Length && 2 * i - j - 1 >= 0; j++)
        {
            if((strings[2 * i - j - 1] !=strings[j])) return false;



        }

        return true;
    }
    private static void Print(string[] args)
    {
        Console.WriteLine("\n\n");
        foreach (string s in args)
        {
            Console.WriteLine(s);
        }
    }
    private static void AddInput(List<string> inputs, List<string[]> inputsNorm,List<string[]> inputsRot)
    {
        inputsNorm.Add(inputs.ToArray());
        string[] rot = new string[inputs[0].Length];

        for (int i = 0; i < inputs.Count; i++)
        {
            string inp = inputs[i];
            for (int j = 0; j < inp.Length; j++)
            {
                char c = inp[j];

                rot[j] += c;
            }
        }
        inputsRot.Add(rot);
    }
}