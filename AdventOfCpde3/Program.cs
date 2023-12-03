using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string[] lines = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\3.txt");
        List<int[]> numinfos = new List<int[]>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            MatchCollection matches = Regex.Matches(line, @"\d+");
            foreach(Match match in matches) {
                int[] numInfo = new int[4];
                numInfo[0] = match.Index;
                
                numInfo[1] = match.Index + match.Length-1;
                numInfo[2] = i;
                numInfo[3] = int.Parse( match.Value);
                numinfos.Add(numInfo);
            }

        }
        numinfos = numinfos.OrderBy(o => o[0]).OrderBy(o => o[2]).ToList();
        int result = 0;
        foreach(var numinfo in numinfos) {
            if(HasSymbolNeighbor(numinfo,lines))
            {
                result += numinfo[3];
            }
        }

        Console.WriteLine(result.ToString());
    }

    private static bool HasSymbolNeighbor(int[] numinfo, string[] lines) {

        int start = numinfo[0];
        int end = numinfo[1];
        int lineNum = numinfo[2];
        int lineLength = lines[lineNum].Length;

        bool hasSymbolNeighbor = false;
        if (numinfo[0] > 0)
        {
            if (isSymbol(lines[lineNum][start - 1]))
            {
                Console.WriteLine(numinfo[3] + ": " +lines[lineNum]);
                return true;
            }
            
        }
        if (numinfo[1] +1  < lineLength) {
            if (isSymbol(lines[lineNum][end + 1]))
            {
                Console.WriteLine(numinfo[3] + ": " + lines[lineNum]);
                return true;
            }
        }
        if(lineNum > 0)
        {
            for(int i = start > 0 ? start - 1 : 0; i <= end +1 && i < lineLength; i++)
            {
                if (isSymbol(lines[lineNum - 1][i]))
                {
                    Console.WriteLine(numinfo[3] + ": " + lines[lineNum]);
                    return true;
                }
            }
        }
        if (lineNum < lines.Length-1)
        {
            for (int i = start > 0 ? start - 1 : 0; i <= end + 1 && i < lineLength; i++)
            {
                if (isSymbol(lines[lineNum + 1][i]))
                {
                    Console.WriteLine(numinfo[3] + ": " + lines[lineNum]);
                    return true;
                }
            }
        }
        return false;
    }

    private static bool isSymbol(char symbol)
    {
       return symbol != '.' && !char.IsNumber(symbol);
    }
}