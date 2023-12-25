internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\10.txt");
        Tile[,] tileMap = new Tile[strings[0].Length, strings.Length];

        for (int y = 0; y < strings.Length; y++)
        {
            for (int x = 0; x < strings[y].Length; x++)
            {
                char c = strings[y][x];
                if (c != '.')
                {
                    tileMap[x, y] = new Tile(x, y, c);

                }
            }

        }
        int nothingCounter;// = tileMap.Cast<Tile>().Count(o => o == null);
        do
        {
            nothingCounter = tileMap.Cast<Tile>().Count(o => o == null);

            for (int y = 0; y < strings.Length; y++)
            {
                for (int x = 0; x < strings[y].Length; x++)
                {
                    Tile tile = tileMap[x, y];
                    if (tile != null && tile.symb != 'S')
                    {

                        if (tile.HasTop && y == 0) tileMap[x, y] = null;
                        else if (tile.HasBottom && y == tileMap.GetLength(1) - 1) tileMap[x, y] = null;
                        else if (tile.HasLeft && x == 0) tileMap[x, y] = null;
                        else if (tile.HasRight && x == tileMap.GetLength(0) - 1) tileMap[x, y] = null;
                        else if (tile.HasRight && (tileMap[x + 1, y] == null || !tileMap[x + 1, y].HasLeft)) tileMap[x, y] = null;
                        else if (tile.HasLeft && (tileMap[x - 1, y] == null || !tileMap[x - 1, y].HasRight)) tileMap[x, y] = null;
                        else if (tile.HasTop && (tileMap[x, y - 1] == null || !tileMap[x, y - 1].HasBottom)) tileMap[x, y] = null;
                        else if (tile.HasBottom && (tileMap[x, y + 1] == null || !tileMap[x, y + 1].HasTop)) tileMap[x, y] = null;

                    }
                }
            }
            Console.WriteLine(nothingCounter.ToString());

        } while (tileMap.Cast<Tile>().Count(o => o == null) != nothingCounter);

        for (int y = 0; y < strings.Length; y++)
        {
            string line = "";
            for (int x = 0; x < strings[y].Length; x++)
            {
                Tile tile = tileMap[x, y];
                if (tile != null)
                {
                    line += tile.symb;
                }
                else
                {
                    line += " ";
                }
            }
            Console.WriteLine(line);
        }

        Parallel.ForEach<Tile>(tileMap.Cast<Tile>(), tile =>
        {
            if (tile != null)
            {
                tile.SetConnections(tileMap);
            }
        });
        Console.WriteLine(tileMap.Cast<Tile>().Where(o => o != null && o.Connections.Count != 2).Count() + "/" + tileMap.Cast<Tile>().Count(o => o != null));
        
        Tile start = tileMap.Cast<Tile>().First(o => o?.symb == 'S');

        Tile temp = start;
        Tile prev = start.Connections.First();
        Tile next = start.Connections.First();
        int counter = 1;
        do
        {
            temp = next;
            next = next.GetNext(prev);
            prev = temp;
            counter++;
        } while (next.symb != 'S');
    
        Console.WriteLine(counter/2);


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


    public void SetConnections(Tile[,] tiles)
    {
        var neighbors = tiles.Cast<Tile>().Where(o => o != null  && (Math.Abs(o.x - x)  +Math.Abs( o.y - y) ) == 1);
       Connections = neighbors.Where(o =>

            HasRight && o.HasLeft && o.x == x+1||
            HasLeft && o.HasRight && o.x == x-1||
            HasTop && o.HasBottom && o.y == y-1||
            HasBottom && o.HasTop && o.y == y+1
        ).ToList();
    }

    public Tile GetNext(Tile from)
    {
        return Connections.First(o => o != from);
    }
}
