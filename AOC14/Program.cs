using System.Diagnostics;

internal class Program
{
   static List<int> Hashes = new List<int>();

    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\14.txt");
     
        char[,] input = new char[strings[0].Length,strings.Length];


        for (int i = 0; i < input.GetLength(1); i++)
        {
            for (int j = 0; j < input.GetLength(0); j++)
            {
                input[j, i] = strings[i][j];
            }

        }


        
        Stopwatch sw = Stopwatch.StartNew();
        bool found = false;
        long count = 0;
        long cycles = 1000000000;
        int index = 0;
        do
        {

            int hash = getHashCode(input);
            if (Hashes.Contains(hash))
            {
                index = Hashes.IndexOf(hash);
                found = true;
                break;
            }

                Hashes.Add(hash);
             
                Roll(ref input);

            count++;

        } while (!found);

        long remaining =  (cycles -index-1) % (count - index);

        for(int i = 0; i <= remaining; i++)
        {
            Roll(ref input);
        }
        Console.WriteLine(sw.ElapsedMilliseconds);
        
        List<int> dups = Hashes.Where(o => Hashes.Count(x => x==o) > 1).OrderBy(o => o).ToList();

        long res = 0;
        Print(input);
        for (int i = 0; i < input.GetLength(1); i++)
        {
            for (int j = 0; j < input.GetLength(0); j++)
            {
                if (input[j, i] == 'O') res += input.GetLength(1) - i;
            }
          
        }
        Console.WriteLine(res);
    }

    private static char[,] Roll(ref char[,] input,char Dir = 'N')
    {
        
        
        if (Dir == 'N')
        {

            for (int j = 0; j < input.GetLength(0); j++)
            {

                int lastStone = -1;
                for (int i = 0; i < input.GetLength(1); i++)
                {
                    char c = input[j, i];


                    if (c == '#') lastStone = i;
                    else if (c == 'O')
                    {
                        if (lastStone < i - 1)
                        {
                            input[j, lastStone + 1] = 'O';

                            input[j, i] = '.';
                            lastStone++;
                        }
                        else
                        {
                            lastStone = i;
                        }

                    }

                }
            }
            Roll(ref input, 'W');
        }else if(Dir == 'S')
        {
          for (int j = 0; j < input.GetLength(0); j++)
               {

                   int lastStone = input.GetLength(1);
                   for (int i = input.GetLength(1) - 1; i >= 0; i--)
                   {
                       char c = input[j, i];


                       if (c == '#') lastStone = i;
                       else if (c == 'O')
                       {
                           if (lastStone > i + 1)
                           {
                               input[j, lastStone - 1] = 'O';

                               input[j, i] = '.';
                               lastStone--;
                           }
                           else
                           {
                               lastStone = i;
                           }

                       }

                   }
               }
            Roll(ref input, 'E');
        }
        else if(Dir == 'W')
        {
            for (int j = 0; j < input.GetLength(1); j++)
            {

                int lastStone = -1;
                for (int i = 0; i < input.GetLength(0); i++)
                {
                    char c = input[i, j];


                    if (c == '#') lastStone = i;
                    else if (c == 'O')
                    {
                        if (lastStone < i - 1)
                        {
                            input[lastStone + 1, j] = 'O';

                            input[i, j] = '.';
                            lastStone++;
                        }
                        else
                        {
                            lastStone = i;
                        }

                    }

                }
            }
            Roll(ref input, 'S');
        }
        else
        {
            for (int j = 0; j < input.GetLength(1); j++)
            {

                int lastStone = input.GetLength(0);
                for (int i = input.GetLength(0)-1; i >= 0; i--)
                {
                    char c = input[i, j];


                    if (c == '#') lastStone = i;
                    else if (c == 'O')
                    {
                        if (lastStone > i + 1)
                        {
                            input[lastStone - 1, j] = 'O';

                            input[i, j] = '.';
                            lastStone--;
                        }
                        else
                        {
                            lastStone = i;
                        }

                    }

                }
            }
        }
        return input;
    }


    private static void Print(char[,] output)
    {
        for(int i = 0;i < output.GetLength(1);i++) {
            for(int j = 0;j < output.GetLength(0); j++)
            {
                Console.Write(output[j,i]);
            }
           Console.WriteLine();
        }
    }

    private static int getHashCode(char[,] input)
    {
        int hc = input.Length;
        foreach (int val in input)
        {
            hc = unchecked(hc * 314159 + val);
        }
        return hc;
    }
}