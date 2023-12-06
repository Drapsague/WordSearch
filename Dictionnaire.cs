using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;


namespace mots_glisses
{
    internal class Dictionnaire
    {
        List<string> dictionnaire;
        // Dossier du projet - pour ouvrir les fichiers nécessaires
        string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        SortedList<char, int> nbWordsPerLetter;
        string langue;

        
        public Dictionnaire(string langue)
        {
            this.langue = langue;
            string filename = "Mots_Français.txt";
            dictionnaire = readDico(filename);
            dictionnaire = Tri_quick_sort(dictionnaire);
        }

        /// <summary>
        /// Décrit le dictionnaire à savoir ici le nombre de mots par lettre et la langue
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string res = "Langue : " + this.langue + "\n";
            foreach (KeyValuePair<char, int> kv in nbWordsPerLetter)
            {
                res += String.Format("Occurences lettre {0} : {1}", kv.Key, kv.Value);
            }
            return res;
        }


        public List<string> readDico(string filename)
        {
            List<string> dictio = new List<string>();
            string fullname = this.path + "\\" + filename;
            StreamReader sr = new StreamReader(fullname);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(" ");
                // Ajoute les éléments d'un tableau à une liste
                dictio.AddRange(words);
            }
            return dictio;
        }


        /// <summary>
        /// Affiche les mots du dictionnaire (DEBUG)
        /// </summary>
        /// <returns></returns>
        public string afficheDico()
        {
            string res = "";
            foreach (string mot in dictionnaire)
            {
                res += mot + "\n";
            }
            return res;
        }


        public bool RechDichoRecursif(string mot)
        {

        }


        public static void Tri_quick_sort(List<string> dictio)
        {
            
        }
    }
}
