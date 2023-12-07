using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mots_glisses
{
    internal class Plateau
    {
        char[,] plateau;
        int nbLignes;
        int nbColonnes;
        int[] poidsMots;
        int[] maxOccurences;
        int nbLettresRestantes;
        string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;


        public Plateau(int nbLignes, int nbColonnes, string fichierContraintes = "Lettre.txt")
        {
            this.nbLignes = nbLignes;
            this.nbColonnes = nbColonnes;
            this.nbLettresRestantes = this.nbLignes * this.nbColonnes;

            GenAleat();
            readContraintes(fichierContraintes);
        }


        public Plateau(string fichierSauvegarde, string fichierContraintes = "Lettre.txt")
        {
            ToRead(fichierSauvegarde);
            readContraintes(fichierContraintes);
            this.nbLettresRestantes = this.nbLignes * this.nbColonnes;
        }


        public int NbLignes {  get { return nbLignes; } }
        public int NbColonnes { get { return nbColonnes; } }
        public int[] PoidsMots { get { return poidsMots; } }
        public int[] MaxOccurences { get { return maxOccurences; } }
        public int NbLettresRestantes { get { return nbLettresRestantes; } }



        /// <summary>
        /// Génère une matrice de char aléatoire
        /// </summary>
        /// <returns></returns>
        void GenAleat()
        {
            char[,] plateau = new char[nbLignes, nbColonnes];
            Random rnd = new Random();

            // Stocke le nombre d'occurences pour chaque lettre
            int[] occurences = new int[26];

            /*
            for (int i = 0; i < nbLignes; i++)
            {
                for (int j = 0; j < nbColonnes; j++)
                {
                    char randomChar = (char)('a' + rnd.Next(0, 26));
                    // Vérifie si le nombre d'occurences de la lettre choisie aléatoirement
                    // dans le plateau est inférieur au nombre max d'occurences pour cette lettre
                    int ascii_conversion = (int)randomChar - 97;
                    if (occurences[ascii_conversion] < maxOccurences[ascii_conversion])
                    {
                        plateau[i, j] = randomChar;
                        occurences[ascii_conversion]++;
                    }
                }
            }
            */


            int i = 0;
            int j = 0;
            while (i * j < nbLignes * nbColonnes)
            {
                char randomChar = (char)('a' + rnd.Next(0, 26));
                // Vérifie si le nombre d'occurences de la lettre choisie aléatoirement
                // dans le plateau est inférieur au nombre max d'occurences pour cette lettre
                int ascii_conversion = (int)randomChar - 97;
                if (occurences[ascii_conversion] < maxOccurences[ascii_conversion])
                {
                    this.plateau[i, j] = randomChar;
                    occurences[ascii_conversion]++;
                    i++;
                    j++;
                }
            }
        }


        // AJOUTER DES TRY CATCH PARTOUT
        /// <summary>
        /// Lit les contraintes d'occurences et de poids sur les lettres
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        void readContraintes(string filename)
        {
            string fullname = String.Format("{0}\\{1}", this.path, filename);
            string[] lines = File.ReadAllLines(fullname); 
            
            if (lines.Length == 26) 
            {
                int[] maxOccurences = new int[26];
                int[] poidsMots = new int[26];
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] data = lines[i].Split(",");
                    maxOccurences[i] = int.Parse(data[1]);
                    poidsMots[i] = int.Parse(data[2]);
                }
                this.poidsMots = poidsMots;
                this.maxOccurences = maxOccurences;
            }
            else
            {
                throw new Exception("Le fichier doit contenir exactement 26 lignes");
            }
        }


        /// <summary>
        /// Récupère un plateau à partir d'un csv
        /// </summary>
        /// <param name="nomfile"></param>
        void ToRead(string nomfile)
        {
            string fullname = this.path + "\\" + nomfile;
            string[] lines = File.ReadAllLines(fullname);            
            this.nbLignes = lines.Length;
            this.nbColonnes = lines[0].Split(";").Length;
            this.plateau = new char[nbLignes, nbColonnes];

            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(";");
                for (int j = 0; j < data.Length; j++)
                {
                    // Accès à l'indice 0 car chaque string est une lettre et ainsi on cast cette string en un char
                    this.plateau[i, j] = data[j][0];
                }
            }
        }


        /// <summary>
        /// Affiche la difficulté, le nombre de lignes, de colonnes et le nombre de mots à trouver du plateau
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            return String.Format("Nombre de lignes : {0}\nNombre de colonnes : {1}\n", this.nbLignes, this.nbColonnes);
        }


        /// <summary>
        /// Sauvegarde le plateau actuel dans un fichier .csv
        /// </summary>
        /// <param name="nomfile"></param>
        public void ToFile(string nomfile)
        {
            string fullname = this.path + "\\" + nomfile;
            string[] lines = File.ReadAllLines(fullname);
            this.nbLignes = lines.Length;
            this.nbColonnes = lines[0].Split(";").Length;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(";");
                for (int j = 0; j < data.Length; j++)
                {
                    // Accès à l'indice 0 car chaque string est une lettre et ainsi on cast cette string en un char
                    this.plateau[i, j] = data[j][0];
                }
            }
        }



        public string afficherPlateau()
        {
            string res = "";
            for (int i = 0; i < nbLignes; i++)
            {
                // Mise en forme des interlignes ___
                res += String.Concat(Enumerable.Repeat("+---", nbColonnes)) + "+\n";

                for (int j = 0; j < nbColonnes; j++)
                {
                    res += String.Format("| {0} ", plateau[i, j]);
                }
                res += "|\n";
            }
            // Mise en forme du dernier interligne
            res += String.Concat(Enumerable.Repeat("+---", nbColonnes)) + "+\n";
            return res;
        }


        public void Maj_Plateau(object objet)
        {
            this.nbLettresRestantes--;
        }
    }
}
