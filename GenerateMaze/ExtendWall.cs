using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GenerateMaze
{
    class ExtendWall : Maze
    {
        public ExtendWall(int height, int width)
            : base(height, width)
        {
            makeWall = true;
            //Show();
        }

        public override void GenerateMaze()
        {
            List<Mass> wall = new List<Mass>(),
                       empty = new List<Mass>();

            Classification(wall, empty);

            while (empty.Count > 0)
            {
                ConnectNewWall(empty);
                Classification(wall, empty);
                //Show();
            }
        }

        void ConnectNewWall(List<Mass> empty)
        {
            List<Mass> NewWall = new List<Mass>();
            Mass e = IListRandom(empty);
            e.MassState = State.NewWall;
            NewWall.Add(e);
            do
            {
                var directions = SurveyDirection(e);
                if (directions.Count == 0) break;
                var d = IListRandom(directions);
                var nw = MakeNewWall(e, d);
                NewWall.Add(nw);
                e = nw;
            } while (e.MassState == State.NewWall);
            
            if (e.MassState == State.Wall)
                MakeWall(NewWall);
            else if (e.MassState == State.NewWall)
                RemoveNewWall(NewWall);
        }

        Mass MakeNewWall(Mass m, Direction4 direction)
        {
            int i = m.I, j = m.J;
            int newI = i, newJ = j;
            switch (direction)
            {
                case Direction4.Up: newI = i - 2; break;
                case Direction4.Right: newJ = j + 2; break;
                case Direction4.Down: newI = i + 2; break;
                case Direction4.Left: newJ = j - 2; break;
            }

            if (maze[newI, newJ].MassState != State.Wall)
                maze[newI, newJ].MassState = State.NewWall;

            return maze[newI, newJ];
        }

        void RemoveNewWall(List<Mass> newWall)
        {
            newWall.ForEach(nw => nw.MassState = State.Empty);
        }

        void MakeWall(List<Mass> newWall)
        {
            for (int i = 0; i < newWall.Count - 1; i++)
            {
                Mass wall1 = newWall[i], wall2 = newWall[i + 1];
                wall1.MassState = State.Wall;
                wall2.MassState = State.Wall;
                if (wall1.I != wall2.I)
                    maze[(wall1.I + wall2.I) / 2, wall1.J].MassState = State.Wall;
                else if (wall1.J != wall2.J)
                    maze[wall1.I, (wall1.J + wall2.J) / 2].MassState = State.Wall;
            }
        }

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
                    else if (maze[i, j].MassState == State.NewWall)
                        s += "◆";
                }
                s += "\r\n";
            }
            return s;
        }

        protected override List<Direction4> SurveyDirection(Mass m)
        {
            int i = m.I, j = m.J;
            List<Direction4> directions = new List<Direction4>();
            if (i > 1 && (maze[i - 2, j].MassState == State.Empty || maze[i - 2, j].MassState == State.Wall)) 
                directions.Add(Direction4.Up);
            if (i < mazeHeight - 2 && (maze[i + 2, j].MassState == State.Empty || maze[i + 2, j].MassState == State.Wall)) 
                directions.Add(Direction4.Down);
            if (j > 1 && (maze[i, j - 2].MassState == State.Empty || maze[i, j - 2].MassState == State.Wall)) 
                directions.Add(Direction4.Left);
            if (j < mazeWidth - 2 && (maze[i, j + 2].MassState == State.Empty || maze[i, j + 2].MassState == State.Wall)) 
                directions.Add(Direction4.Right);

            return directions;
        }
    }
}
