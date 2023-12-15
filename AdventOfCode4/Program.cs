using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\4.txt");

        List<int[]> inputs = new List<int[]>();
        List<int[]> outputs = new List<int[]>();

        foreach (string s in strings)
        {
            string input = s.Split('|')[0];
            string output = s.Split('|')[1];

            string[] inputAr =  input.Split(':')[1].Split(' ');
            string[] outputAr = output.Split(' ');

            inputAr = inputAr.Where(o => o != string.Empty).ToArray();
            outputAr = outputAr.Where(o => o != string.Empty).ToArray();
            inputs.Add(inputAr.Select(o => int.Parse(o)).ToArray());
            outputs.Add(outputAr.Select(o => int.Parse(o)).ToArray());
        }
        int result = 0;


        result= GetCards(inputs, outputs,0,outputs.Count);

        Console.WriteLine(result.ToString());
    }

    private static int GetCards(List<int[]> inputs, List<int[]> outputs,int start,int count)
    {
        int result = 0;
        for (int i = start; i <  outputs.Count && i < start + count; i++)
        {
            int matches = inputs[i].Where(o => outputs[i].Contains(o)).Count();

            result += 1;
            if (matches > 0)
            {
               
                result += GetCards(inputs, outputs, i +1,matches);

            }
        }
        return result;
    }
}