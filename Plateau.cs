using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mots_glisses
{
    public class Plateau
    {
        readonly string PATH = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        char[,] plateau;
        int nbLignes;
        int nbColonnes;
        int[] poidsMots;
        int[] maxOccurences;
        int nbLettresRestantes;


        /// <summary>
        /// Constructeur 1 de Plateau : aléatoire
        /// </summary>
        /// <param name="nbLignes">nombre de lignes du plateau</param>
        /// <param name="nbColonnes">nombre de colonnes du plateau</param>
        /// <param name="fichierContraintes">fichier contenant les contraintes d'occurences maximales et le poids des mots pour le calcul du score</param>
        public Plateau(int nbLignes, int nbColonnes, string fichierContraintes = "Lettre.txt")
        {
            this.nbLignes = nbLignes;
            this.nbColonnes = nbColonnes;
            this.nbLettresRestantes = this.nbLignes * this.nbColonnes;

            ReadContraintes(fichierContraintes);
            GenAleat();
        }


        /// <summary>
        /// Constructeur 2 de Plateau : à partir de fichiers de sauvegarde CSV
        /// </summary>
        /// <param name="fichierSauvegarde">fichier CSV contenant la sauvegarde d'un plateau</param>
        /// <param name="fichierContraintes">fichier contenant les contraintes d'occurences maximales et le poids des mots pour le calcul du score</param>
        public Plateau(string fichierSauvegarde, string fichierContraintes = "Lettre.txt")
        {
            ToRead(fichierSauvegarde);
            ReadContraintes(fichierContraintes);
            this.nbLettresRestantes = this.nbLignes * this.nbColonnes;
        }


        public int NbLignes {  get { return nbLignes; } }
        public int NbColonnes { get { return nbColonnes; } }
        public int[] PoidsMots { get { return poidsMots; } }
        public int[] MaxOccurences { get { return maxOccurences; } }
        public int NbLettresRestantes { get { return nbLettresRestantes; } set { if (value < 0) nbLettresRestantes = value; } }


        /// <summary>
        /// Lit les contraintes d'occurences et de poids sur les lettres
        /// </summary>
        /// <param name="filename">fichier contenant les contraintes</param>
        void ReadContraintes(string filename)
        {
            string fullname = String.Format("{0}\\{1}", this.PATH, filename);
            string[] lines;
            try
            {
                lines = File.ReadAllLines(fullname);
            }
            catch
            {
                lines = null;
                Console.Clear();
                Console.WriteLine("Problème lors de l'ouverture du fichier de contraintes : " + filename);
                Thread.Sleep(2000);
                Environment.Exit(1);
            }

            // Vérifie qu'il y a bien autant de lignes dans le fichier que de lettres dans l'alphabet
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
                Console.Clear();
                Console.WriteLine("Le fichier doit contenir exactement 26 lignes");
                Thread.Sleep(2000);
                Environment.Exit(1);
            }
        }


        /// <summary>
        /// Génère une matrice de caractères (char) aléatoires
        /// </summary>
        void GenAleat()
        {
            this.plateau = new char[nbLignes, nbColonnes];
            Random rnd = new Random();

            // Stocke le nombre d'occurences pour chaque lettre
            int[] occurences = new int[26];

            int index = 0;
            while (index < nbLignes * nbColonnes)
            {
                char randomChar = (char)('a' + rnd.Next(0, 26));
                // Vérifie si le nombre d'occurences de la lettre choisie aléatoirement
                // dans le plateau est inférieur au nombre max d'occurences pour cette lettre
                // Si c'est le cas on l'ajoute au plateau sinon on tire au sort une autre lettre
                int ascii_conversion = (int)randomChar - 97;
                if (occurences[ascii_conversion] < maxOccurences[ascii_conversion])
                {
                    int i = index / nbColonnes;
                    int j = index % nbColonnes;
                    this.plateau[i, j] = randomChar;
                    occurences[ascii_conversion]++;
                    index++;
                }
            }
        }


        /// <summary>
        /// Récupère un plateau à partir d'un fichier CSV
        /// </summary>
        /// <param name="filename"></param>
        void ToRead(string filename)
        {
            string fullname = this.PATH + "\\" + filename;

            string[] lines;
            try
            {
                lines = File.ReadAllLines(fullname);
            }
            catch
            {
                lines = null;
                Console.Clear();
                Console.WriteLine("Problème lors de l'ouverture du fichier CSV de sauvegarde : " + filename);
                Thread.Sleep(2000);
                Environment.Exit(1);
            }
            
            this.nbLignes = lines.Length;
            this.nbColonnes = lines[0].Split(";").Length;
            this.plateau = new char[nbLignes, nbColonnes];

            // On itère sur chaque ligne et on récupère la valeur de chaque case du CSV (séparé par un ;) qui correspond à un char de notre plateau
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
        /// Retourne la difficulté, le nombre de lignes, de colonnes et le nombre de mots à trouver dans le plateau
        /// </summary>
        /// <returns>String contenant les caractéristiques du plateau</returns>
        public string toString()
        {
            return String.Format("Nombre de lignes : {0}\nNombre de colonnes : {1}\n", this.nbLignes, this.nbColonnes);
        }


        /// <summary>
        /// Sauvegarde le plateau actuel dans un fichier CSV
        /// </summary>
        /// <param name="filename">nom du fichier de sauvegarde CSV</param>
        public void ToFile(string filename)
        {
            string fullname = this.PATH + "\\" + filename;
            string[] lines = new string[this.nbLignes];

            for (int i = 0; i < nbLignes; i++)
            {
                string data = "";
                for (int j = 0; j < nbColonnes - 1; j++)
                {
                    // Accès à l'indice 0 car chaque string est une lettre et ainsi on cast cette string en un char
                    data += this.plateau[i, j] + ";";
                }
                data += plateau[i, nbColonnes - 1];
                lines[i] = data;
            }

            try
            {
                File.WriteAllLines(fullname, lines);
                Console.WriteLine("Plateau enregistré avec succès");
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Problème lors de l'enregistrement du fichier CSV de sauvegarde : " + filename);
                Thread.Sleep(2000);
                Environment.Exit(1);
            }
        }


        /// <summary>
        /// Affichage stylisé du plateau
        /// </summary>
        /// <returns>String contenant un affichage stylisé du plateau</returns>
        public string Afficher()
        {
            string res = "";
            for (int i = 0; i < nbLignes; i++)
            {
                // Mise en forme des interlignes/intercolonnes +---
                res += String.Concat(Enumerable.Repeat("+---", nbColonnes)) + "+\n";

                for (int j = 0; j < nbColonnes; j++)
                {
                    res += String.Format("| {0} ", plateau[i, j]);
                }
                res += "|\n";
            }
            // Mise en forme des interlignes/intercolonnes +---
            res += String.Concat(Enumerable.Repeat("+---", nbColonnes)) + "+\n";
            return res;
        }


        /// <summary>
        /// Supprime le mot et réarrange le plateau en conséquence en faisant descendre les lettres à la manière d'un Puissance 4
        /// </summary>
        /// <param name="listeCoord">Liste des coordonnées des lettres du mot à supprimer</param>
        public void MajPlateau(List<int[]> listeCoord)
        {
            // Supprime lettres du mot, du plateau
            foreach (int[] coord in listeCoord)
            {
                plateau[coord[0], coord[1]] = ' ';
            }

            // On itère sur chaque ligne en partant du bas (car lettres supprimées sont surtout en bas du plateau)
            // A chaque fois qu'on trouve une case dont la lettre a été supprimée, on itère sur toutes les lignes sans changer la colonne (donc sur les cases au-dessus de celle qui a été supprimée)
            // On regarde s'il y a d'autres lettres qui ont été supprimées juste au-dessus de la notre, si oui alors cela signifie que l'on devra décaler les lettres de la colonne du nombre de lettres supprimées en plus de celle supprimée au départ
            for (int i = nbLignes - 1; i >= 0; i--)
            {
                for (int j = 0; j < nbColonnes; j++)
                {
                    // Vérifie si la case contient une lettre ou si elle a été supprimée
                    if (plateau[i, j] == ' ')
                    {
                        // On itère sur les lettres se situant au-dessus de celle supprimée
                        for (int k = i; k > 0; k--)
                        {
                            // Par défaut le décalage à réaliser est de 1 car c'est le nombre de lettres supprimées
                            int decalageHaut = 1;
                            // k - decalageHaut > 0 permet de s'arrêter quand on atteint la boardure haute du tableau
                            // On vérifie si la lettre du dessus a été supprimée et si c'est le cas on augmente le décalage
                            while (k - decalageHaut > 0 && plateau[k - decalageHaut, j] == ' ')
                            {
                                decalageHaut++;
                            }
                            // On copie la lettre la plus en bas de la colonne (qui n'a pas été supprimée) à l'endroit de la lettre supprimée
                            plateau[k, j] = plateau[k - decalageHaut, j];
                            // On supprime ensuite l'ancien emplacement de cette lettre
                            plateau[k - decalageHaut, j] = ' ';
                        } 
                        // INUTILE : plateau[0, j] = ' ';
                    }
                }
            }
        }

        
        /// <summary>
        /// Permet de récupérer la liste des coordonnées d'un mot s'il est dans le plateau
        /// </summary>
        /// <param name="mot">Mot recherché</param>
        /// <returns>Liste des coordonnées du mot, qui est vide si ce mot n'est pas dans le plateau</returns>
        public List<int[]> RechercheMot(string mot)
        {
            mot = mot.ToLower();

            // Itère sur les colonnes en fixant la ligne à celle du bas car les mots y ont toujours leur première lettre
            for (int y = 0; y < nbColonnes; y++)
            {
                // Si la 1ère lettre est trouvée on lance la récursion
                if (plateau[nbLignes - 1, y] == mot[0])
                {
                    return Recursion(mot, 0, nbLignes - 1, y);
                }
            }
            // Sinon renvoie une liste de coordonnées vide si le mot n'est pas dans le tableau
            // Si la ligne du bas ne contient pas sa 1ère lettre, il n'y est pas
            return new List<int[]>();
        }


        /// <summary>
        /// Fonction récursive permettant de retourner le chemin parcouru pour trouver un mot, sous forme d'une liste de coordonnées, si le mot est trouvé
        /// </summary>
        /// <param name="mot">Mot à chercher</param>
        /// <param name="indiceLettre">rang ou indice de la lettre du mot que nous cherchons dans le tableau au cours de cette récursion</param>
        /// <param name="x">ligne à laquelle on se situe lors de cette récursion</param>
        /// <param name="y">colonne à laquelle on se situe lors de cette récursion</param>
        /// <returns>Liste des coordonnées du mot s'il est trouvé sinon une liste vide</returns>
        List<int[]> Recursion(string mot, int indiceLettre, int x, int y)
        {
            // Cas où l'on sort du plateau ou alors on est allé trop loin dans la récursion (plus loin que la taille du mot recherché)
            if (x < 0 || x > nbLignes - 1 || y < 0 || y > nbColonnes - 1 || indiceLettre > mot.Length - 1)
            {
                return new List<int[]>();
            }

            // Vérifie si la lettre que nous cherchons dans le mot est égale à celle où nous sommes dans le plateau
            if (plateau[x, y] == mot[indiceLettre])
            {
                // On vérifie si la lettre trouvée est la dernière lettre du mot, auquel cas on a trouvé le mot et on remonte la pile d'appels
                if (indiceLettre == mot.Length - 1)
                {
                    return new List<int[]> { new int[] { x, y } };
                }

                // Sinon on continue la recherche dans toutes les directions

                // Recherche vers la gauche
                List<int[]> gauche = Recursion(mot, indiceLettre + 1, x, y - 1);
                if (gauche.Count > 0)
                {
                    return new List<int[]> { new int[] { x, y } }.Concat(gauche).ToList();
                }

                // Recherche vers le haut à gauche
                List<int[]> hautGauche = Recursion(mot, indiceLettre + 1, x - 1, y - 1);
                if (hautGauche.Count > 0)
                {
                    return new List<int[]> { new int[] { x, y } }.Concat(hautGauche).ToList();
                }

                // Recherche vers le haut
                List<int[]> haut = Recursion(mot, indiceLettre + 1, x - 1, y);
                if (haut.Count > 0)
                {
                    return new List<int[]> { new int[] { x, y } }.Concat(haut).ToList();
                }

                // Recherche vers le haut à droite
                List<int[]> hautDroite = Recursion(mot, indiceLettre + 1, x - 1, y + 1);
                if (hautDroite.Count > 0)
                {
                    return new List<int[]> { new int[] { x, y } }.Concat(hautDroite).ToList();
                }

                // Recherche vers la droite
                List<int[]> droite = Recursion(mot, indiceLettre + 1, x, y + 1);
                if (droite.Count > 0)
                {
                    return new List<int[]> { new int[] { x, y } }.Concat(droite).ToList();
                }
            }
            // Sinon on va dans la mauvaise direction, on renvoie une liste vide
            return new List<int[]>();
        }
    }
}
