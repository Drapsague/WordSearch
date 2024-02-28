using static System.Formats.Asn1.AsnWriter;

namespace mots_glisses
{
    public class Jeu
    {
        Dictionnaire dictionnaire;
        Plateau plateau;
        List<Joueur> joueurs;
        string dureeTour;
        int nbTours;


        /// <summary>
        /// Constructeur 1 Jeu : partie basée sur un plateau généré aléatoirement
        /// </summary>
        /// <param name="nbTours">nombre de tours dans la partie</param>
        /// <param name="difficulte">difficulte de la partie (1-3), définie la taille du plateau et le temps de jeu par tour</param>
        /// <param name="langue">langue des mots du plateau / dictionnaire (FR ou EN)</param>
        public Jeu(int nbTours, int difficulte, string langue = "FR")
        {
            this.dictionnaire = new Dictionnaire(langue);
            this.nbTours = nbTours;
           
            creerJoueurs();

            // Permet de faire passer les tours
            for (int numTour = 0; numTour < nbTours; numTour++)
            {
                // Crée un nouveau plateau à chaque tour basé sur la difficulté
                switch (difficulte)
                {
                    case 1:
                        this.plateau = new Plateau(4, 4);
                        this.dureeTour = "0:1:0";
                        break;

                    case 2:
                        this.plateau = new Plateau(6, 6);
                        this.dureeTour = "0:0:45";
                        break;

                    case 3:
                        this.plateau = new Plateau(8, 8);
                        this.dureeTour = "0:0:30";
                        break;
                }
                Tour(numTour);
            }
            // Affichage de fin de partie
            FinPartie();
        }


        /// <summary>
        /// Constructeur 2 Jeu : partie basée sur des fichiers CSV de sauvegarde de plateaux
        /// </summary>
        /// <param name="dureeTour">temps dont le joueur dispose pour trouver un mot</param>
        /// <param name="fichiersPlateaux">liste de fichiers CSV contenant des plateaux</param>
        /// <param name="langue">langue du jeu / dictionnaire</param>
        public Jeu(string dureeTour, List<string> fichiersPlateaux, string langue = "FR")
        {            
            this.dictionnaire = new Dictionnaire(langue);
            // Format d'un TimeSpan --> hh:mm:ss sauf que l'input ici n'est que mm:ss donc on rajoute hh:
            this.dureeTour = "0:" + dureeTour;
            
            creerJoueurs();
            // Le nombre de rounds dépend du nombre de plateaux
            this.nbTours = fichiersPlateaux.Count;

            // Permet de faire passer les tours
            for (int numTour = 0; numTour < nbTours; numTour++)
            {
                // Lit un nouveau plateau à chaque tour
                this.plateau = new Plateau(fichiersPlateaux[numTour]);
                Tour(numTour);
            }
            // Affichage de fin de partie
            FinPartie();
        }


        /// <summary>
        /// Crée les joueurs
        /// </summary>
        void creerJoueurs()
        {
            Console.Clear();
            this.joueurs = new List<Joueur>();

            // Saisie sécurisée du nombre de joueurs
            int nbJoueurs;
            do
            {
                Console.Write("Entrer le nombre de joueurs (> 1) : ");
                try
                {
                    nbJoueurs = int.Parse(Console.ReadLine());

                    if (nbJoueurs < 2)
                    {
                        Console.WriteLine("Il n'y a pas assez de joueurs");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("La valeur entrée n'est pas un nombre entier");
                    nbJoueurs = -1;
                }
            } while (nbJoueurs < 2);

            // Choix du nom des joueurs
            for (int i = 1; i <= nbJoueurs; i++)
            {
                Console.Clear();
                Console.Write("Entrer le nom du joueur " + i + " : ");
                // Saisie sécurisée du nom des joueurs
                string nom;
                do
                {
                    nom = Console.ReadLine();
                    if (nom == null || nom.Length == 0)
                    {
                        Console.WriteLine("Le nom du joueur doit être d'au moins un caractère");
                    }
                } while (nom == null || nom.Length == 0);
                this.joueurs.Add(new Joueur(nom));                
            }
        }


        /// <summary>
        /// Lance un tour
        /// </summary>
        /// <param name="numTour">numéro du tour actuel</param>
        void Tour(int numTour)
        {
            Console.Clear();
            Console.WriteLine("Tour n°" + numTour);
            // Permet d'itérer sur chaque joueur sans connaitre à l'avance le nombre d'itérations
            int indexJoueur = 0;
            // Permet aux joueurs d'arrêter la partie lorsqu'il n'y a plus aucun mot
            bool stopRound = false;

            while (plateau.NbLettresRestantes > 2 && !stopRound)
            {
                Console.Clear();

                // Joueur à qui c'est le tour
                Joueur joueurActuel = joueurs[indexJoueur % joueurs.Count];
                indexJoueur++;                
                
                // Création d'un TimeSpan correspondant au temps alloué au jour pour trouver un mot
                double stop = TimeSpan.Parse(dureeTour).TotalMinutes;
                TimeSpan finTour = TimeSpan.FromMinutes(stop);
                DateTime debut = DateTime.Now;

                // Vérification que la taille du mot est supérieure à 2
                string mot = SaisieStringSecu(joueurActuel, finTour, debut);
                
                // Pour changer de round s'il n'y a plus de mots à trouver
                if (mot == "EXIT")
                {
                    return;
                }


                // Vérifie si temps écoulé ne dépasse pas le temps alloué à un tour
                if (DateTime.Now - debut < finTour)
                {
                    // On vérifie si le mot appartient au dictionnaire Français
                    if (dictionnaire.RechDichoRecursif(mot.ToUpper(), dictionnaire.Dico.Count))
                    {
                        // On vérifie que le joueur n'a pas déjà trouvé le mot
                        if (!joueurActuel.MotsTrouves.Contains(mot))
                        {
                            // On vérifie si le mot appartient à la grille (un mot possède au moins 2 lettres)
                            List<int[]> listeCoord = plateau.RechercheMot(mot.ToLower());
                            if (listeCoord.Count > 1)
                            {
                                // Suppression du mot dans le tableau
                                plateau.MajPlateau(listeCoord);

                                // On diminue le nombre de lettres restantes sur le plateau du nombre de lettres du mot
                                // plateau.NbLettresRestantes -= mot.Length;
                                plateau.NbLettresRestantes -= listeCoord.Count;

                                // Calcul et incrément du score du joueur
                                int points = CalculerScore(mot);
                                joueurActuel.AddScore(points);

                                Console.Clear();
                                Console.WriteLine(String.Format("\nBravo {0} tu as trouvé le mot '{1}' ce qui t'a rapporté {2} points", joueurActuel.Nom, mot, points));
                                Thread.Sleep(4000);
                            }
                            else
                            {
                                Console.WriteLine("Le plateau ne contient pas ce mot");
                                Thread.Sleep(4000);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Vous avez déjà trouvé ce mot");
                            Thread.Sleep(4000);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Le mot entré n'est pas un mot français");
                        Thread.Sleep(4000);
                    }
                }
                else
                {
                    Console.WriteLine("Le temps est écoulé, le mot entré n'a pas été pris en compte");
                    Thread.Sleep(4000);
                }
            }
        }


        /// <summary>
        /// Saisie sécurisée d'un mot qui doit être de longueur 2 au moins
        /// </summary>
        /// <param name="joueurActuel">joueur dont c'est le tour</param>
        /// <param name="finTour">temps alloué à un tour</param>
        /// <param name="debut">moment auquel on a démarré le timer</param>
        /// <returns>mot entré par l'utilisateur respectant les contraintes</returns>
        string SaisieStringSecu(Joueur joueurActuel, TimeSpan finTour, DateTime debut)
        {
            string input;
            do
            {
                Console.Clear();

                // On affiche les infos sur le tour du joueur ainsi que le temps restant mis à jour
                Console.WriteLine(String.Format("Tour de : {0}\nScore : {1}", joueurActuel.Nom, joueurActuel.Score));
                TimeSpan tempsRestant = finTour - (DateTime.Now - debut);
                Console.Write(String.Format($"Il te reste {tempsRestant.Minutes} minutes et {tempsRestant.Seconds} secondes pour jouer\n\n{plateau.Afficher()}\n\n"));
                
                Console.Write("Entrer un mot : ");
                input = Console.ReadLine();

                // Cas où joueur veut passer au tour suivant
                if (input.ToUpper() == "EXIT") { return "EXIT"; }

                // On recalcule le temps restant après que l'utilisateur ait rentré un mot
                tempsRestant = finTour - (DateTime.Now - debut);
                // On vérifie que le temps ne soit pas dépassé auquel cas on ne redemande pas une saisie même si le mot a une taille inférieure à 2
                if (tempsRestant.TotalSeconds <= 0) { return ""; }

                // Mot doit avoir une longueur supérieure à 2
                if (input.Length < 2)
                {
                    Console.WriteLine("Le mot n'est pas au moins de taille 2");
                }
            } while (input.Length < 2);

            return input;
        }


        /// <summary>
        /// Calcul du score dépendant de la longueur du mot et du poids des lettres le composant
        /// </summary>
        /// <param name="mot">mot trouvé par le joueur dont on calcule le score associé</param>
        /// <returns>score correspondant au mot</returns>
        int CalculerScore(string mot)
        {
            int score = 0;
            foreach(char lettre in mot)
            {
                int indexLettre = Char.ToLower(lettre) - 'a';
                score += plateau.PoidsMots[indexLettre];
            }
            return score + mot.Length;
        }


        /// <summary>
        /// Affiche fin de partie avec leaderboard du top 10 des joueurs
        /// </summary>
        void FinPartie()
        {
            Console.Clear();
            Console.WriteLine($"LA PARTIE EST FINIE\n");
            
            // On trie les joueurs selon leur score
            List<Joueur> leaderboard = joueurs;
            for (int i = 0; i < leaderboard.Count; i++)
            {
                for (int j = 0; j < leaderboard.Count - 1; j++)
                {
                    if (leaderboard[j].Score < leaderboard[j + 1].Score)
                    {
                        Joueur temp = leaderboard[j];
                        leaderboard[j] = leaderboard[j + 1];
                        leaderboard[j + 1] = temp;
                    }
                }
            }

            string res = $"==================================\n          TOP 10 LEADERBOARD          \n==================================\n\n";
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    res += $"{i + 1}. {joueurs[i].Nom} - {joueurs[i].Score} Points\n";
                }
                catch
                {
                    res += $"{i + 1}. N/A\n";
                }
            }
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }
}
