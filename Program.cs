using System;

namespace mots_glisses
{
    class Program
    {
        static void Main(string[] args)
        {
            string fich = "Test1.csv";
            Plateau pl = new Plateau(fich);
            Console.Write(pl.afficherPlateau());
            int nb = int.Parse(Console.ReadLine());
        }
    }
}
