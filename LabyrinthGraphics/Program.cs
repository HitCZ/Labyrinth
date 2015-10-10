using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth {
    class Program {
        static void Main(string[] args) {
            LabyrinthGraphics.Labyrinth l = new LabyrinthGraphics.Labyrinth(11);
            char input = '0';

            while (input != '2') {
                Console.WriteLine("1 - Vygenerovat bludiště");
                Console.WriteLine("2 - Ukončit");
                Console.WriteLine("\nVaše volba: ");

                input = Console.ReadKey().KeyChar;
                Console.WriteLine("\n");

                switch (input) {
                    case '1':
                        l.PrintMap();
                        //l.ToFile();
                        break;
                    case '2':
                        break;
                    default:
                        Console.WriteLine("Musíte zvolit 1 nebo 2.");
                        break;
                }
            }
        }
    }
}
