using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace LabyrinthGraphics {
    class Labyrinth {
        private List<Coordination> coords;
        private Random r;
        private Direction direction;
        private char[,] map;
        private const char WALL_VERTICAL = '|';
        private const char WALL_HORIZONTAL = '_';
        private const char BLANK = '0';
        private const char ROOT = 'X';
        private const int MIN_DIMENSION = 4;

        public string Name { get; set; }
        public char[,] Map {
            get {
                return map;
            }
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Labyrinth(int dim) {
            Generate(dim);
        }

        /// <summary>
        /// Vyplní pole charem BLANK
        /// </summary>
        public void FillMap() {
            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(0); j++) {
                    map[i, j] = BLANK;
                }
            }
        }

        public void DrawMap(Canvas canvas) {
            Rectangle rect;
            SolidColorBrush black = new SolidColorBrush(Colors.Black);
            SolidColorBrush white = new SolidColorBrush(Colors.White);
            int width = 20;
            int height = width;
            int posLeft = 3;
            int posTop = 3;

            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(0); j++) {
                    rect = new Rectangle();
                    rect.Width = width;
                    rect.Height = height;

                    if (map[i, j] == WALL_HORIZONTAL 
                        || map[i, j] == WALL_VERTICAL) {
                        rect.Stroke = black;
                        rect.Fill = black;
                    }
                    else if (map [i, j] == BLANK) {
                        rect.Stroke = white;
                        rect.Fill = white;
                    }

                    Canvas.SetTop(rect, posTop);
                    Canvas.SetLeft(rect, posLeft);
                    canvas.Children.Add(rect);
                    posLeft += 20;
                }
                posLeft = 3;
                posTop += 20;
            }
        }

        /// <summary>
        /// Vytvoří zdi kolem hrací plochy
        /// </summary>
        public void CreateBorders() {
            //levá a horní strana
            for (int i = 0; i < map.GetLength(0); i++) {
                map[i, 0] = WALL_VERTICAL;
                map[0, i] = WALL_HORIZONTAL;
            }
            //pravá a dolní
            for (int j = map.GetLength(0) - 1; j >= 0; j--) {
                map[j, map.GetLength(0) - 1] = WALL_VERTICAL;
                map[map.GetLength(0) - 1, j] = WALL_HORIZONTAL;
            }
        }

        /// <summary>
        /// Vytvoří zaklady pro zdi
        /// </summary>
        public void FillBases() {
            int x = 2;
            int y = 2;
            coords = new List<Coordination>();

            while (map[x, y - 1] == BLANK && map[x, y + 1] == BLANK) {
                map[x, y] = ROOT;
                coords.Add(new Coordination(x, y));

                if (y + 2 != map.GetLength(1) - 1)
                    y += 2;
                else {
                    x += 2;
                    y = 2;
                }
            }
        }

        /// <summary>
        /// U každého základu zvolí náhodný směr a v tomto směru táhne zeď, 
        /// dokud nenarazí na jinou zeď
        /// </summary>
        public void BuildWalls() {
            int x;
            int y;
            int length;
            r = new Random();

            foreach (Coordination c in coords) {
                length = Enum.GetNames(typeof(Direction)).Length;
                direction = (Direction)r.Next(0, length);
                x = c.X;
                y = c.Y;

                switch (direction) {
                    case Direction.LEFT:
                        while (y > 0 && map[x, y] != WALL_HORIZONTAL
                            && map[x, y] != WALL_VERTICAL) {
                            //Console.WriteLine(direction + " X: {0} Y: {1}", x, y);
                            map[x, y] = WALL_HORIZONTAL;
                            y--;
                        }
                        break;

                    case Direction.UP:
                        while (x > 0 && map[x, y] != WALL_HORIZONTAL
                            && map[x, y] != WALL_VERTICAL) {
                            //Console.WriteLine(direction + " X: {0} Y: {1}", x, y);
                            map[x, y] = WALL_VERTICAL;
                            x--;
                        }
                        break;

                    case Direction.RIGHT:
                        while (y < map.GetLength(0) && map[x, y] 
                            != WALL_HORIZONTAL && map[x, y] != WALL_VERTICAL) {
                            //Console.WriteLine(direction + " X: {0} Y: {1}", x, y);
                            map[x, y] = WALL_HORIZONTAL;
                            y++;
                        }
                        break;

                    case Direction.DOWN:
                        while (x < map.GetLength(0) && map[x, y] != WALL_HORIZONTAL
                            && map[x, y] != WALL_VERTICAL) {
                            //Console.WriteLine(direction + " X: {0} Y: {1}", x, y);
                            map[x, y] = WALL_VERTICAL;
                            x++;
                        }
                        break;
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Vypíše mapu do konzole
        /// </summary>
        public void PrintMap() {
            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(0); j++) {
                    Console.Write(map[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Vygeneruje bludiště, pokud jsou splněny podmínky:
        ///     a) zadaná dimenze musí být větší než minimální dimenze (4)
        ///     b) zadaná dimenze je liché číslo
        /// Pokud tyto podmínky splněny nejsou, dojde k vyhození vyjímky.
        /// </summary>
        /// <param name="dim">velikost dimenzí - stejná velikost pro obě</param>
        public void Generate(int dim) {
            if (dim > MIN_DIMENSION && dim % 2 > 0) {
                map = new char[dim, dim];
                FillMap();
                CreateBorders();
                FillBases();
                BuildWalls();
            }
            else 
                throw new ArgumentException("Rozsah musí být větší než "
                    + MIN_DIMENSION + " a musí být liché číslo.");
        }

        /// <summary>
        /// Uloží vygenerované bludiště do souboru "labyrinth.txt",
        /// pokud již soubor existuje, vloží nové bludiště na konec.
        /// </summary>
        public void ToFile() {
            using (StreamWriter sw = new StreamWriter("labyrinth.txt", true)) {
                sw.WriteLine();
                for (int i = 0; i < map.GetLength(0); i++) {
                    for (int j = 0; j < map.GetLength(0); j++) {
                        sw.Write(map[i, j]);
                    }
                    sw.WriteLine();
                }
                Console.WriteLine("\nZápis do souboru proběhl úspěšně.");
            }
        }
    }
}
