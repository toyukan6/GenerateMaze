using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMaze
{
    abstract class Maze
    {
        protected Mass[,] maze;

        protected int mazeHeight { get { return maze.GetLength(0); } }

        protected int mazeWidth { get { return maze.GetLength(1); } }

        protected Random rand = new Random();

        protected bool makeWall = false;

        public Maze(int height, int width)
        {
            maze = new Mass[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze[i, j] = new Mass(i, j);
                    if (i == 0 || i == height - 1 || j == 0 || j == width - 1)
                        maze[i, j].MassState = State.Wall;
                }
            }
        }

        public abstract void GenerateMaze();

        protected abstract List<Direction4> SurveyDirection(Mass m);

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < mazeHeight; i++)
            {
                for (int j = 0; j < mazeWidth; j++)
                {
                    if (maze[i, j].MassState == State.Empty)
                        s += "□";
                    else if (maze[i, j].MassState == State.Wall)
                        s += "■";
                }
                s += "\r\n";
            }
            return s;
        }

        protected virtual void Classification(List<Mass> wall, List<Mass> empty)
        {
            wall.Clear();
            empty.Clear();
            int first = (makeWall) ? 0 : 1;
            for (int i = first; i < mazeHeight; i += 2)
            {
                for (int j = first; j < mazeWidth; j += 2)
                {
                    if (maze[i, j].MassState == State.Wall)
                        wall.Add(maze[i, j]);
                    else if (maze[i, j].MassState == State.Empty)
                        empty.Add(maze[i, j]);
                }
            }
        }

        protected T IListRandom<T>(IList<T> ilist)
        {
            return ilist[rand.Next(ilist.Count)];
        }

        protected void Show()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine(ToString());
            Console.ReadKey();
        }
    }
}
