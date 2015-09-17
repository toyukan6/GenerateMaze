using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMaze
{
    class DiggingAHole : Maze
    {
        public DiggingAHole(int height, int width)
            : base(height, width)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    maze[i, j].MassState = State.Wall;
                }
            }
            //Show();
        }

        public override void GenerateMaze()
        {
            List<Mass> wall = new List<Mass>(),
                       road = new List<Mass>();

            Classification(wall, road);
            DigNewRoad(wall);
            Classification(wall, road);

            while (wall.Count > 0)
            {
                DigNewRoad(road);
                Classification(wall, road);
                //Show();
            }
        }

        void DigNewRoad(List<Mass> massList)
        {
            List<Mass> NewRoad = new List<Mass>();
            massList.RemoveAll(m => SurveyDirection(m).Count == 0);
            Mass mass = IListRandom(massList);
            mass.MassState = State.Empty;
            NewRoad.Add(mass);
            var directions = SurveyDirection(mass);
            while(directions.Count > 0)
            {
                var d = IListRandom(directions);
                var nw = MakeNewRoad(mass, d);
                NewRoad.Add(nw);
                mass = nw;
                directions = SurveyDirection(mass);
            }

            MakeRoad(NewRoad);
        }

        Mass MakeNewRoad(Mass m, Direction4 d)
        {
            int i = m.I, j = m.J;
            int newI = i, newJ = j;
            switch (d)
            {
                case Direction4.Up: newI = i - 2; break;
                case Direction4.Right: newJ = j + 2; break;
                case Direction4.Down: newI = i + 2; break;
                case Direction4.Left: newJ = j - 2; break;
            }

            maze[newI, newJ].MassState = State.Empty;

            return maze[newI, newJ];
        }

        void MakeRoad(List<Mass> road)
        {
            for (int i = 0; i < road.Count - 1; i++)
            {
                Mass wall1 = road[i], wall2 = road[i + 1];
                wall1.MassState = State.Empty;
                wall2.MassState = State.Empty;
                if (wall1.I != wall2.I)
                    maze[(wall1.I + wall2.I) / 2, wall1.J].MassState = State.Empty;
                else if (wall1.J != wall2.J)
                    maze[wall1.I, (wall1.J + wall2.J) / 2].MassState = State.Empty;
            }
        }

        protected override List<Direction4> SurveyDirection(Mass m)
        {
            int i = m.I, j = m.J;
            List<Direction4> directions = new List<Direction4>();
            if (i > 1 && maze[i - 2, j].MassState == State.Wall)
                directions.Add(Direction4.Up);
            if (i < mazeHeight - 2 && maze[i + 2, j].MassState == State.Wall)
                directions.Add(Direction4.Down);
            if (j > 1 && maze[i, j - 2].MassState == State.Wall)
                directions.Add(Direction4.Left);
            if (j < mazeWidth - 2 && maze[i, j + 2].MassState == State.Wall)
                directions.Add(Direction4.Right);

            return directions;
        }
    }
}
