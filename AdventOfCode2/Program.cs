using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\2.txt");
        int counter = 0;
        foreach (string s in strings)
        {
            bool possible = true;
            string game = Regex.Match(s, @"Game \d+:").Value;
            string vals = s.Replace(game, " ");
            string[] valAr = vals.Split(";");

            int minBlue = 0;
            int minRed = 0;
            int minGreen = 0;

            foreach(string set in valAr)
            {
                int blue = getNum("blue",set);
                int red = getNum("red", set);
                int green = getNum("green", set);

               
                if(blue > minBlue) {
                minBlue = blue;
                }
                if (green > minGreen)
                {
                    minGreen = green;
                }
                if (red > minRed)
                {
                    minRed = red;
                }

            }
            counter += minRed * minBlue * minGreen;
        }
        Console.WriteLine(counter);
    }

    private static int getNum(string color,string set)

    {
        bool succes = int.TryParse(Regex.Match(Regex.Match(set, @"\d+ " + color).Value, @"\d+").Value, out int res);
        return succes ? res : 0;
    }
}