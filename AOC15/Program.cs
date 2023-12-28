internal class Program
{
    private static void Main(string[] args)
    {
        string input = File.ReadAllText(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\15.txt");
        string[] vals = input.Split(',');

        List<Lens>[] map = new List<Lens>[256];

        for (int i = 0; i < map.Length; i++)
        {
            map[i] = new List<Lens>();
        }
       

        foreach (var item in vals)
        {
            string label;
            if (item.Contains('-'))
            {
                label = item.Replace("-", "");
                int ind = hash(label);
                map[ind].RemoveAll(o => o.Label == label);
            }
            else
            {
                label = item.Split('=')[0];
                int num = int.Parse(item.Split('=')[1]);
                int ind = hash(label);
                if (map[ind].Any(o => o.Label == label)) map[ind].First(o => o.Label == label).Power = num;
                else map[ind].Add(new Lens(label, num));
            }
        }
        long res = 0;
        for (int i = 0; i < map.Length; i++) { 
            for(int j = 0;j < map[i].Count;j++)
            {
                res += (i + 1) * (j + 1) * map[i][j].Power;
            }
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

public class Lens
{
    public string Label { get; set; }
    public int Power { get; set; }

    public Lens(string label, int power)
    {
        Label = label;
        Power = power;
    }
}