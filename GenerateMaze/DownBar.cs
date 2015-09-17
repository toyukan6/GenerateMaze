using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMaze
{
    class DownBar : Maze
    {
        public DownBar(int height, int width)
            : base(height, width)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i % 2 == 0 && j % 2 == 0)
                        maze[i, j].MassState = State.Wall;
                }
            }
        }

        public override void GenerateMaze()
        {
            for (int i = 2; i < mazeHeight - 2; i += 2)
            {
                for (int j = 2; j < mazeWidth - 2; j += 2)
                {
                    var directions = SurveyDirection(maze[i, j]);
                    var d = IListRandom(directions);
                    MakeWall(maze[i, j], d);
                }
            }
        }

        protected override List<Direction4> SurveyDirection(Mass m)
        {
            int i = m.I, j = m.J;
            List<Direction4> directions = new List<Direction4>();
            if (i == 2 && maze[i - 1, j].MassState == State.Empty)
                directions.Add(Direction4.Up);
            if (i < mazeHeight - 1 && maze[i + 1, j].MassState == State.Empty)
                directions.Add(Direction4.Down);
            if (j > 0 && maze[i, j - 1].MassState == State.Empty)
                directions.Add(Direction4.Left);
            if (j < mazeWidth - 1 && maze[i, j + 1].MassState == State.Empty)
                directions.Add(Direction4.Right);

            return directions;
        }

        void MakeWall(Mass m, Direction4 d)
        {
            int newI = m.I, newJ = m.J;
            switch (d)
            {
                case Direction4.Up: newI = m.I - 1; break;
                case Direction4.Right: newJ = m.J + 1; break;
                case Direction4.Down: newI = m.I + 1; break;
                case Direction4.Left: newJ = m.J - 1; break;
            }

            maze[newI, newJ].MassState = State.Wall;
        }
    }
}
