using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;


namespace ProjetPOO
{
    public class Dictionnaire
    {
        List<string> dico;
        int longueur;
        string langage = "Fançais";

        public Dictionnaire(List<string> dico,int longueur,string langage)
        {
            this.dico = dico;
            this.longueur = longueur;
            this.langage = langage;
        }

        public string Langage
        {
            get { return langage; }
        }

        public int Longueur
        {
            get { return longueur; }
        }

        public List<string> Dico
        {
            get { return dico; }
        }

        public string toString()
        {
            return "Les mots sont en " + langage + " composé de " + longueur + " lettres sont aux nombres de  " + dico.Count + ".";
        }

        public bool RechDichoRecursif(int debut, int fin, string mot)
        {
            int milieu = (fin + debut) / 2;
            string find = dico[milieu];

            if (find != mot)
            {
                if (fin != debut)
                {
                    if(debut < fin)
                    {
                        if (mot.CompareTo(find) == -1)
                        {
                            return RechDichoRecursif(debut, milieu - 1, mot);
                        }
                        else
                        {
                            return RechDichoRecursif(milieu + 1, fin, mot);
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
