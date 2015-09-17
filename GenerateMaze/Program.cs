using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 13, height = 13;
            DownBar db = new DownBar(height, width);
            GenerateMaze(db, "DownBar.txt");
            DiggingAHole dh = new DiggingAHole(height, width);
            GenerateMaze(dh, "DiggingAHole.txt");
            ExtendWall ew = new ExtendWall(height, width);
            GenerateMaze(ew, "ExtendWall.txt");
            Clustering c = new Clustering(height, width);
            GenerateMaze(c, "Clustering.txt");
        }

        static void GenerateMaze(Maze maze, string fileName)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            maze.GenerateMaze();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            string s = maze.ToString();
            //Console.WriteLine(s);
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(s);
            }
        }
    }
}
