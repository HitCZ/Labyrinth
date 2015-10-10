using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace LabyrinthGraphics {
    class Block {
        private double width;
        private double height;
        private Color black;

        public int Width { get; private set; }
        public int Height { get; private set; }
        private Color Color { get; set; }

        public Block(int width, int height, Color color) {
            this.Width = width;
            this.Height = height;
            this.Color = color;
        }

    }
}
