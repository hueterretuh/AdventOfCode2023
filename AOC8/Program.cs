using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\8.txt");
        bool[] leftRight;
        string input = string.Join(string.Empty, strings.Take(2));
        List<bool?> lrList = new List<bool?>();
        foreach(char c in input)
        {
            lrList.Add(getLeftRightNone(c));
        }
        leftRight = lrList.Where(o => o.HasValue).Select(o => o.Value).ToArray();

        Dictionary<string,LeftRight> leftRightDic = new Dictionary<string,LeftRight>();

        foreach(string line in strings.Skip(2))
        {
            leftRightDic.Add(line.Substring(0,3),new LeftRight(line.Substring(7,3),line.Substring(12,3)));
        }

        string node = "AAA";
        int i = 0;
        int counter = 0;
        do
        {
            node = leftRightDic[node].getNode(leftRight[i]);

            counter++;
            if (i == leftRight.Length - 1) i = 0;
            else i++;
        } while (node != "ZZZ");

        Console.WriteLine(counter );
    }


    private static bool? getLeftRightNone(char c)
    {
        if (c == 'L') return true;
        else if (c == 'R') return false;
        else return null;
    }
}

internal class LeftRight
{
    public string Left { get; set; }
    public string Right { get; set; }
    public LeftRight(string left, string right) {
    
    this.Left = left;
    this.Right = right;
    }

    public string getNode(bool left)
    {
        return left ? Left : Right;
    }
}

