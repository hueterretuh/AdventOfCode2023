using System.Xml;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\20.txt");
        List<Device> devices = new List<Device>();
        foreach (string s in strings)
        {
            if(s.StartsWith("%")) devices.Add(new FlipFlop(s));
            else if(s.StartsWith("&")) devices.Add(new Conjunction(s));
            else devices.Add(new Broadcaster(s));
        }
     //   devices.Add(new Output());
        foreach(Device dev in devices)
        {
            dev.SetConnections(devices);
        }
        long lows = 0;
        long highs = 0;


        for (int i = 0; i < 1000; i++)
        {
            Broadcaster broadcaster = devices.First(o => o.GetType() == typeof(Broadcaster)) as Broadcaster;
            List<Signal> signals = broadcaster.GetInput(false, null);

            lows += signals.Count(o => !o.High) + 1;
            highs += signals.Count(o => o.High);
            do
            {
                List<Signal> newSignals = new List<Signal>();
                foreach (Signal signal in signals)
                {
                    newSignals.AddRange(signal.Send());
                }
                signals = newSignals;
                lows += signals.Count(o => !o.High);
                highs += signals.Count(o => o.High);
            } while (signals.Count > 0);
        }
        Console.WriteLine(lows * highs);
    }
}
public class Signal
{
    public Device From { get; set; }
    public Device To { get; set; }

    public bool High { get; set; }

    public Signal(Device from, Device to,bool high)
    {
        this.From = from;
        this.To = to;
        this.High = high;
    }

    public List<Signal> Send()
    {
    return    To.GetInput(High, From);
    }
}
public abstract class Device
{
    public List<Device> inputs { get; set; } = new List<Device>();
   public List<Device> outputs { get; set; } = new List<Device>();
    public string Name { get; set; }
    public string inputString { get; set; }
    public Device(string input)
    {
        this.inputString = input;
        var ins = input.Split(" -> ");
        Name = ins[0].Substring(1);

    }
    public Device() { }
    public virtual void SetConnections(List<Device> devices)
    {
        if (inputString == null) return;
        var ins = inputString.Split(" -> ");
        var outs = ins[1].Split(", ");
        outputs = devices.Where(o => outs.Contains(o.Name)).ToList();
        if (outputs.Count != outs.Count())
        {
            var NF = outs.Where(o => !outputs.Any(x => x.Name == o));
            if (NF.Count() != 1) throw new Exception();
            Output output = new Output() { Name = NF.First() };
      //      devices.Add(output);
            outputs.Add(output);


        }

        foreach(var outDevice in outputs)
        {
            outDevice.inputs.Add(this);
            if(outDevice is Conjunction conf)
            {
                conf.Memory.Add(this, false);
            }
        }
    }

    public abstract List<Signal> GetInput(bool High,Device Sender);
}

public class FlipFlop : Device
{
    public bool State { get; set; } = false;
    public FlipFlop(string input) : base(input)
    {
    }
    public override List<Signal> GetInput(bool High,Device Sender) 
    {
        List<Signal> outSignals = new List<Signal>();
        if (High) return outSignals;
        
        
            foreach(var to in outputs)
            {
                if (State) outSignals.Add(new Signal(this, to, false));
                else outSignals.Add(new Signal(this, to, true));
            }
        
        State = !State;
        return outSignals;

        
    }
}
public class Conjunction : Device
{
    public Dictionary<Device,bool> Memory { get; set; } = new Dictionary<Device,bool>();
    public Conjunction(string input) : base(input)
    {
    }

    public override List<Signal> GetInput(bool High,Device Sender)
    {
        List<Signal> outSignals = new List<Signal>();
        Memory[Sender] = High;
        bool AllHigh = Memory.All(o => o.Value);
        foreach(var to in outputs)
        {
           if(AllHigh) outSignals.Add(new Signal(this,to, false));
            else outSignals.Add(new Signal(this, to, true));
        }

        return outSignals;

    }
    public override void SetConnections(List<Device> devices)
    {
        base.SetConnections(devices);

    }
}

public class Broadcaster : Device
{
    public Broadcaster(string input) : base(input)
    {
    }

    public override List<Signal> GetInput(bool High, Device Sender)
    {
        List<Signal> outSignals = new List<Signal>();
        foreach (var to in outputs)
        {
            outSignals.Add(new Signal(this, to, High));
        }
        return outSignals;
    }
}

public class Output : Device
{
    public Output() : base()
    {
        this.Name = "output";
    }

    public override List<Signal> GetInput(bool High, Device Sender)
    {
        return new List<Signal>();
    }
}
