internal class Program
{
    private static void Main(string[] args)
    {
        string input = File.ReadAllText(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\15.txt");


        string[] vals = input.Split(',');

        long res = 0;
        foreach (var item in vals)
        {
            res += hash(item);
        }
        Console.WriteLine(res);
    }

    private static int hash(string s)
    {
        int res = 0;

        foreach(char c in s)
        {
            res += c;
            res *= 17;
              res  %= 256;
        }
        return res;
    }
}