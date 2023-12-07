using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Diagnostics;

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
            //dictionnaire = Tri_quick_sort(dictionnaire);
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


        List<string> readDico(string filename)
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


        /// <summary>
        /// Recherche si un mot appartient au dictionnaire
        /// </summary>
        /// <param name="liste"></param>
        /// <param name="mot"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool RechDichoRecursif(List<string> liste, string mot, int index)
        {
            if (index > liste.Count - 1)
            {
                return false;
            }
            if (mot == liste[index])
            {
                return true;
            }

            return RechDichoRecursif(liste, mot, index + 1);

            


        }

        /// <summary>
        /// echange deux mots dans une liste
        /// </summary>
        /// <param name="liste"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        static void Echanger(List<string> liste, int index1, int index2)
        {
            string temp = liste[index1];
            liste[index1] = liste[index2];
            liste[index2] = temp;
        }


        /// <summary>
        /// renvoie l'index du pivot necessaire pour le Quick-Sort
        /// </summary>
        /// <param name="liste"></param>
        /// <param name="debut"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        static int indexPivot(List<string> liste, int debut, int fin)
        {
            string pivot = liste[fin];
            int i = debut - 1;
            for (int j =  debut; j < fin; j++)
            {
                if (liste[j].CompareTo(pivot) < 0)
                {
                    i++;
                    Echanger(liste, i, j);
                }
            }

            Echanger(liste, i + 1, fin);
            return i + 1;

        }


        /// <summary>
        /// Tri une liste par ordre alphabetique
        /// </summary>
        /// <param name="dictio"></param>
        /// <param name="debut"></param>
        /// <param name="fin"></param>
        static void Tri_quick_sort(List<string> dictio, int debut, int fin)
        {

            if(debut < fin) 
            {
                int pivot = indexPivot(dictio, debut, fin);
                Tri_quick_sort(dictio, debut, pivot - 1);
                Tri_quick_sort(dictio, pivot + 1, fin);
                
            }

        }
        
    }
}
