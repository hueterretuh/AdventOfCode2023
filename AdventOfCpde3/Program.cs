using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string[] lines = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\3.txt");
        List<int[]> numinfos = new List<int[]>();
        List<int[]> starInfos = new List<int[]>();

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

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            MatchCollection matches = Regex.Matches(line, @"\*");
            foreach (Match match in matches)
            {
                int[] numInfo = new int[3];
                numInfo[0] = match.Index;

                numInfo[1] = match.Index ;
                numInfo[2] = i;
                starInfos.Add(numInfo);
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

        int result2 = 0;
        starInfos = starInfos.OrderBy(o => o[0]).OrderBy(o => o[2]).ToList();
        foreach(var starInfo in starInfos)
        {
            result2 += GetGears(starInfo, lines, numinfos);
        }
        Console.WriteLine(result2.ToString());
    }

    private static int GetGears(int[] starInfo, string[] lines, List<int[]> numInfos)
    { 
        int result = 0;
        int[][] neighbors = numInfos.Where(o => numIsNeighbor(starInfo, lines, o)).ToArray();
        if (neighbors.Count() != 2) return 0;
        return neighbors[0][3] * neighbors[1][3];

    }

    private static bool numIsNeighbor(int[] starInfo, string[] lines, int[] numInfo)
    {
        if (Math.Abs(numInfo[2] - starInfo[2]) > 1) return false;
        int starind = starInfo[0];
        int numbegin = numInfo[0] > 0 ? numInfo[0]-1 : 0;
        int numend = numInfo[1] < lines[0].Length-1 ? numInfo[1]+1 : lines[0].Length-1;

        int[] validStars = Enumerable.Range(numbegin,numend - numbegin +1).ToArray();
        if(validStars.Any(o => o == starind)) return true;
        return false;

        
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
                //Console.WriteLine(numinfo[3] + ": " +lines[lineNum]);
                return true;
            }
            
        }
        if (numinfo[1] +1  < lineLength) {
            if (isSymbol(lines[lineNum][end + 1]))
            {
                //Console.WriteLine(numinfo[3] + ": " + lines[lineNum]);
                return true;
            }
        }
        if(lineNum > 0)
        {
            for(int i = start > 0 ? start - 1 : 0; i <= end +1 && i < lineLength; i++)
            {
                if (isSymbol(lines[lineNum - 1][i]))
                {
                   // Console.WriteLine(numinfo[3] + ": " + lines[lineNum]);
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
                   // Console.WriteLine(numinfo[3] + ": " + lines[lineNum]);
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