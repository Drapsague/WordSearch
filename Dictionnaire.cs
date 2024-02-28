using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO.Enumeration;

namespace mots_glisses
{
    public class Dictionnaire
    {
        List<string> dictionnaire;
        // Dossier du projet - pour ouvrir les fichiers nécessaires
        readonly string PATH = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        SortedList<char, int> nbMotsParLettre;
        string langue;

        
        /// <summary>
        /// Constructeur de Dictionnaire
        /// </summary>
        /// <param name="langue">langue du dictionnaire</param>
        public Dictionnaire(string langue)
        {
            dictionnaire = new List<string>();
            this.langue = langue;
            string filename;

            switch (this.langue)
            {
                case "FR":
                    filename = "Mots_Français.txt";
                    break;
                case "EN":
                    filename = "Mots_Anglais.txt";
                    break;
                default:
                    filename = "Mots_Français.txt";
                    break;
            }

            nbMotsParLettre = new SortedList<char, int>();
            ReadDico(filename);
            TriQuickSort(dictionnaire.Count - 1);
        }


        public List<string> Dico
        {
            get { return this.dictionnaire;}
        }

        public SortedList<char, int> NbMotsParLettre
        {
            get { return this.nbMotsParLettre; }
        }

        public string Langue
        {
            get { return this.langue; }
        }
        

        /// <summary>
        /// Décrit le dictionnaire à savoir ici le nombre de mots par lettre et la langue
        /// </summary>
        /// <returns>String contenant la langue du dictionnaire et le nombre de mots par lettre</returns>
        public string toString()
        {
            string res = "Langue : " + this.langue + "\n";
            foreach (KeyValuePair<char, int> kv in nbMotsParLettre)
            {
                res += String.Format("Occurences lettre {0} : {1}\n", kv.Key, kv.Value);
            }
            return res;
        }
        

        /// <summary>
        /// Récupère la liste de mots et calcule le nombre de mots par lettre à partir du fichier dictionnaire 
        /// </summary>
        /// <param name="filename">Nom du fichier dictionnaire à lire</param>
        void ReadDico(string filename)
        {
            string fullname = this.PATH + "\\" + filename;
            StreamReader sr;
            try
            {
                sr = new StreamReader(fullname);
            }
            catch
            {
                sr = null;
                Console.Clear();
                Console.WriteLine("Il y a eu un problème lors de l'ouverture du fichier dictionnaire : " + filename);
                Thread.Sleep(2000);
                Environment.Exit(1);
            }

            string line;
            while ((line = sr.ReadLine()) != null)
            {
                // On récupère la liste de mots séparés par un espace 
                string[] words = line.Split(" ");
                foreach (string word in words)
                {
                    // On vérifie si la lettre existe déjà
                    if (nbMotsParLettre.ContainsKey(word[0]))
                    {
                        // Dans ce cas on incrémente le nb d'occurences
                        nbMotsParLettre[word[0]]++;
                    }
                    else
                    {
                        nbMotsParLettre[word[0]] = 0;
                    }
                    // Ajoute le mot à la liste des mots (dictionnaire)
                    this.dictionnaire.Add(word);
                }
            }
            sr.Close();
        }


        /// <summary>
        /// Affiche les mots du dictionnaire (DEBUG)
        /// </summary>
        /// <returns>String contenant les mots du dictionnaire (avec retour à la ligne)</returns>
        public string AfficheDico()
        {
            string res = "";
            foreach (string mot in dictionnaire)
            {
                res += mot + "\n";
            }
            return res;
        }


        /// <summary>
        /// Cherche si un mot appartient au dictionnaire
        /// </summary>
        /// <param name="mot">Mot recherché</param>
        /// <param name="fin">Taille du dictionnaire</param>
        /// <param name="debut">Début du dictionnaire (par défaut à 0) mais peut-être augmenté pour accélérer la recherche</param>
        /// <returns>true si le mot est dans le dictionnaire, false sinon</returns>
        public bool RechDichoRecursif(string mot, int fin, int debut = 0)
        {
            int milieu = (debut + fin) / 2;
            if (debut > fin || dictionnaire == null || dictionnaire.Count == 0) return false;
            else if (mot.ToUpper() == dictionnaire[milieu])
            {
                return true;
            }
            else if (mot.ToUpper().CompareTo(dictionnaire[milieu]) < 0)
            {
                return RechDichoRecursif(mot, milieu - 1, debut);
            }
            else
            {
                return RechDichoRecursif(mot, fin, milieu + 1);
            }
        }


        /// <summary>
        /// Echange deux mots (en place) dans une liste
        /// </summary>
        /// <param name="index1">rang du 1er mot à échanger</param>
        /// <param name="index2">rang du 2ème mot à échanger</param>
        void Echanger(int index1, int index2)
        {
            string temp = dictionnaire[index1];
            dictionnaire[index1] = dictionnaire[index2];
            dictionnaire[index2] = temp;
        }


        /// <summary>
        /// Renvoie l'index du pivot necessaire pour le Tri Quick Sort
        /// </summary>
        /// <param name="debut"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        int IndexPivot(int debut, int fin)
        {
            string pivot = dictionnaire[fin];
            int i = debut - 1;
            for (int j = debut; j < fin; j++)
            {
                if (dictionnaire[j].CompareTo(pivot) < 0)
                {
                    i++;
                    Echanger(i, j);
                }
            }
            Echanger(i + 1, fin);
            return i + 1;
        }


        /// <summary>
        /// Tri une liste (en place) par ordre alphabetique avec la méthode Quick Sort
        /// </summary>
        /// <param name="debut">rang du début du dictionnaire (par défaut à 0)</param>
        /// <param name="fin">rang de la fin du dictionnaire (longueur du dictionnaire - 1)</param>
        void TriQuickSort(int fin, int debut = 0)
        {
            if (debut < fin)
            {
                int pivot = IndexPivot(debut, fin);
                TriQuickSort(pivot - 1, debut);
                TriQuickSort(fin, pivot + 1);
            }
        }
    }
}
