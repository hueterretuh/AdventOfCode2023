using System.Diagnostics.Tracing;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

internal class Program
{
    private static bool ClockWise = false;
    private static void Main(string[] args)
    {
     

        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\18.txt");
        List<Line> instructions = new List<Line>();
        List<long[]> XY = new List<long[]>();

        
        foreach (string s in strings)
        {
            instructions.Add(new Line(s));
        }

        long X = 0;
        long Y = 0;

     //   XY.Add(new[] { X, Y });

        foreach (Line instruction in instructions)
        {

            if (instruction.direction == Line.Direction.Left)
            {
                instruction.XStart = X ;
                instruction.YStart = Y;
                X -= (instruction.Length );
            }
            else if (instruction.direction == Line.Direction.Right)
            {
                instruction.XStart = X ;
                instruction.YStart = Y;
                X += instruction.Length ;
            }
            else if (instruction.direction == Line.Direction.Up)
            {
                instruction.XStart = X;
                instruction.YStart = Y ;
                Y -= (instruction.Length );
            }
            else
            {
                instruction.XStart = X;
                instruction.YStart = Y;
                Y += instruction.Length ;
            }
            XY.Add(new[] { X, Y });
            instruction.XEnd = X;
            instruction.YEnd = Y;

        }

        int Winkel = 0;

        for(int i = 1; i < instructions.Count; i++)
        {
            Winkel += Math.Sign(instructions[i - 1].CrossProduct(instructions[i]));
        }

        Console.WriteLine(Winkel);
        if (Winkel > 0) ClockWise = true; else ClockWise = false;



        Int128 result =  instructions.Sum(o => o.Length)  ;


        
        for(int i= 1; i < XY.Count-1; i++)
        {
            result += XY[i][0] * (XY[i + 1][1] - XY[i-1][1]);
        }
        result += XY[0][0] * (XY[1][1] - XY[XY.Count - 1][1] );
        result += XY[XY.Count-1][0] * (XY[0][1] - XY[XY.Count - 2][1] );
        result /= 2;
        result += ClockWise ? 1 : -1;
        Console.WriteLine (result);
        Console.WriteLine(XY.Min(o => o[0]) + ";" + XY.Max(o => o[0]));
        Console.WriteLine(XY.Min(o => o[1]) + ";" + XY.Max(o => o[1]));

        
    }
}

    
public class Line
{
   public Direction direction {  get; set; }
    public int Length { get; set; }

    public long XStart {get; set;}
    public long XEnd { get; set;}
    public long YStart { get; set;}
    public long YEnd { get; set;}
    
    public long[] Vektor { 
        get {
            long XDif = XEnd - XStart;
            if (XDif < 0) XDif--;
            else if(XDif > 0) XDif++;
            long YDif = YEnd - YStart;
            if (YDif < 0) YDif--;
            else if (YDif > 0) YDif++;
            return  new[] { XDif ,YDif }; 
        } }
    
    public Line(string input)
    {
        
        var inp = input.Split(' ');
        string hex = inp[2].Replace("#", "0x").Replace("(", "").Replace(")", "");
        char directionChar = hex[hex.Length - 1];
        if (directionChar == '1') direction = Direction.Down;
        else if (directionChar == '2') direction = Direction.Left;
        else if (directionChar == '0') direction = Direction.Right;
        else direction = Direction.Up;

        Length = Convert.ToInt32(hex.Substring(0,hex.Length-1),16);
                
        /*
        var inp = input.Split(' ');
        if (inp[0] == "D") direction = Direction.Down;
        else if (inp[0] == "L") direction = Direction.Left;
        else if (inp[0] == "R") direction = Direction.Right;
        else direction = Direction.Up;

        Length = int.Parse(inp[1]);
        */
        }
    public Line(int XStart,int XEnd,int YStart,int YEnd,Direction direction)
    {
        this.XStart = XStart;
        this.XEnd = XEnd;
        this.YEnd = YEnd;
        this.YStart = YStart;
        this.direction = direction;
    }
    /*
    public List<Line> Divide(List<Line> lines)
    {
        if (Vektor[1] ==0) return new List<Line>() { this };
        List<Line> result = new List<Line>();
      //  var crosses = lines.Where(o => o.Vektor[1] >= 0   && o.YStart >= YStart && o.YStart <= YEnd || o.YEnd >= YStart && o.YEnd <= YEnd || o.YEnd <= YStart && o.YStart >= YEnd).OrderBy(o => o.YEnd);

        List<int> Ys = lines.Where(o => o.Vektor[1] != 0).Select(o => o.YEnd).ToList();
       // Ys.AddRange(lines.Select(o => o.YStart));
        if (Vektor[1] < 0) Ys = Ys.Where(o => o <= YStart && o >= YEnd).Distinct().OrderByDescending(o => o).ToList();
        else if (Vektor[1] > 0) Ys = Ys.Where(o => o <= YEnd && o >= YStart).Distinct().OrderBy(o => o).ToList();

        int curY = YStart;
       
        foreach(int y in Ys)
        {
            if (Vektor[1] > 0) result.Add(new Line(XStart, XEnd, curY, y - 1,direction));
            else result.Add(new Line(XStart, XEnd, curY, y + 1, direction));
            curY = y;
        }
        result.Add(new Line(XStart,XEnd,curY,YEnd, direction));

        return result;
    }

    public long GetArea(List<Line> lines , bool clockWise)
    {
        if (Vektor[1] ==0)  return 0; 
        if ( !clockWise && Vektor[1]  >= 0) return 0;
        if( clockWise && Vektor[1] <= 0) return 0;
        var equals = lines.Where(o => o != this && o.Vektor[1] != 0 && (o.XEnd > XEnd && !clockWise || o.XEnd < XEnd && clockWise) && (o.YStart == YStart || o.YStart == YEnd));//( (o.YStart == YEnd && o.YEnd == YStart) || (o.YEnd == YEnd && o.YStart == YStart) ));
        Line wall = equals.OrderBy(o => Math.Abs(XEnd - o.XEnd)).FirstOrDefault();
        if (lines.Any(o => o.Vektor[1] == 0 && (o.YStart == YStart || o.YStart == YEnd) && ((o.XStart == XStart + (clockWise ? -1 : 1) || o.XEnd == XEnd + (clockWise ? 1 : -1))))) return 0;
        if(wall == null) return 0;
        return Math.Abs(Vektor[1]) * (Math.Abs(XEnd - wall.XEnd)-1);
    }
    */
    public Line(Direction direction, int length)
    {
        this.direction = direction;
        Length = length;

    }
    public long CrossProduct(Line line)
    {
        return Vektor[0] * line.Vektor[1] - line.Vektor[0] * Vektor[1];
            
    }
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down

    }
}