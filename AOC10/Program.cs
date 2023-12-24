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

        for(int y = 0; y < strings.Length; y++)
        {
            for (int x = 0; x < strings[y].Length; x++)
            {
                Tile tile = tileMap[x, y];
                if(tile != null)
                {
                    if (tile.HasTop && y == 0) tile = null;
                    else if (tile.HasBottom && y == tileMap.GetLength(1) - 1) tile =null;
                    else if (tile.HasLeft && x == 0) tile = null;
                    else if (tile.HasRight && x == tileMap.GetLength(0) - 1) tile = null;
                    else if (tile.HasRight && (tileMap[x + 1, y] == null || !tileMap[x + 1, y].HasLeft)) tile = null;
                    else if (tile.HasLeft && (tileMap[x-1,y] == null || !tileMap[x-1, y].HasRight)) tile = null;
                    else if (tile.HasTop && (tileMap[x,y-1] == null || !tileMap[x,y-1].HasBottom )) tile = null;
                    else if(tile.HasBottom && (tileMap[x,y+1] == null || !tileMap[x,y+1].HasTop)) tile = null;
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

    public bool HasBottom { get { return symb == '|' || symb == 'S' || symb == '7' || symb == 'F'; } }
    public bool HasLeft { get { return symb == 'S' || symb == '7' || symb == '-' || symb == 'J'; } }

    public bool HasRight { get { return symb == 'S' || symb == '-' || symb == 'L' || symb == 'F'; } }


}
