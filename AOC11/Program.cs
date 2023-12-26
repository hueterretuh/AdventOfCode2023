using System.Xml.Schema;

internal class Program
{
    private static void Main(string[] args)
    {
        List<string> input = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\11.txt").ToList();
        List<string> stringList = new List<string>();
        stringList.AddRange(input);


        int inserted = 0;
        for (int i = 0; i < input.Count; i++)
        {
            string? item = input[i];
            if (!item.Contains('#')){
                stringList.Insert(i + inserted++ , new string('.', item.Length));
            }
        }
        inserted = 0;
        for(int i = 0;i < input[0].Length ;i++)
        {
            if(input.All(o => o[i] == '.')){
                for (int i1 = 0; i1 < stringList.Count; i1++)
                {

                    stringList[i1] = stringList[i1].Insert(i + inserted, ".");
                }
                inserted++;
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
                result += Math.Abs( stars[i][1] - stars[j][1]) + Math.Abs(stars[i][2] - stars[j][2]);

          //      Console.WriteLine(i + "," + j);
            }
        }
        Console.WriteLine(result.ToString());
    }
}