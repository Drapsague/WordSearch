using System;



namespace mots_glisses
{
    public class Joueur
    {
        string nom;
        List<string> motsTrouves;
        int score = 0;


        //Constructeur
        public Joueur(string nom, int score)
        {
            if (nom != null)
            {
                this.nom = nom;
                this.motsTrouves = new List<string>();
                this.score = score;

            }else
            {
                Console.WriteLine( "Erreur Nom");
            }
            
        }
        /// <summary>
        /// Ajoute les mots trouvés dans une liste
        /// </summary>
        /// <param name="mot"></param>
        public void Add_Mot(string mot)
        {
            motsTrouves.Add(mot);
        }

        /// <summary>
        /// Incremente le score du joueur
        /// </summary>
        /// <param name="val"></param>
        public void Add_Score(int val)
        {
            this.score += val;
        }

        /// <summary>
        /// Vérifie si un mot a deja été trouvé
        /// </summary>
        /// <param name="mot"></param>
        /// <returns></returns>
        public bool Contient(string mot)
        {
            return motsTrouves.Contains(mot) ? true : false;
        }



        /// <summary>
        /// Renvoie les caractéristique du joueur : son nom, son score et les mots qu'il a trouvé
        /// </summary>
        /// <returns></returns>
        public string toString()
        {
            string temp = "";
            for (int i = 0; i < motsTrouves.Count - 1; i++)
            {
                temp += motsTrouves[i] + ", ";
            }
            temp += motsTrouves[motsTrouves.Count];

            return String.Format("Le nom du joueur est : {0}, il a un score de {1} et a trouvé les mots suivant : {2}", this.nom, this.score, temp);

        }





    }

}
