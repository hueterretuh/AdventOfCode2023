internal class Program
{
    private static void Main(string[] args)
    {
        List<Race> races = new List<Race>();
        races.Add(new Race(47707566, 282107911471062));
   //     races.Add(new Race(70, 1079));
 //       races.Add(new Race(75, 1147));
//        races.Add(new Race(66, 1062));

        int res = 1;
        foreach (var race in races)
        {
          int counter = 0;
            for (int i = 1; i < race.Time; i++) { 
                if((race.Time - i)*i > race.Distance)
                {
                    counter++;
                }
            }
            res *= counter;
        }

        Console.WriteLine(res);
    }
}

public class Race
{


    public Race(long time, long distance)
    {
        Time = time;
        Distance = distance;
    }

    public long Time { get; set;}
    public long Distance { get; set; }


}