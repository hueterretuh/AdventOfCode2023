using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\5.txt");
        List<Int64[]> seeds = new List<Int64[]>();
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

                tempSeeds.AddRange(s.Replace("seeds: ", "").Split(' ').Where(o => o != "").Select(o => Int64.Parse(o)));

            }
            else
            {
                mappings.Add(new Mapping(type, s.Split(' ').Where(o => o != "").ToArray()));
            }

        }

        for(int i = 0; i < tempSeeds.Count; i+=2)
        {
            seeds.Add( new[] { tempSeeds[i], tempSeeds[i + 1] });
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
        /*
        List<Conversion> conversions = new List<Conversion>();
        foreach (long[] seed in seeds)
        {
            conversions.AddRange(getOuts(new Conversion(seed[0], seed[0] + seed[1]-1), sortedMappings).OrderBy(o => o.beginnSrc));
        }

        Console.WriteLine(conversions.Min(o => o.beginnSrc));
        */
        /*
        List<Conversion[]> cons = new List<Conversion[]>();

        foreach (Mapping[] mappingAr in sortedMappings) { 
        
            List<Conversion> conversions = new List<Conversion>();
            foreach(Mapping map in mappingAr)
            {
                conversions.Add(new Conversion(map.src, map.srcEnd, map.dest, map.destEnd));
            }

            conversions = conversions.OrderBy(o => o.beginnSrc).ToList();
            List<Conversion> newConversion = new List<Conversion>();
            if (conversions[0].beginnSrc != 0)
            {
                newConversion.Add(new Conversion(0, conversions[0].beginnSrc - 1, 0, conversions[0].beginnSrc - 1));
            }
            for(int i =1;i < conversions.Count; i++)
            {
                if (conversions[i - 1].endSrc + 1 < conversions[i].beginnSrc - 1)
                {
                    newConversion.Add(new Conversion(conversions[i - 1].endSrc + 1, conversions[i].beginnSrc - 1, conversions[i - 1].endSrc + 1, conversions[i].beginnSrc - 1));

                }
            }
            newConversion.Add(new Conversion(conversions[conversions.Count - 1].endSrc + 1, long.MaxValue, conversions[conversions.Count - 1].endSrc + 1, long.MaxValue));
            newConversion.AddRange(conversions);
            newConversion = newConversion.OrderBy(o => o.beginnSrc).ToList();

            cons.Add(newConversion.ToArray());
        }
        
        for (int i = 0; i < cons.Count-1; i++)
        {
            List<Conversion> convsers = new List<Conversion>();
            Conversion[] srConAr = cons[i];
            Conversion[] deConAr = cons[i+1];

            foreach (Conversion src in srConAr) 
            {
                deConAr.Where(dest => src.beginDest >= dest.beginnSrc && src.endDest <= dest.endSrc);
            }

        }
        */


        long result = long.MaxValue;
        foreach (Int64[] seed in seeds)
        {
            Console.WriteLine("Yes");
            Parallel.For(seed[0], seed[0] + seed[1] - 1, i =>
            //  for (long i = seed[0]; i< seed[0] + seed[1]; i++)
            {
                long res = i;
                foreach (Mapping[] mappingAr in sortedMappings)
                {

                    var mMap = mappingAr.FirstOrDefault(o => res >= o.src && res < o.srcEnd);
                    if (mMap != null)
                    {
                        res += (mMap.dest - mMap.src);
                    }
                }
                if (res < result) result = res;
            });
          //  List<long> tempResults = new List<long>();

            
        }

        Console.WriteLine(result);

    }

    private static IEnumerable<Conversion> getOuts(Conversion conversion, List<Mapping[]> maps) {

        IEnumerable<Conversion> outs = getOut(conversion, maps[0]);
        for (int i = 0; i < maps.Count; i++) {
            List<Conversion> newOuts = new List<Conversion>();
            Mapping[] mapArr = maps[i];
            foreach(Conversion con in outs)
            {
                newOuts.AddRange(getOut(con, mapArr));

            }
            
            outs = newOuts;
        }
        return outs;
    }
    private static IEnumerable<Conversion> getOut(Conversion conversion, Mapping[] maps)
    {
        maps= maps.OrderBy(o => o.src).ToArray();
        List<Conversion> result = new List<Conversion>();
        if(conversion.beginnSrc == 0)
        {
            Debug.Print("ASDF");
        }
        if(conversion.beginnSrc < maps[0].src)
        {
            if (conversion.endSrc >= maps[0].src)
                yield return new Conversion(conversion.beginnSrc, maps[0].src - 1);
            else
                yield return new Conversion(conversion.beginnSrc, conversion.endSrc);
        }
        for (int i = 0; i < maps.Length; i++) {
            Mapping mapping = maps[i];
            Mapping nextMap;
            if (i < maps.Length - 1) {
              nextMap = maps[i + 1];
            }
            else
            {
                nextMap = null;
            }
            if (mapping.src <= conversion.beginnSrc && mapping.srcEnd >= conversion.beginnSrc)
            {
                if(mapping.srcEnd  <= conversion.endSrc)
                {
                    yield return new Conversion(conversion.beginnSrc + (mapping.dest - mapping.src), mapping.destEnd);
                    yield return new Conversion(mapping.srcEnd +1,conversion.endSrc);
                    
                }else
                {
                    yield return new Conversion(conversion.beginnSrc + (mapping.dest - mapping.src), conversion.endSrc + (mapping.dest - mapping.src));
                    break;
                }
            }
            else if(mapping.srcEnd > conversion.endSrc && mapping.src < conversion.endSrc)
            {
                if(mapping.src >= conversion.beginnSrc)
                {
                    yield return new Conversion(mapping.dest, conversion.endSrc + (mapping.dest - mapping.src));
                    yield return new Conversion(conversion.beginnSrc, mapping.src -1);
                }
            }else if(mapping.src >= conversion.beginnSrc && mapping.srcEnd <= conversion.endSrc)
            {
                if(nextMap != null && nextMap.src > conversion.endSrc)
                {
                    yield return new Conversion(mapping.srcEnd +1 , conversion.endSrc);
                }else if(nextMap != null && mapping.srcEnd +1 != nextMap.src)
                {
                    yield return new Conversion(mapping.srcEnd +1 , nextMap.src-1);
                }
                    
                yield return new Conversion(mapping.dest, mapping.destEnd);

            }else if(i < maps.Length -1 && conversion.beginnSrc > mapping.srcEnd && conversion.endSrc < maps[i + 1].src)
            {
                yield return new Conversion(conversion.beginnSrc, conversion.endSrc);
                break;
            }
        }

        if(conversion.endSrc > maps[maps.Length -1].srcEnd)
        {
            if(conversion.beginnSrc > maps[maps.Length - 1].srcEnd)
            {
                yield return new Conversion(conversion.beginnSrc, conversion.endSrc);
            }
            else
            {
                yield return new Conversion(maps[maps.Length - 1].srcEnd+1, conversion.endSrc);
            }
                
        }
        
    }
    public class Conversion
    {


        public long beginnSrc { get; set; }
        public long endSrc { get; set; }



        public Conversion(long beginnSrc, long endSrc) 
        {
            if(beginnSrc == 0)
            {
                Debug.Assert(endSrc == 0);
            }
            this.beginnSrc = beginnSrc;
            this.endSrc = endSrc;

        }
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
                return src + range-1 ;
            }
        }
        public Int64 destEnd
        {
            get
            {
                return dest + range -1;
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