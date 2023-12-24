internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\9.txt");
        Tile[,] tileMap = new Tile[strings[0].Length,strings.Length];

        for (int y = 0; y < strings.Length; y++)
        {
            for(int x = 0; x < strings[y].Length; x++)
            {
                char c = strings[y][x];
                if(c != '.')
                {
                    tileMap[x, y] = new Tile(x, y,c);

                }
            }
            
        }
    }


}

public class Tile
{
    public Tile(int x, int y, char symb)
    {
        this.x = x;
        this.y = y;
        Connections = new List<Tile>();
        this.symb = symb;
    }

    public int x { get; set; }
    public int y { get; set; }

    public char symb { get; set; }
    public List<Tile> Connections { get; set; }

    public bool HasTop { get { return symb == '|' || symb == 'L' || symb == 'J' || symb == 'S'; } }


}
