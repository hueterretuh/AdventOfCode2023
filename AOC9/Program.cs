internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\9.txt");
        List<int[]> inputs = new List<int[]>();
       // inputs.Add(new[] { 10, 13, 16, 21, 30, 45 });
        foreach (string s in strings)
        {
            string[] numstrings = s.Split(' ');
         inputs.Add(    numstrings.Select(o => int.Parse(o)).ToArray());

        }
        int result = 0;
        foreach (int[] nums in inputs)
        {
            List < int[]> tree = new List <int[]>();

            tree.Add(nums);
            int[] nextLine = getNextLine(nums);
            tree.Add(nextLine);
             while (nextLine.Any(o => o != 0))
                {

                    nextLine = getNextLine(nextLine);
                    tree.Add(nextLine);

                }
            result += getNewNum(tree);
        }

        Console.WriteLine(result.ToString());
    }
    private static int getNewNum(List<int[]> inputs)
    {
        int[][] inputAr = inputs.ToArray();
        int newNum = 0;
        for (int i = inputAr.Length-1; i >= 0; i--)
        {
            int numBefore = inputAr[i ][inputAr[i ].Length - 1];
            newNum = newNum + numBefore;


        }
        return newNum;
    }
    private static int[] getNextLine(int[] nums)
    {
        int[] result = new int[nums.Length-1];

        for (int i = 0; i < nums.Length -1; i++)
        {
            result[i] = nums[i +1] - nums[i];

        }



        return result;
    }
}