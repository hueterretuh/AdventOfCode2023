using System.Xml.Schema;

internal class Program
{
    private static void Main(string[] args)
    {
        List<string> input = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\11.txt").ToList();
        List<string> stringList = new List<string>();
        stringList.AddRange(input);

        List<int> emptyCols = new List<int>();
        List<int> emptyRows = new List<int>();

      //  int inserted = 0;
        for (int i = 0; i < input.Count; i++)
        {
            string? item = input[i];
            if (!item.Contains('#')){
               emptyRows.Add(i);
            }
        }
     //   inserted = 0;
        for(int i = 0;i < input[0].Length ;i++)
        {
            if(input.All(o => o[i] == '.')){
               emptyCols.Add(i);
            }
        }

        string[] strings = stringList.ToArray();
        int?[,] map = new int?[strings[0].Length, strings.Length];

        List<int[]> stars = new List<int[]>();
        int counter = 0;
        for(int y = 0;y < strings.Length; y++)
        {
            for(int x = 0;x < strings[y].Length;x++) {

                if (strings[y][x] == '.') map[x,y] = null;
                else
                {
                    map[x, y] = counter;
                    stars.Add(new [] { counter ,x,y});
                    counter++;
                }
            }
        }
        long result = 0;
        for(int i = 0; i < stars.Count; i++)
        {

            for(int j = i +1; j< stars.Count; j++)
            {
                int[] xs = { stars[i][1], stars[j][1] };
                int[] ys = { stars[i][2], stars[j][2] };
                int x1 = xs.Min();
                int x2 = xs.Max();
                int y1 = ys.Min();
                int y2 = ys.Max();

                long offset = 0;
                if (x1 != x2) offset += emptyCols.Where(x => x > x1 && x < x2).Count() * 999999;
                if (y1 != y2) offset += emptyRows.Where(y => y > y1 && y < y2).Count() * 999999;
                result += Math.Abs( x1 - x2) + Math.Abs(y1 - y2);
                result += offset;
          //      Console.WriteLine(i + "," + j);
            }
        }
        Console.WriteLine(result.ToString());
    }
}