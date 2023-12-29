using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

internal class Program
{
    private static char[,] input;
    private static bool[,] visited;
    private static char[,] output;
    private static List<string> cache = new List<string>();
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\16.txt");
        input = new char[strings[0].Length, strings.Length];
        output = new char[strings[0].Length, strings.Length];
        visited = new bool[strings[0].Length, strings.Length];



        for (int i = 0; i < strings.Length; i++)
        {
            string s = strings[i];
            for (int i1 = 0; i1 < s.Length; i1++)
            {
                char c = s[i1];
                input[i1, i] = c;
                //output[i1, i1] = c;
            }
        }
        Array.Copy(input, output, input.LongLength);
        List<Tuple<int[], Direction>> moves = new List<Tuple<int[], Direction>>();
        Tuple<int[], Direction> nextMove = new Tuple<int[], Direction>(new[] { 0, 0 }, Direction.Right);
        moves.Add(nextMove);
        do
        {
            List<Tuple<int[], Direction>> nextMoves = new List<Tuple<int[], Direction>>();
            foreach (var move in moves)
            {
                var curmoves = Move(move.Item1, move.Item2);
                if (curmoves != null) nextMoves.AddRange(curmoves);

            }
            moves = nextMoves;
        }
        while (moves.Count > 0);
        int res = visited.Cast<bool>().Count(o => o);
        for (int y = 0; y < visited.GetLength(1); y++)
        {
            for (int x = 0; x < visited.GetLength(0); x++)

            {
                Console.Write(output[x, y] );
            }
            Console.WriteLine();

    
        }
        
        Console.WriteLine(res);
        }


    enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }
    private static List<Tuple<int[],Direction>> Move(int[] XY,Direction direction)
    {
        if (XY[0] < 0 || XY[0] >= input.GetLength(0) || XY[1] < 0 || XY[1] >= input.GetLength(1))
        {
            return null;
        }
        visited[XY[0], XY[1]] = true;
        /*
        if (int.TryParse(output[XY[0], XY[1]].ToString(), out int num))
        {
            output[XY[0], XY[1]] = num++.ToString("0")[0];
        }
        else if ("<>^v".Contains(output[XY[0], XY[1]]))
        {
            output[XY[0], XY[1]] = '2';
        }
        else if (output[XY[0], XY[1]] == '.')
        {
            if (direction == Direction.Left)
            {
                output[XY[0], XY[1]] = '<';

            }
            else if (direction == Direction.Right)
            {
                output[XY[0], XY[1]] = '>';
            }
            else if (direction == Direction.Up)
            {
                output[XY[0], XY[1]] = '^';
            }
            else
            {
                output[XY[0], XY[1]] = 'v';
            }
        }
        */
        string key = XY[0] + "," + XY[1] + "," + direction;
        
        if (cache.Contains(key))
        {
            return null;
        }
        else cache.Add(key);
        
        List<Tuple<int[],Direction>> res = new List<Tuple<int[],Direction>>();

      //  Console.WriteLine($"{XY[0]},{XY[1]} - {direction}");
        int[] nextXY = new int[2];
        XY.CopyTo(nextXY,0);
        Direction nextDir = direction;
        char curField = input[nextXY[0],nextXY[1]];

        
        if(curField == '.')
        {
            nextXY = moveStandard(nextXY,direction);
        }else if(curField == '\\')
        {
            if(direction == Direction.Right)
            {
                nextDir = Direction.Down;
                nextXY[1] += 1;
            }else if(direction == Direction.Left)
            {
                nextDir = Direction.Up;
                nextXY[1] -= 1;
            }else if(direction == Direction.Up)
            {
                nextDir = Direction.Left;
                nextXY[0] -=1;
            }
            else
            {
                nextDir = Direction.Right;
                nextXY[0] += 1;
            }
        }else if (curField == '/')
        {
            if (direction == Direction.Right)
            {
                nextDir = Direction.Up;
                nextXY[1] -= 1;
            }
            else if (direction == Direction.Left)
            {
                nextDir = Direction.Down;
                nextXY[1] += 1;
            }
            else if (direction == Direction.Up)
            {
                nextDir = Direction.Right;
                nextXY[0] += 1;
            }
            else
            {
                nextDir = Direction.Left;
                nextXY[0] -= 1;
            }
        }else if((curField == '-' && (direction == Direction.Left || direction == Direction.Right)) || (curField == '|' && (direction == Direction.Up || direction == Direction.Down)))
        {
              nextXY =  moveStandard(nextXY, direction);
        }else if(curField == '-' && (direction == Direction.Up || direction == Direction.Down))
        {
            var moves = Move(new[] { nextXY[0] - 1, nextXY[1] }, Direction.Left);
            if(moves != null) res.AddRange(moves);

            var moves2 = ( Move(new[] { nextXY[0] + 1, nextXY[1] }, Direction.Right));
            if (moves2 != null) res.AddRange(moves2);
            return res;
        }else if(curField == '|' && (direction == Direction.Right || direction == Direction.Left))
        {
            var moves =  Move(new[] { nextXY[0] , nextXY[1] -1}, Direction.Up);
            if (moves != null) res.AddRange(moves);
            var moves2 =  Move(new[] { nextXY[0], nextXY[1]+1 }, Direction.Down);
            if (moves2 != null) res.AddRange(moves2);
            return res;
        }
        else
        {
            throw new Exception("asdf");
        }

        res.Add( new Tuple<int[], Direction>(nextXY, nextDir));
        return res;

    }

    private static int[] moveStandard(int[] XY,Direction direction)
    {
        int[] nextXY = XY;

        if (direction == Direction.Left)
        {
            nextXY[0] = XY[0] - 1;
        }
        else if (direction == Direction.Right)
        {
            nextXY[0] = XY[0] + 1;
        }
        else if (direction == Direction.Up)
        {
            nextXY[1] = XY[1] - 1;
        }
        else
        {
            nextXY[1] = XY[1] + 1;
        }
        return nextXY;
    }
}