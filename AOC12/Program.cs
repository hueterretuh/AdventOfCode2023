internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\12.txt");
        List<Line> lines = new List<Line>();

        foreach (string s in strings)
        {
            lines.Add(new Line(s));
        }

        long result = 0;
        foreach (Line line in lines)
            result += line.GetCombs();

        Console.WriteLine(result);
    }



    public enum Status
    {
        OK,
        Damged,
        UnKnown,
        UnKownAsDamag
    }

    public class Line
    {
        public Status[] stati { get; set; }
        public int[] groups { get; set; }

        public Line(string line)
        {
            var inp = line.Split(' ');
            stati = new Status[inp[0].Length];
            for (int i = 0; i < inp[0].Length; i++)
            {
                char c = inp[0][i];
                if (c == '.') stati[i] = Status.OK;
                else if (c == '#') stati[i] = Status.Damged;
                else stati[i] = Status.UnKnown;
            }

            groups = inp[1].Split(",").Select(o => int.Parse(o)).ToArray();

        }

        public long GetCombs()
        {
            long fitCounter = 0;
            List<int> undefs = new List<int>();

            for (int i = 0; i < stati.Length; i++)
            {
                if (stati[i] == Status.UnKnown) undefs.Add(i);
            }

            UInt128 helper = 0;
            for (UInt128 i = 0; i < (uint)1 << undefs.Count; i++)
            {
                Status[] statAr = new Status[stati.Length];
                stati.CopyTo(statAr, 0);
                for (int j = 0; j < undefs.Count; j++)
                {

                    if ((i & ((uint)1) << j) >> j == 0) statAr[undefs[j]] = Status.OK;
                    else statAr[undefs[j]] = Status.UnKownAsDamag;

                }
                if (Fits(statAr, groups))
                {
                    fitCounter++;
                }

            }
            return fitCounter;
        }

        private static bool Fits(Status[] stati, int[] groups)
        {
            List<int> mGroups = new List<int>();

            int counter = stati[0] != Status.OK ? 1 : 0;
            for (int i = 1; i < stati.Length; i++)
            {

                if ((stati[i] == Status.OK) && (stati[i - 1] != Status.OK))
                {
                    mGroups.Add(counter);
                    counter = 0;
                }
                else if(stati[i] != Status.OK)
                {
                    counter++;
                }
            }
            if (stati.Last() != Status.OK) { mGroups.Add(counter); }

           
            if(groups.Count() != mGroups.Count())   return false;

            for (int i = 0; i < groups.Length; i++)
            {
                if (mGroups[i] != groups[i]) return false;
            }
            return true;
        }
    }
}