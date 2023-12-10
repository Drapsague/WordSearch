namespace mots_glisses
{
    public class Joueur
    {
        string nom;
        List<string> motsTrouves;
        int score = 0;


        /// <summary>
        /// Constructeur de Joueur
        /// </summary>
        /// <param name="nom">Nom du joueur</param>
        public Joueur(string nom)
        {
            if (nom != null && nom.Length != 0)
            {
                this.nom = nom;
                this.motsTrouves = new List<string>();
                this.score = 0;
            }
            else
            {
                // Si nom n'est pas correct alors on choisit aléatoirement un nom pour le joueur
                Random rnd = new Random();
                string chars = "abcdefghijklmnopqrstuvwxyz";
                int wordLength = rnd.Next(5);
                string rndNom = "";
                
                for (int i = 0; i < wordLength; i++)
                {
                    rndNom += chars[rnd.Next(0, 26)];
                }

                Console.WriteLine("Le nom entré n'est pas correct, le joueur s'appellera donc " + rndNom);
                Thread.Sleep(2000);
            }
            
        }


        public string Nom
        {
            get { return this.nom; }
        }

        public List<string> MotsTrouves
        {
            get { return this.motsTrouves; }
        }

        public int Score
        {
            get { return this.score; }
        }


        /// <summary>
        /// Ajoute un mot à la liste des mots trouvés par le joueur
        /// </summary>
        /// <param name="mot">Mot trouvé par le joueur</param>
        public void AddMot(string mot)
        {
            motsTrouves.Add(mot);
        }


        /// <summary>
        /// Incremente le score du joueur
        /// </summary>
        /// <param name="val">Valeur dont le score augmente</param>
        public void AddScore(int val)
        {
            if (val > 0)
            {
                this.score += val;
            }
        }


        /// <summary>
        /// Vérifie si un mot a deja été trouvé par le joueur
        /// </summary>
        /// <param name="mot">Mot à chercher parmi la liste des mots trouvés par le joueur</param>
        /// <returns>Si oui ou non le mot a été trouvé</returns>
        public bool Contient(string mot)
        {
            return motsTrouves.Contains(mot);
        }


        /// <summary>
        /// Renvoie les caractéristiques du joueur : son nom, son score et les mots qu'il a trouvé
        /// </summary>
        /// <returns>String contenant les caractéristiques du joueur</returns>
        public string toString()
        {
            string temp = "";
            for (int i = 0; i < motsTrouves.Count - 1; i++)
            {
                temp += motsTrouves[i] + ", ";
            }
            temp += motsTrouves[motsTrouves.Count - 1];

            return String.Format("Le nom du joueur est : {0}, il a un score de {1} et a trouvé les mots suivants : {2}", this.nom, this.score, temp);
        }
    }
}
