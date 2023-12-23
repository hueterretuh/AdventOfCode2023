using System.Collections.Concurrent;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\8.txt");
        bool[] leftRight;
        string input = string.Join(string.Empty, strings.Take(2));
        List<bool?> lrList = new List<bool?>();
        foreach(char c in input)
        {
            lrList.Add(getLeftRightNone(c));
        }
        leftRight = lrList.Where(o => o.HasValue).Select(o => o.Value).ToArray();

        Dictionary<string,LeftRight> leftRightDic = new Dictionary<string,LeftRight>();

        foreach(string line in strings.Skip(2))
        {
            leftRightDic.Add(line.Substring(0,3),new LeftRight(line.Substring(7,3),line.Substring(12,3)));
        }

        /*
        string node = "AAA";
        int i = 0;
        int counter = 0;
        do
        {
            node = leftRightDic[node].getNode(leftRight[i]);

            counter++;
            if (i == leftRight.Length - 1) i = 0;
            else i++;
        } while (node != "ZZZ");
        */

        int i = 0;
        int counter = 0;
        int[] mult = new int[6];
        KeyValuePair<string, LeftRight>[] nodes = leftRightDic.Where(o => o.Key.EndsWith('A')).ToArray();
     //   List<string[]> testList = new List<string[]>();

      //  string[] tests = new string[6];
        do
        {
           
            //ConcurrentBag<KeyValuePair<string, LeftRight>> nextNodes = new ConcurrentBag<KeyValuePair<string, LeftRight>>();
            KeyValuePair<string, LeftRight>[] nextNodes = new KeyValuePair<string, LeftRight>[nodes.Length];
            //    Stopwatch stopwatch =  Stopwatch.StartNew();
            Parallel.For(0,nodes.Length,j =>
            {
                string nodeName = nodes[j].Value.getNode(leftRight[i]);
                nextNodes[j] = new KeyValuePair<string, LeftRight>(nodeName, leftRightDic[nodeName]);
         //     tests[j]  += ";" +nodeName;
            });
          //  Debug.Print("1:" +stopwatch.ElapsedMilliseconds.ToString());

            counter++;
            if (i == leftRight.Length - 1)
            {
                i = 0;
                //testList.Add(tests);
                //tests = new string[6];
            }
            else i++;
   //         stopwatch.Restart();
      //      Debug.Print("2:" + stopwatch.ElapsedTicks.ToString());
            nodes = nextNodes;

            if(nodes.Any(o => o.Key.EndsWith('Z')))
            {
                for (int i1 = 0; i1 < nodes.Length; i1++)
                {
                    KeyValuePair<string, LeftRight> node = nodes[i1];
                    if (node.Key.EndsWith('Z'))
                    {
                        Console.WriteLine(i1 + "\t " + counter);
                        if (mult[i1] == 0) mult[i1] = counter;

                        if(!mult.Any(o => o == 0))
                        {
                            Int128 result = 1;
                            foreach (var item in mult)
                            {
                                result *= item;   
                            }
                            Console.WriteLine(findlcm(mult,mult.Length));
                            return;
                        }
                    }
                }
            }
        //    stopwatch.Restart();
       //     Debug.Print("3:" + stopwatch.ElapsedTicks.ToString());
        } while(nodes.Any( o=> !o.Key.EndsWith('Z')) && counter < 1000000);

        /*
        List<int[]> indexList = new List<int[]>();
        foreach (string[] testst in testList)
        {
            int[] indexes = new int[testst.Length];
            for (int i1 = 0; i1 < testst.Length; i1++)
            {
                string tes = testst[i1];
                if(tes.EndsWith("Z")) indexes[i1] = i1;
            }
            indexList.Add(indexes);
        }

        foreach(var reses in indexList)
        {
            foreach(var res in reses)
            {
                Console.WriteLine(res + ";");
            }
            Console.WriteLine("\n");
        }
        */
        Console.WriteLine(counter );
    }


    private static bool? getLeftRightNone(char c)
    {
        if (c == 'L') return true;
        else if (c == 'R') return false;
        else return null;
    }
    static long  gcd(long a, long b)
    {
        if (b == 0)
            return a;
        return gcd(b, a % b);
    }
    static long findlcm(int[] arr, int n)
    {
        // Initialize result
        long ans = arr[0];

        // ans contains LCM of arr[0], ..arr[i]
        // after i'th iteration,
        for (int i = 1; i < n; i++)
            ans = (((arr[i] * ans)) /
                    (gcd(arr[i], ans)));

        return ans;
    }
}

internal class LeftRight
{
    public string Left { get; set; }
    public string Right { get; set; }
    public LeftRight(string left, string right) {
    
    this.Left = left;
    this.Right = right;
    }

    public string getNode(bool left)
    {
        return left ? Left : Right;
    }
}

