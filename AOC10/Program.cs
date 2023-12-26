using Microsoft.VisualBasic;
using SkiaSharp;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal class Program
{
    private static int MaxX;
    private static int MaxY;
    private static void Main(string[] args)
    {
        string[] strings = File.ReadAllLines(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\10.txt");
        Tile[,] tileMap = new Tile[strings[0].Trim().Length, strings.Length];
        MaxY = strings.Length - 1;
        MaxX = strings[0].Length - 1;

        for (int y = 0; y < strings.Length; y++)
        {
            for (int x = 0; x < strings[y].Length; x++)
            {
                char c = strings[y][x];
                if (c != '.')
                {
                    tileMap[x, y] = new Tile(x, y, c);

                }
            }

        }
        int nothingCounter;// = tileMap.Cast<Tile>().Count(o => o == null);
        do
        {
            nothingCounter = tileMap.Cast<Tile>().Count(o => o == null);

            for (int y = 0; y < strings.Length; y++)
            {
                for (int x = 0; x < strings[y].Length; x++)
                {
                    Tile tile = tileMap[x, y];
                    if (tile != null && tile.symb != 'S')
                    {

                        if (tile.HasTop && y == 0) tileMap[x, y] = null;
                        else if (tile.HasBottom && y == tileMap.GetLength(1) - 1) tileMap[x, y] = null;
                        else if (tile.HasLeft && x == 0) tileMap[x, y] = null;
                        else if (tile.HasRight && x == tileMap.GetLength(0) - 1) tileMap[x, y] = null;
                        else if (tile.HasRight && (tileMap[x + 1, y] == null || !tileMap[x + 1, y].HasLeft)) tileMap[x, y] = null;
                        else if (tile.HasLeft && (tileMap[x - 1, y] == null || !tileMap[x - 1, y].HasRight)) tileMap[x, y] = null;
                        else if (tile.HasTop && (tileMap[x, y - 1] == null || !tileMap[x, y - 1].HasBottom)) tileMap[x, y] = null;
                        else if (tile.HasBottom && (tileMap[x, y + 1] == null || !tileMap[x, y + 1].HasTop)) tileMap[x, y] = null;

                    }
                }
            }
            Console.WriteLine(nothingCounter.ToString());

        } while (tileMap.Cast<Tile>().Count(o => o == null) != nothingCounter);

        Print(tileMap);

        Parallel.ForEach<Tile>(tileMap.Cast<Tile>(), tile =>
        {
            if (tile != null)
            {
                tile.SetConnections(tileMap);
            }
        });
        Console.WriteLine(tileMap.Cast<Tile>().Where(o => o != null && o.Connections.Count != 2).Count() + "/" + tileMap.Cast<Tile>().Count(o => o != null));

        Tile start = tileMap.Cast<Tile>().First(o => o?.symb == 'S');

        Tile temp = start;
        Tile prev = start.Connections.First();
        Tile next = start.Connections.First();

        List<Tile> Loop = new List<Tile>();
        Loop.Add(start);
        Loop.Add(next);

        int counter = 1;
        do
        {
            temp = next;
            next = next.GetNext(prev);
            prev = temp;
            counter++;
            next.InLoop = true;
            Loop.Add(next);
        } while (next.symb != 'S');

        Console.WriteLine(counter / 2);

        for (int y = 0; y < strings.Length; y++)
        {
            string line = "";
            for (int x = 0; x < strings[y].Length; x++)
            {
                if (tileMap[x,y] == null) tileMap[x, y] = new Tile(x, y, '.');
                if (!Loop.Contains(tileMap[x, y]))
                {
                   tileMap[x, y].symb = '.';
                    
                }

            }

        }
        Print(tileMap);
        Parallel.ForEach(tileMap.Cast<Tile>(), tile =>
        {
            tile.SetNeighbors(tileMap);
        });

        Tile[,] newMap = new Tile[strings[0].Length * 2 - 1, strings.Length * 2 - 1];

        for (int y = 0; y < newMap.GetLength(1); y++)
        {

            for (int x = 0; x < newMap.GetLength(0); x++)
            {
                if (tileMap[x / 2, y / 2].symb != '.')
                {

                }
                if (y % 2 == 0)
                {
                    if (x % 2 == 0)
                    {

                        newMap[x, y] = tileMap[x / 2, y / 2];
                    }
                    else
                    {
                        if (tileMap[x / 2, y / 2].Right.HasLeft && tileMap[x / 2, y / 2].HasRight)
                        {
                            newMap[x, y] = new Tile(x, y, '-');
                            newMap[x, y].Virual = true;
                        }
                        else
                        {
                            newMap[x, y] = new Tile(x, y, '.');
                            newMap[x, y].Virual = true;
                        }
                    }
                }
                else
                {
                    if (x % 2 == 0)
                    {
                        if (tileMap[x / 2, y / 2].Bot.HasTop && tileMap[x / 2, y / 2].HasBottom)
                        {
                            newMap[x, y] = new Tile(x, y, '|');
                            newMap[x, y].Virual = true;
                        }
                        else
                        {
                            newMap[x, y] = new Tile(x, y, '.');
                            newMap[x, y].Virual = true;
                        }

                    }
                    else
                    {
                        newMap[x, y] = new Tile(x, y, '.');
                        newMap[x, y].Virual = true;
                    }
                }
                Console.Write(newMap[x, y].symb);
            }
        Console.WriteLine();

        }
        for (int y = 0; y < newMap.GetLength(1); y++)
        {

            for (int x = 0; x < newMap.GetLength(0); x++)
            {
                newMap[x, y].x = x;
                newMap[x, y].y = y;
            }
        }
        //Print(newMap);

        Parallel.ForEach(newMap.Cast<Tile>(), tile =>
        {
            tile.SetNeighbors(newMap);
        });

        int count = 0;
        int emptys = newMap.Cast<Tile>().Count(o => o.IsEmpty);
        for (int y = 0; y < newMap.GetLength(1); y++)
        {

            for (int x = 0; x < newMap.GetLength(0); x++)
            {
                if (newMap[x, y].IsEmpty)
                {
                    if (!newMap[x, y].Out.HasValue)
                    {
                        List<Tile> trace = new List<Tile>(); 
                         bool? res= newMap[x, y].GetOut(newMap, trace);
                        if (res.HasValue)
                        {
                            newMap[x, y].Out = res.Value;
                        }
                        else newMap[x, y].Out = false;

                        trace.ForEach(o => o.Out = newMap[x, y].Out);
                    }
                    Console.WriteLine(count++ + "/"+ emptys);
                }
             
            }
        }

        foreach (Tile item in newMap.Cast<Tile>())
        {
            if (item.IsEmpty && item.Out.Value) item.newSym = '0';
            else if ( item.IsEmpty && !item.Out.Value) item.newSym = 'I';
        }

        Print(newMap);
        Console.WriteLine(newMap.Cast<Tile>().Count(o => o.IsEmpty && !o.Out.Value  && !o.Virual));
    }
    private static void Print(Tile[,] tileMap)
    {
        Console.WriteLine("\n\n\n");
        Console.WriteLine("\n" + string.Join("", Enumerable.Range(0, tileMap.GetLength(0)).Select(o => o%10)));
        for (int y = 0; y < tileMap.GetLength(1); y++)
        {
            string line = y.ToString("000");
            for (int x = 0; x < tileMap.GetLength(0); x++)
            {
                Tile tile = tileMap[x, y];
                if (tile != null)
                {
                    line += tile.newSym ?? tile.symb;
                }
                else
                {
                    line += " ";
                }
            }
            Console.WriteLine(line);
        }
    }


}

public class Tile
{
    public Tile(int x, int y, char symb)
    {
        this.x = x;
        this.y = y;
        Connections = new List<Tile>();
        this.symb = symb;
    }
    public char? newSym { get; set; } = null;
    public bool IsEmpty => symb == '.';
    public int x { get; set; }
    public int y { get; set; }

    public bool Virual;
    public bool InLoop { get; set; } = false;
    public bool? Out { get; set; } = null;
    public char symb { get; set; }
    public List<Tile> Connections { get; set; }
    public List<Tile> Neighbors { get; set; }

    public bool HasTop { get { return symb == '|' || symb == 'L' || symb == 'J' || symb == 'S'; } }

    public bool HasBottom { get { return symb == '|' || symb == 'S' || symb == '7' || symb == 'F'; } }
    public bool HasLeft { get { return symb == 'S' || symb == '7' || symb == '-' || symb == 'J'; } }

    public bool HasRight { get { return symb == 'S' || symb == '-' || symb == 'L' || symb == 'F'; } }

    public Tile Left
    {
        get
        {
            return Neighbors.FirstOrDefault(IsLeft);
        }
    }

    public Tile Right => Neighbors.FirstOrDefault(o => IsRight(o));
    public Tile Top => Neighbors.FirstOrDefault(o => IsTop(o));
    public Tile Bot => Neighbors.FirstOrDefault(o => IsBottom(o));

    public void SetConnections(Tile[,] tiles)
    {
        var neighbors = tiles.Cast<Tile>().Where(o => o != null  && (Math.Abs(o.x - x)  +Math.Abs( o.y - y) ) == 1);
       Connections = neighbors.Where(o =>

            HasRight && o.HasLeft && o.x == x+1||
            HasLeft && o.HasRight && o.x == x-1||
            HasTop && o.HasBottom && o.y == y-1||
            HasBottom && o.HasTop && o.y == y+1
        ).ToList();
    }

        public void SetNeighbors(Tile[,] tiles)
        {
        Neighbors = new List<Tile>();
        if (x - 1 >= 0) Neighbors.Add(tiles[x - 1, y]);
        if(y - 1 >= 0) Neighbors.Add(tiles[x, y - 1]);
        if(y + 1 < tiles.GetLength(1)) Neighbors.Add(tiles[x,y + 1]);
        if (x + 1 < tiles.GetLength(0)) Neighbors.Add(tiles[x+1, y ]);
    }

    public Tile GetNext(Tile from)
    {

        return Connections.First(o => o != from);
    }
    public bool IsLeft(Tile tile)
    {
        if(tile is null) return false;
        return tile.x - x < 0 && tile.y == y;
    }
    public bool IsTop(Tile tile)
    {
        if (tile is null) return false;
        return (tile.y - y < 0) && tile.x == x;
    }
    public bool IsBottom(Tile tile)
    {
        if (tile is null) return false;
        return tile.y - y > 0 && tile.x == x;
    }
    public bool IsRight(Tile tile)
    {
        if (tile is null) return false;
        return tile.x - x > 0 && tile.y == y;
    }

    public bool? GetOut(Tile[,] tiles, List<Tile> trace)
    {
        if (Neighbors.Count < 4)
        {

         //   if (trace.Count > 250) Print(tiles,trace);   
            return true;
        }

        foreach (var neighbor in Neighbors.Where(o => o.IsEmpty && !trace.Contains(o)).OrderBy(o => o.y)) 
       {

            if (neighbor.Out.HasValue)
            {
                Out = neighbor.Out;
                //if (trace.Count > 250) Print(tiles, trace);
                return  neighbor.Out.Value;
            }
          //  List<Tile> nTrace = new List<Tile>();
            //nTrace.AddRange(trace);
            //nTrace.Add(this);
            trace.Add(this);
            bool? tempOut = neighbor.GetOut(tiles, trace);


            if (tempOut.HasValue && tempOut.Value)
            {
                Out = true;
                return true;

            }
            
        }
        return null;
     //  if (trace.Count > 250) Print(tiles, trace);

    }

    private void Print(Tile[,] tiles,List<Tile> trace)
    {
        SKBitmap bmp = new(tiles.GetLength(0), tiles.GetLength(1));
        using SKCanvas canvas = new(bmp);
        canvas.Clear(SKColor.Parse("#000000"));

        SKPaint paint = new() { Color = SKColors.White.WithAlpha(100), IsAntialias = true };
        byte[,,] pic = new byte[tiles.GetLength(0), tiles.GetLength(1), 3];

        for (int y = 0; y < tiles.GetLength(1); y++)
        {

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                pic[x, y, 1] = tiles[x, y].IsEmpty ? (byte)0 : (byte)200;
            }
        }

        int counter = 0;
        foreach (var tile in trace)
        {
            pic[tile.x, tile.y, 0] = (byte) (256 * ( 1- (float)counter++/ (float)trace.Count));
        }

        canvas.DrawBitmap(Tile.ArrayToImage(pic), SKPoint.Empty);
        SKFileWStream fs = new(@"C:\Users\jamin\OneDrive\Dokumente\AdventOfCode\10\" + Guid.NewGuid() + ".Png");
        bmp.Encode(fs, SKEncodedImageFormat.Png, 1000);
    }
    public static SKBitmap ArrayToImage(byte[,,] pixelArray)
    {
        int width = pixelArray.GetLength(1);
        int height = pixelArray.GetLength(0);

        uint[] pixelValues = new uint[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                byte alpha = 255;
                byte red = pixelArray[y, x, 0];
                byte green = pixelArray[y, x, 1];
                byte blue = pixelArray[y, x, 2];
                uint pixelValue = (uint)red + (uint)(green << 8) + (uint)(blue << 16) + (uint)(alpha << 24);
                pixelValues[y * width + x] = pixelValue;
            }
        }

        SKBitmap bitmap = new();
        GCHandle gcHandle = GCHandle.Alloc(pixelValues, GCHandleType.Pinned);
        SKImageInfo info = new(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);

        IntPtr ptr = gcHandle.AddrOfPinnedObject();
        int rowBytes = info.RowBytes;
        bitmap.InstallPixels(info, ptr, rowBytes, delegate { gcHandle.Free(); });

        return bitmap;
    }
    public override string ToString()
    {
        return x + ";" + y;
    }
}
