using Microsoft.VisualBasic;
using System.Numerics;

internal class Program
{
    public static int[,] input;

    public static Dictionary<string,Search> cache = new Dictionary<string,Search>();
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\17.txt");
        input = new int[strings[0].Length, strings.Length];


        for (int i = 0; i < strings.Length; i++)
        {
            for (int j = 0; j < strings[i].Length; j++)
            {
                input[j,i] = int.Parse( strings[i][j].ToString());
            }
        }

        SortedSet<Search> sorted = new SortedSet<Search>(new SearchCompare());
        sorted.Add(new Search(0, 0, 0, 0,Search.Direction.None, input,new List<Search> ()));
        IEnumerable<Search> reses;
        do
        {
            
            reses = GetNext(sorted.First());
            sorted.Remove(sorted.First());
            
            if (reses == null) continue;
            foreach (Search s in reses)
            {
              bool inserted =  sorted.Add((Search)s);
                if (!inserted)
                {
                    Console.WriteLine();
                }
            }
            if(sorted.Count() <=5)
            {
                Console.WriteLine("WTF");
            }
        }while(reses == null || !reses.Any(o => o.x == input.GetLength(0)-1 && o.y == input.GetLength(1)-1));

        Console.WriteLine(sorted.First(o => o.x == input.GetLength(0) - 1 && o.y == input.GetLength(1) - 1).cost);
    }

    private static List<Search> GetNext(Search search)
    {
        Console.WriteLine(search.x + ";" + search.y);
        string key = search.Key;
        bool exists =  cache.TryGetValue(key, out Search cacheVal);
        if (exists)
        {
            if(cacheVal.cost > search.cost)
            {
                cache.Remove(key);
                cache.Add(key, search);
            }
            else
            {
                return null;
            }
        }
        else
        {
            cache.Add(key, search);
        }

        List<Search> list = new List<Search>();

        Search search1 = search.GetRight();
        if(search1 != null) list.Add( search1);
        Search search2 = search.GetLeft();
        if (search2 != null) list.Add( search2);
        Search search3 = search.GetDown();
        if (search3 != null) list.Add( search3);
        Search search4 = search.GetUP();
        if (search4 != null) list.Add( search4);
        return list;
        
    }

 
    
}

public class SearchCompare : IComparer<Search>
{
    public int Compare(Search? x, Search? y)
    {
        return x.CompareTo(y);
    }
}
public class Search : IComparable<Search> 
{
    public int[,] input { get; set; }
    Direction direction { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public int line { get; set; }
    public int cost { get; set; }
    public string Key { get { return x + "," + y + "," + line + "," + direction; } }
    public List<Search> Path = new List<Search>();

    public Search(int x, int y, int line, int cost,Direction direction, int[,] input,List<Search> Path)
    {
        this.x = x;
        this.y = y;
        this.line = line;
        this.cost = cost;
        this.input = input;
        this.Path = CopyList(Path);
        this.Path.Add(this);
        this.direction = direction;
    }

    public Search GetLeft()
    {
        if(direction == Direction.Right) return null;
        if (x == 0) return null;
        if (direction == Direction.Left && line == 3) return null;
       
        Search NewSearch =  new Search(x - 1, y, direction == Direction.Left ? line + 1 : 1, cost + input[x - 1, y],Direction.Left, input, Path);
        if(!Path.Any(o => o.x == NewSearch.x && o.y == NewSearch.y)) return NewSearch;
        return null;
    }

    public Search GetRight()
    {
        if (direction == Direction.Left) return null;
        if (x == input.GetLength(0) -1) return null;
        if (direction == Direction.Right && line == 3) return null;
        Search NewSearch = new Search(x + 1, y, direction == Direction.Right ? line + 1 : 1, cost + input[x + 1, y],Direction.Right, input, Path);
        if (!Path.Any(o => o.x == NewSearch.x && o.y == NewSearch.y)) return NewSearch;
        return null;
    }

    public Search GetUP()
    {
        if (direction == Direction.Down) return null;
        if (y == 0) return null;
        if (direction == Direction.Up && line == 3) return null;
        Search NewSearch = new Search(x , y-1, direction == Direction.Up ? line + 1 : 1, cost + input[x , y-1],Direction.Up, input,Path);
        if (!Path.Any(o => o.x == NewSearch.x && o.y == NewSearch.y)) return NewSearch;
        return null;
    }

    public Search GetDown()
    {
        if (direction == Direction.Up) return null;
        if (y == input.GetLength(1)-1) return null;
        if (direction == Direction.Down && line == 3) return null;
        Search NewSearch =  new Search(x, y + 1, direction == Direction.Down ? line + 1 : 1, cost + input[x, y + 1],Direction.Down, input, Path);
        if (!Path.Any(o => o.x == NewSearch.x && o.y == NewSearch.y)) return NewSearch;
        return null;
    }

    public int CompareTo(Search? other)
    {
        if (other == null) return - 1;
        else if (other == this) return 0;
        else if (other.cost < cost) return 1;
        else if(other.cost > cost) return -1;
        else
        {
            if (Math.Pow(other.x * other.x + other.y * other.y, 0.5) < Math.Pow(x * x + y * y, 0.5)) return 1;
            else return -1;
        }

    }

    private List<Search> CopyList(List<Search> Paths)
    {
        List<Search> list = new List<Search>();
        foreach (Search search in Paths)
        { 
        list.Add(search);
        }
        return list;
    }
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        None
    }
}