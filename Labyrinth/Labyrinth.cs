using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LabyrinthText {
    class Labyrinth {
        private char[,] map;
        private List<Coordination> coords = new List<Coordination>();
        private Random r;
        private Direction direction;
        private const char wallVertical = '|';
        private const char wallHorizontal = '_';
        private const char root = 'X';
        private const int minDimension = 4;

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
        /// Vyplní pole charem '0'
        /// </summary>
        public void FillMap() {
            for (int i = 0; i < map.GetLength(0); i++) {
                for (int j = 0; j < map.GetLength(0); j++) {
                    map[i, j] = '0';
                }
            }
        }

        /// <summary>
        /// Vytvoří zdi kolem hrací plochy
        /// </summary>
        public void CreateBorders() {
            //levá a horní strana
            for (int i = 0; i < map.GetLength(0); i++) {
                map[i, 0] = wallVertical;
                map[0, i] = wallHorizontal;
            }
            //pravá a dolní
            for (int j = map.GetLength(0) - 1; j >= 0; j--) {
                map[j, map.GetLength(0) - 1] = wallVertical;
                map[map.GetLength(0) - 1, j] = wallHorizontal;
            }
        }

        /// <summary>
        /// Vytvoří zaklady pro zdi
        /// </summary>
        public void FillBases() {
            int x = 2;
            int y = 2;

            while (map[x, y - 1] == '0' && map[x, y + 1] == '0') {
                map[x, y] = root;
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
            r = new Random();
            int x;
            int y;
            int length;

            foreach (Coordination c in coords) {
                length = Enum.GetNames(typeof(Direction)).Length;
                direction = (Direction)r.Next(0, length);
                x = c.X;
                y = c.Y;

                switch (direction) {
                    case Direction.LEFT:
                        while (y > 0 && map[x, y] != wallHorizontal
                            && map[x, y] != wallVertical) {
                            //Console.WriteLine(direction + " X: {0} Y: {1}", x, y);
                            map[x, y] = wallHorizontal;
                            y--;
                        }
                        break;

                    case Direction.UP:
                        while (x > 0 && map[x, y] != wallHorizontal
                            && map[x, y] != wallVertical) {
                            //Console.WriteLine(direction + " X: {0} Y: {1}", x, y);
                            map[x, y] = wallVertical;
                            x--;
                        }
                        break;

                    case Direction.RIGHT:
                        while (y < map.GetLength(0) && map[x, y] 
                            != wallHorizontal && map[x, y] != wallVertical) {
                            //Console.WriteLine(direction + " X: {0} Y: {1}", x, y);
                            map[x, y] = wallHorizontal;
                            y++;
                        }
                        break;

                    case Direction.DOWN:
                        while (x < map.GetLength(0) && map[x, y] != wallHorizontal
                            && map[x, y] != wallVertical) {
                            //Console.WriteLine(direction + " X: {0} Y: {1}", x, y);
                            map[x, y] = wallVertical;
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
            if (dim > minDimension && dim % 2 > 0) {
                map = new char[dim, dim];
                FillMap();
                CreateBorders();
                FillBases();
                BuildWalls();
            }
            else
                throw new ArgumentException("Rozsah musí být větší než "
                    + minDimension + " a musí být liché číslo."
                    + " Obě dimenze musí být také stejně velké!");
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
