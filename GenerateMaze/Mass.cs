using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateMaze
{
    class Mass
    {
        public State MassState;

        public int I { get; private set; }

        public int J { get; private set; }

        public int Label;

        public Mass(int i, int j)
            : this(i, j, 0)
        {
        }

        public Mass(int i, int j, int label)
        {
            MassState = State.Empty;
            I = i;
            J = j;
            this.Label = label;
        }
    }

    enum State
    {
        Empty,
        Wall,
        NewWall,
    }

    enum Direction4
    {
        Up,
        Right,
        Down,
        Left,
    }
}
