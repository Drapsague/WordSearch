using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mots_glisses
{
    internal class Menu
    {
        readonly string PATH = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;


        /// <summary>
        /// Constructeur du Menu
        /// </summary>
        public Menu() 
        {      
            // Affiche le message d'accueil avec l'animation de glissement
            Welcome();

            // Lance le menu principal
            MenuPrincipal();
        }

        
        /// <summary>
        /// Message d'accueil avec animation de glissement
        /// </summary>
        /// <param name="filename">fichier contenant le message d'accueil</param>
        void Welcome(string filename = "welcome.txt")
        {
            Console.CursorVisible = false;
            string fullname = PATH + "\\" + filename;
            string[] welcomeMsg;
            try
            {
                welcomeMsg = File.ReadAllLines(fullname);
            }
            catch
            {
                welcomeMsg = null;
                Console.Clear();
                Console.WriteLine("Problème lors de l'ouverture du fichier : " + filename);
                Thread.Sleep(2000);
                Environment.Exit(1);
            }

            foreach (string line in welcomeMsg)
            {
                if (line.Length != 0)
                {
                    foreach (char lettre in line)
                    {
                        Console.Write(lettre);
                    }
                    Thread.Sleep(100);
                }
                Console.WriteLine();
            }
        }


        /// <summary>
        /// Menu principal
        /// </summary>
        /// <param name="filename">fichier contenant le message d'accueil</param>
        void MenuPrincipal(string filename = "welcome.txt")
        {
            string fullname = PATH + "\\" + filename;
            string welcomeMsg;
            try
            {
                welcomeMsg = File.ReadAllText(fullname);
            }
            catch
            {
                welcomeMsg = "";
                Console.Clear();
                Console.WriteLine("Problème lors de l'ouverture du fichier : " + filename);
                Thread.Sleep(2000);
                Environment.Exit(1);
            }

            // Liste des différentes options sur le menu principal
            string[] mainOptions = { "1 - Commencer une partie aléatoire", "2 - Commencer une partie à partir de fichiers csv", "3 - Lire les règles", "4 - Quitter" };
            int selectedIndex = 0;

            // Cache le curseur de l'utilisateur
            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                // Affiche le message de bienvenue contenu dans le fichier "welcome.txt"
                Console.WriteLine(welcomeMsg);

                // Afficher les options du menu
                for (int i = 0; i < mainOptions.Length; i++)
                {
                    // Surligne en vert l'option sélectionnée
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(mainOptions[i]);
                    Console.ResetColor();
                }

                // Capturer l'entrée de l'utilisateur
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // Déplacer le curseur en fonction de l'entrée de l'utilisateur
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(mainOptions.Length - 1, selectedIndex + 1);
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.ResetColor();

                        // Action en fonction du choix utilisateur
                        string selection = mainOptions[selectedIndex];
                        switch (selection)
                        {
                            case "1 - Commencer une partie aléatoire":
                                JeuAleat();
                                break;

                            case "2 - Commencer une partie à partir de fichiers csv":
                                JeuFichiers();
                                break;

                            case "3 - Lire les règles":
                                // Ecriture des règles en bleu
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.WriteLine(File.ReadAllText(String.Format(@"{0}\regles.txt", PATH)));
                                Console.ResetColor();
                                Console.WriteLine("Appuyer sur 'Entrer' pour continuer");
                                Console.ReadKey();
                                break;

                            case "4 - Quitter":
                                return;
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Menu correspondant à un jeu dont les parties sont basées sur des plateaux générés aléatoirement
        /// </summary>
        void JeuAleat()
        {
            // Choix de la difficulté
            Console.WriteLine("Quelle difficulté souhaitez-vous ? : ");
            int difficulte = ChoixDifficulte();

            // Saisie sécurisée du nombre de tours
            int nbTours;
            do
            {
                Console.Write("Combien de tours souhaitez-vous jouer ? : ");
                string input = Console.ReadLine();
                try
                {
                    nbTours = int.Parse(input);
                    if (nbTours < 1)
                    {
                        Console.WriteLine("Une partie doit durer au moins un round");
                    }
                }
                catch
                {
                    Console.WriteLine("La valeur entrée n'est pas un entier");
                    // Pour que la condition while soit respectée
                    nbTours = 0;
                }
            } while (nbTours < 1);

            Console.Clear();
            // Création d'une partie
            new Jeu(nbTours, difficulte);
        }


        /// <summary>
        /// Menu pour choisir la difficulté de la partie
        /// </summary>
        /// <returns>difficulté entre 1 et 3 avec 3 le plus difficile</returns>
        int ChoixDifficulte()
        {
            // Liste des différentes options sur le menu principal
            string[] mainOptions = { "Niveau 1 - Plateau 4x4 - 1 minute par tour", "Niveau 2 - Plateau 6x6 - 45 secondes par tour", "Niveau 3 - Plateau 8x8 - 30 secondes par tour" };
            int selectedIndex = 0;

            // Cache le curseur de l'utilisateur
            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choisir la difficulté de la partie\n");

                // Afficher les options du menu
                for (int i = 0; i < mainOptions.Length; i++)
                {
                    // Surligne en vert l'option sélectionnée
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(mainOptions[i]);
                    Console.ResetColor();
                }

                // Capturer l'entrée de l'utilisateur
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // Déplacer le curseur en fonction de l'entrée de l'utilisateur
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(mainOptions.Length - 1, selectedIndex + 1);
                        break;
                    case ConsoleKey.Enter:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.ResetColor();

                        // Action en fonction du choix utilisateur
                        string selection = mainOptions[selectedIndex];
                        switch (selection)
                        {
                            case "Niveau 1 - Plateau 4x4 - 1 minute par tour":
                                return 1;

                            case "Niveau 2 - Plateau 6x6 - 45 secondes par tour":
                                return 2;

                            case "Niveau 3 - Plateau 8x8 - 30 secondes par tour":
                                return 3;
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Menu correspondant à un jeu dont les parties sont basées sur des fichiers CSV de sauvegarde de plateaux
        /// </summary>
        void JeuFichiers()
        {
            Console.Clear();

            // Saisie sécurisée du nombre de tours qui va définir le nombre de fichiers CSV de sauvegarde à charger
            int nbTours;
            do
            {
                Console.Write("Combien de tours souhaitez-vous jouer ? : ");
                string input = Console.ReadLine();
                try
                {
                    nbTours = int.Parse(input);
                    if (nbTours < 1)
                    {
                        Console.WriteLine("Une partie doit durer au moins un round");
                    }
                }
                catch
                {
                    Console.WriteLine("La valeur entrée n'est pas un entier");
                    // Pour que la condition while soit respectée
                    nbTours = 0;
                }
            } while (nbTours < 1);

            Console.Clear();

            // Récupérer les fichiers de sauvegarde
            List<string> fichiersPlateaux = new List<string>();
            for (int t = 1; t <= nbTours; t++)
            {
                fichiersPlateaux.Add(OpenFileMenu());
                Console.WriteLine($"Le fichier pour le tour {t} a été chargé avec succès");
                Thread.Sleep(1500);                
            }

            Console.Clear();

            // Choix de la durée d'un tour (minutes)
            int dureeTourMin;
            do
            {
                Console.Write("De combien de minutes souhaitez-vous disposer pour jouer ? : ");
                string input = Console.ReadLine();
                try
                {
                    dureeTourMin = int.Parse(input);
                    if (dureeTourMin < 0)
                    {
                        Console.WriteLine("La durée doit être positive");
                    }
                }
                catch
                {
                    Console.WriteLine("La valeur entrée n'est pas un entier");
                    // Pour que la condition while soit respectée
                    dureeTourMin = -1;
                }
            } while (dureeTourMin < 0 || dureeTourMin > 59);

            Console.Clear();

            // Choix de la durée d'un tour (secondes)
            int dureeTourSec;
            do
            {
                Console.Write("De combien de secondes souhaitez-vous disposer pour jouer ? : ");
                string input = Console.ReadLine();
                try
                {
                    dureeTourSec = int.Parse(input);
                    if (dureeTourSec < 0)
                    {
                        Console.WriteLine("La durée doit être positive");
                    }
                }
                catch
                {
                    Console.WriteLine("La valeur entrée n'est pas un entier");
                    // Pour que la condition while soit respectée
                    dureeTourSec = -1;
                }
            } while (dureeTourSec < 0 || dureeTourSec > 59);

            Console.Clear();

            // Formatage de la durée d'un tour
            string dureeTour = dureeTourMin + ":" + dureeTourSec;
            new Jeu(dureeTour, fichiersPlateaux);
        }


        /// <summary>
        /// Menu d'ouverture des fichiers situés dans le répertoire courant
        /// </summary>
        /// <returns> Nom du fichier CSV choisi par l'utilisateur</returns>
        string OpenFileMenu()
        {
            string fichierJeu = "";
            Dictionary<string, Action> dispatchTable = new Dictionary<string, Action>();
            string[] options = FileList();
            
            // Cas où il n'y a pas de fichiers csv dans le dossier du jeu
            if (options.Length == 0)
            {
                Console.WriteLine("Il n'y a pas de fichiers CSV dans le répertoire du jeu");
                Thread.Sleep(3000);
                Environment.Exit(1);
            }
            int selectedIndex = 0;

            for (int i = 0; i < options.Length; i++)
            {
                int index = i;
                dispatchTable.Add(options[i], () =>
                {
                    fichierJeu = options[index];
                });
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choisissez un fichier à ouvrir parmi les suivants (dans le dossier du jeu) :\n");

                // Afficher les options du menu
                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(options[i]);
                    Console.ResetColor();
                }

                // Capturer l'entrée de l'utilisateur
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                // Déplacer le curseur en fonction de l'entrée de l'utilisateur
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(options.Length - 1, selectedIndex + 1);
                        break;

                    case ConsoleKey.Enter:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.ResetColor();

                        string selection = options[selectedIndex];
                        if (dispatchTable.ContainsKey(selection))
                        {
                            dispatchTable[selection]();
                        }
                        return fichierJeu;
                }
            }
        }


        /// <summary>
        /// Lister les fichiers CSV dans le répertoire courant (celui du jeu)
        /// </summary>
        /// <returns>Liste des noms des fichiers CSV dans le répertoire courant</returns>
        string[] FileList()
        {
            string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\";
            
            // Liste des fichiers csv dans le répertoire courant
            string[] files = Directory.GetFiles(@path, "*.csv");
            
            // On supprime l'extension .csv du nom
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace(@path, "");
            }
            return files;
        }
    }
}
