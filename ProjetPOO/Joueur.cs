using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;


namespace ProjetPOO
{
    public class Joueur
    {
        private string nom;
        private int score = 0;
        private List<string> motUsee = new List<string>();

        public Joueur(string nom)
        {
            this.nom = nom.ToUpper();
        }

        public string Nom
        {
            get { return nom; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public List<string> MotUsee
        {
            get { return motUsee; }
        }

        public bool Contain(string mot)
        {
            if(motUsee != null)
            {
                return motUsee.Contains(mot);
            }
            else
            {
                return false;
            }
            
        }

        public void Add_Mot(string mot)
        {
            motUsee.Add(mot);
        }

        public string toString()
        {
            string message = "Le score de " + score + " est de " + this.nom + " grâce aux mots suivants :\n";

            for(int i = 0; i < motUsee.Count; i++)
            {
                message += motUsee[i] + " ";
            }

            return message;
        }

    }
}
