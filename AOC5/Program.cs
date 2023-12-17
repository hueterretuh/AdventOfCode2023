using System.Collections.Generic;
using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\5.txt");
        List<Int64> seeds = new List<Int64>();
        List<Int64> tempSeeds = new List<Int64>();
        List<Mapping> mappings = new List<Mapping>();
        string type = "seeds";

        foreach (string s in strings)
        {
            if (s == string.Empty)
            {
                type = "";
                continue;
            }

            if (type == "")
            {
                type = s;
                continue;
            }

            if (type == "seeds")
            {

                seeds.AddRange(s.Replace("seeds: ", "").Split(' ').Where(o => o != "").Select(o => Int64.Parse(o)));

            }
            else
            {
                mappings.Add(new Mapping(type, s.Split(' ').Where(o => o != "").ToArray()));
            }

        }



        List<Mapping[]> sortedMappings = new List<Mapping[]>();

        string lastType = mappings[0].type;
        List<Mapping> sameMaps = new List<Mapping>();

        foreach (Mapping mapping in mappings)
        {
            if (mapping.type == lastType)
            {
                sameMaps.Add(mapping);
            }
            else
            {
                sortedMappings.Add(sameMaps.ToArray());
                sameMaps = new List<Mapping>();
                lastType = mapping.type;
                sameMaps.Add(mapping);

            }
        }
        sortedMappings.Add(sameMaps.ToArray());

        List<Int64> results = new List<Int64>();
        foreach (Int64 seed in seeds)
        {
            Int64 res = seed;
            foreach (Mapping[] mappingAr in sortedMappings)
            {

                Mapping mMap = mappingAr.FirstOrDefault(o => res >= o.src && res < o.srcEnd);
                if (mMap != null)
                {
                    res = mMap.dest + (res - mMap.src);
                }
            }
            results.Add(res);
        }

        Console.WriteLine(results.Min().ToString());
    }

    private class Mapping
    {
        public string type;
        public Int64 dest { get; set;}
        public Int64 src { get; set;}
        public Int64 range { get; set;}

        public Int64 srcEnd
        {
            get
            {
                return src + range ;
            }
        }
        public Int64 destEnd
        {
            get
            {
                return dest + range ;
            }
        }
        public Mapping(string type ,string[] input)
        {
            this.type = type;
            this.dest = Int64.Parse(input[0]);
            this.src = Int64.Parse( input[1]);
            this.range = Int64.Parse(input[2]);
        }
    }
}