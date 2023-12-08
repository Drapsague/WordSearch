namespace mots_glisses
{
    internal class Jeu
    {
        Dictionnaire dictionnaire;
        Plateau plateau;
        List<Joueur> joueurs;
        int difficulte;
        int dureeTour;
        int nbRounds;

        /// <summary>
        /// Constructeur pour une partie basée sur un plateau générée aléatoirement
        /// </summary>
        /// <param name="nbLignes"></param>
        /// <param name="nbColonnes"></param>
        /// <param name="langue"></param>
        public Jeu(int dureeTour, int nbRounds, int difficulte, string langue)
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

                DateTime debut = DateTime.Now;
                DateTime actuel = DateTime.Now;

                /// Durée écoulée
                TimeSpan interval = actuel - debut;

                /// On vérifie que le nombre de secondes écoulées est inférieur à la durée de jeu (en secondes aussi)
                /*
                while ((interval.TotalSeconds < stop) && (joueur.MotsTrouves.Count < plateau.MotsRecherches.Count))

                {

                    Console.WriteLine(plateau.afficherPlateau());

                    Console.WriteLine(joueur.ToString());

                    Console.WriteLine("Tu as trouvé " + joueur.MotsTrouves.Count + " mots sur " + plateau.MotsRecherches.Count);

                    string mot, direction;

                    int ligne = -1, colonne = -1;

                    Console.WriteLine("Le temps écoulé est supérieur à " + (int)interval.TotalSeconds / 60 + " minutes et " + (int)interval.TotalSeconds % 60 + " secondes");

                    Console.WriteLine("Tu as " + (int)stop / 60 + " minutes et " + (int)stop % 60 + " secondes pour trouver tous les mots");
                }
                */
            }
        }


        void calculerScore()
        {

        }
    }
}
