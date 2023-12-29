using System.Diagnostics.Tracing;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

internal class Program
{
    static int[,] map;
    private static void Main(string[] args)
    {
        map = new int[500, 500];
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\18.txt");
        List<Instruction> instructions = new List<Instruction>();
        List<int[]> XY = new List<int[]>();

        
        foreach (string s in strings)
        {
            instructions.Add(new Instruction(s));
        }

        int X = 100;
        int Y = 400;
        map[X, Y] = 2;

        foreach (Instruction instruction in instructions)
        {

            
                for (int i = 0; i < instruction.Length; i++)
                {
                    if (instruction.direction == Instruction.Direction.Left) X--;
                    else if (instruction.direction == Instruction.Direction.Right) X++;
                    else if (instruction.direction == Instruction.Direction.Up) Y--;
                    else Y++;
                    XY.Add(new []{ X,Y});
                    map[X, Y] = 2;
        
                }
            
        }

        for (int j = 0; j < map.GetLength(1); j++)
        {

            for (int i = 0; i < map.GetLength(0); i++)
            {
                Console.Write(map[i, j]);
            }
            Console.WriteLine();
        }
        for (int j = 0; j < map.GetLength(1); j++)
        {

            for (int i = 0; i < map.GetLength(0); i++)
            {
                if ((map[i,j] != 2))
                {
                    List<int[]> path = new List<int[]>();
                    List<int[]> ToCheck = new List<int[]>();
                    Tuple<bool?,List<int[]> > res = CanGetOut(i,j, path,ToCheck);
                 
                    if (res.Item2 != null && res.Item2.Count > 0) ToCheck.AddRange(res.Item2);

                    while(res.Item1 == null && ToCheck.Count > 0)
                    {
                      res= CanGetOut(ToCheck.First()[0], ToCheck.First()[1], path,ToCheck);
                        ToCheck.Remove(ToCheck.First());
                        if (res.Item2 != null && res.Item2.Count > 0)
                        {
                            foreach (int[] newItem in res.Item2)
                            {
                             if(!ToCheck.Any(o => o[0] == newItem[0] && o[1] == newItem[1]))   ToCheck.Add(newItem);
                            }
                        }
                    }
                    if (!res.Item1.HasValue)
                    {
                        Console.WriteLine(i +";" + j);
                    }
                    bool canOut = res.Item1.HasValue ? res.Item1.Value : false ;

                    foreach (int[] xy in path)
                    {
                        map[i,j] = canOut ? 3 : 1;
                    }


                }
            }

        }

        for (int j = 0; j < map.GetLength(1); j++)
        {

            for (int i = 0; i < map.GetLength(0); i++)
            {
                Console.Write(map[i,j]);
              }
            Console.WriteLine();
        }
        

        Console.WriteLine();
        Console.WriteLine(XY.Min(o => o[0]) + ";" + XY.Max(o => o[0]));
        Console.WriteLine(XY.Min(o => o[1]) + ";" + XY.Max(o => o[1]));
        Console.WriteLine(map.Cast<int>().Count(o => o == 2 || o == 1).ToString());
        
    }


    private static Tuple<bool?, List<int[]>>CanGetOut(int x, int y, List<int[]> path, List<int[]> ToCheck)
    {
      //  if(ToCheck.Any(o => o[0] == x && o[1] == y)) return new Tuple<bool?, List<int[]>> ( null, null );
        path.Add(new[] { x, y });
        if (x <= 0 || y <= 0 || x >= map.GetLength(0) - 1 || y >= map.GetLength(1)-1) return new Tuple<bool?, List<int[]>>(true,null);
        if (map[x - 1, y] == 1 || map[x, y - 1] == 1 || map[x + 1, y] == 1 || map[x, y + 1] == 1) return new Tuple<bool?, List<int[]>>(false, null);
        if (map[x - 1, y] == 3 || map[x, y - 1] == 3 || map[x + 1, y] == 3 || map[x, y + 1] == 3) return new Tuple<bool?, List<int[]>>(true, null);

        List<int[]> toCheck = new List<int[]>();
        if (map[x-1,y] != 2 && !path.Any(o => o[0] == x-1 && o[1] == y)) toCheck.Add(new[] { x - 1, y });
        if (map[x +1, y] != 2 && !path.Any(o => o[0] == x + 1 && o[1] == y)) toCheck.Add(new[] { x + 1, y });
        if (map[x , y-1] != 2 && !path.Any(o => o[0] == x  && o[1] == y -1)) toCheck.Add(new[] { x, y - 1 });
        if (map[x, y + 1] != 2 && !path.Any(o => o[0] == x  && o[1] == y+1)) toCheck.Add(new[] { x, y + 1 });
        return new Tuple<bool?, List<int[]>>(null,toCheck);
    }
}

    
public class Instruction
{
   public Direction direction {  get; set; }
    public int Length { get; set; }
    public int Color { get; set; }

    public Instruction(string input)
    {
        var inp = input.Split(' ');
        if (inp[0] == "D") direction = Direction.Down;
        else if (inp[0] == "L") direction = Direction.Left;
        else if (inp[0] == "R") direction = Direction.Right;
        else direction = Direction.Up;

        Length = int.Parse(inp[1]);

        Color = Convert.ToInt32(inp[2].Replace("#", "0x").Replace("(","").Replace(")",""),16);
    }
    public Instruction(Direction direction, int length, int color)
    {
        this.direction = direction;
        Length = length;
        Color = color;
    }

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down

    }
}