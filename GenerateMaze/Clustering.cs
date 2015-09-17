using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMaze
{
    class Clustering : Maze
    {
        public Clustering(int height, int width)
            : base(height, width)
        {
            int num = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i % 2 == 1 && j % 2 == 1)
                    {
                        maze[i, j].MassState = State.Empty;
                        maze[i, j].Label = num;
                        num++;
                    }
                    else
                    {
                        maze[i, j].MassState = State.Wall;
                    }
                }
            }
            //Show();
        }

        public override void GenerateMaze()
        {
            var clustering = Cluster();
            while (clustering.Count > 1)
            {
                var key = IListRandom(clustering.Keys.ToArray());
                var mass = GetRandomMass(clustering[key]);
                var d = IListRandom(mass.Item2);
                int label = BreakWall(mass.Item1, d, clustering);
                clustering = ConcatCluster(clustering, mass.Item1.Label, label);
                //Show();
            }
        }

        int BreakWall(Mass m, Direction4 d, Dictionary<int, List<Mass>> clustering)
        {
            int i = m.I, j = m.J;
            int newI = i, newJ = j;
            Mass newMass = null;
            switch (d)
            {
                case Direction4.Up: newI = i - 1; newMass = maze[i - 2, j]; break;
                case Direction4.Right: newJ = j + 1; newMass = maze[i, j + 2]; break;
                case Direction4.Down: newI = i + 1; newMass = maze[i + 2, j]; break;
                case Direction4.Left: newJ = j - 1; newMass = maze[i, j - 2]; break;
            }

            int minLabel = Math.Min(m.Label, newMass.Label),
                maxLabel = Math.Max(m.Label, newMass.Label);

            maze[newI, newJ].MassState = State.Empty;

            if (clustering.ContainsKey(maxLabel))
                clustering[maxLabel].ForEach(c => c.Label = minLabel);

            return maxLabel;
        }

        Dictionary<int, List<Mass>> Cluster()
        {
            var clustering = new Dictionary<int, List<Mass>>();
            for (int i = 1; i < mazeHeight; i += 2)
            {
                for (int j = 1; j < mazeWidth; j += 2)
                {
                    int label = maze[i, j].Label;
                    if (!clustering.ContainsKey(label))
                        clustering[label] = new List<Mass>();
                    clustering[label].Add(maze[i, j]);
                }
            }

            return clustering;
        }

        Dictionary<int, List<Mass>> ConcatCluster(Dictionary<int, List<Mass>> clustering, int minLabel, int maxLabel)
        {
            var newClustering = new Dictionary<int, List<Mass>>();
            foreach (var c in clustering)
            {
                if (c.Key == maxLabel)
                {
                    newClustering[minLabel] = newClustering[minLabel].Concat(clustering[maxLabel]).ToList();
                }
                else
                {
                    newClustering[c.Key] = new List<Mass>();
                    newClustering[c.Key].AddRange(c.Value);
                }
            }

            return newClustering;
        }

        Tuple<Mass, List<Direction4>> GetRandomMass(List<Mass> cluster)
        {
            Mass mass;
            List<Direction4> direction;
            do
            {
                mass = IListRandom(cluster);
                direction = SurveyDirection(mass);
                if (direction.Count == 0)
                    cluster.Remove(mass);
            } while (direction.Count == 0);

            return Tuple.Create(mass, direction);
        }

        protected override List<Direction4> SurveyDirection(Mass m)
        {
            int i = m.I, j = m.J;
            List<Direction4> directions = new List<Direction4>();
            if (i > 1 && m.Label != maze[i - 2, j].Label)
                directions.Add(Direction4.Up);
            if (i < mazeHeight - 2 && m.Label != maze[i + 2, j].Label)
                directions.Add(Direction4.Down);
            if (j > 1 && m.Label != maze[i, j - 2].Label)
                directions.Add(Direction4.Left);
            if (j < mazeWidth - 2 && m.Label != maze[i, j + 2].Label)
                directions.Add(Direction4.Right);

            return directions;
        }

        //public override string ToString()
        //{
        //    string s = "";
        //    for (int i = 0; i < mazeHeight; i++)
        //    {
        //        for (int j = 0; j < mazeWidth; j++)
        //        {
        //            if (maze[i, j].MassState == State.Empty)
        //                s += maze[i, j].Label.ToString().PadLeft(3);
        //            else if (maze[i, j].MassState == State.Wall)
        //                s += " ■";
        //        }
        //        s += "\r\n";
        //    }
        //    return s;
        //}
    }
}
