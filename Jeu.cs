namespace mots_glisses
{
    internal class Jeu
    {
        Dictionnaire dictionnaire;
        Plateau plateau;
        List<Joueur> joueurs;
        int difficulte;
        string dureeTour;
        int nbRounds;

        /// <summary>
        /// Constructeur pour une partie basée sur un plateau générée aléatoirement
        /// </summary>
        /// <param name="nbLignes"></param>
        /// <param name="nbColonnes"></param>
        /// <param name="langue"></param>
        public Jeu(string dureeTour, int nbRounds, int difficulte, string langue)
        {
            this.dictionnaire = new Dictionnaire(langue);
            this.dureeTour = dureeTour;
            this.nbRounds = nbRounds;
            this.difficulte = difficulte;
           
            creerJoueurs();

            for (int r = 0; r < nbRounds; r++)
            {
                switch (difficulte)
                {
                    case 1:
                        this.plateau = new Plateau(4, 4);
                        break;

                    case 2:
                        this.plateau = new Plateau(6, 6);
                        break;

                    case 3:
                        this.plateau = new Plateau(8, 8);
                        break;
                }
                Round();
            }
        }


        /// <summary>
        /// Constructeur pour une partie basée sur un fichier
        /// </summary>
        /// <param name="fichierSauvegarde"></param>
        /// <param name="langue"></param>
        public Jeu(string fichierSauvegarde, string langue)
        {
            // Liste de plateaux pour la partie (donc de fichiers de sauvegarde)
            this.dictionnaire = new Dictionnaire(langue);
            this.plateau = new Plateau(fichierSauvegarde);
            creerJoueurs();
        }


        /// <summary>
        /// Créer les joueurs
        /// </summary>
        void creerJoueurs()
        {
            this.joueurs = new List<Joueur>();
            int nbJoueurs;
            do
            {
                Console.WriteLine("Entrer le nombre de joueurs (> 1) :");
                try
                {
                    nbJoueurs = int.Parse(Console.ReadLine());

                    if (nbJoueurs < 2)
                    {
                        Console.WriteLine("Il n'y a pas assez de joueurs");
                    }
                }
                catch (FormatException e)
                {
                    Console.WriteLine("La valeur entrée n'est pas un nombre");
                    nbJoueurs = -1;
                }
            } while (nbJoueurs < 2);

            for (int i = 0; i < nbJoueurs; i++)
            {
                Console.WriteLine("Entrer le nom du joueur " + i + "\n");
                this.joueurs.Add(new Joueur(Console.ReadLine()));
            }
        }


        void Round()
        {
            foreach (Joueur joueur in joueurs)
            {
                Console.WriteLine("Tour de " + joueur.Nom);

                double stop = TimeSpan.Parse(dureeTour).TotalMinutes;
                TimeSpan intStop = TimeSpan.FromMinutes(stop);
                DateTime debut = DateTime.Now;
                DateTime actuel = DateTime.Now;

                // Durée écoulée
                TimeSpan interval = actuel - debut;
                while (interval < intStop)
                {
                    Console.WriteLine(interval.ToString());
                    actuel = DateTime.Now;
                    interval = actuel - debut;
                }
                Console.WriteLine(interval);
            }
        }


        void calculerScore()
        {

        }
    }
}
