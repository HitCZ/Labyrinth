using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGraphics {
    class Coordination {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Coordination(int x, int y) {
            this.X = x;
            this.Y = y;
        }
    }
}
