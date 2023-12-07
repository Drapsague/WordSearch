namespace mots_glisses
{
    internal class Jeu
    {
        Dictionnaire dictionnaire;
        Plateau plateau;
        List<Joueur> joueurs;

        /// <summary>
        /// Constructeur pour une partie basée sur un plateau générée aléatoirement
        /// </summary>
        /// <param name="nbLignes"></param>
        /// <param name="nbColonnes"></param>
        /// <param name="langue"></param>
        public Jeu(int nbLignes, int nbColonnes, string langue)
        {
            this.dictionnaire = new Dictionnaire(langue);
            this.plateau = new Plateau(nbLignes, nbColonnes);
            creerJoueurs();
        }

        /// <summary>
        /// Constructeur pour une partie basée sur un fichier
        /// </summary>
        /// <param name="fichierSauvegarde"></param>
        /// <param name="langue"></param>
        public Jeu(string fichierSauvegarde, string langue)
        {
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


        void Start()
        {
            foreach(Joueur joueur in joueurs)
            {

            }
        }


        void calculerScore()
        {

        }
    }
}
