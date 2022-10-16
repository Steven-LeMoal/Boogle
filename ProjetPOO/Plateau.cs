using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;


namespace ProjetPOO
{
    public class Plateau
    {
        private De[] tabDe = new De[16];
        private string tabFaceSup; //j'ai choisi un simple string au lieu d'un tableau de string car cela offre certaines fonctions deja faites et son utilisation est modulable

        public Plateau(De[] tabDe, string tabFaceSup)
        {
            this.tabDe = tabDe;
            this.tabFaceSup = tabFaceSup;
        }

        public De[] TabDe
        {
            get { return tabDe; }
        }

        public string TabFaceSup
        {
            get { return tabFaceSup; }
        }

        public string toString()
        {
            string message = "";

            for(int i = 0; i < tabFaceSup.Length; i++)
            {
                message += tabFaceSup[i] + " ";
                message += (i + 1) % 4 == 0 ? "\n": "";
            }

            return message;
        }


        public bool Test_Plateau(string mot, int[] tab = null)//Cette algorithme teste des chemins pour savoir si un mot peut être former en partant de la premiere lettre et en jouant sur la profondeur des différentes combinaisons (branches) possibles
        {
            string motTest = "";

            tab = tab == null ? new int[16] : tab;

            int voisin = -1;
            int profondeur = 0;//profondeur est différent de 0 si il y a plus d'une branche avant d'être desecndu dans une branche spécifique (si je dois explorer d'autres branches)
            int a = 0;

            bool peutMonter = false;
            bool sontMonter = false;


            foreach (char x in mot)//Cette partie correspond à la traduction des détails mis dans le mot au cas où une branche ne fonctionne pas et je dois remonter au rang au-dessus
            {
                if (x > 64 && x < 91)
                {
                    motTest += x;
                }
                else
                {
                    if (x == '|')//J'ai choisis une 'cryptographie' très simple : si il y a des branches que je dois explorer mais que je dois descendre plus profondement pour teest certain cas alors j'ajoute à la fin du mot 'la profondeur de la branche | le nombre de branche qu'il me rester à explorer à cette profondeur | '
                    {
                        peutMonter = true;
                        a = a == 1 ? 0 : 1;
                    }
                    else if (x == '?')//Voir plus bas les détails par rapport à cela
                    {
                        sontMonter = true;
                    }
                    else if (a == 1)
                    {
                        voisin = Convert.ToInt32(x) - 48;
                    }
                    else
                    {
                        profondeur = Convert.ToInt32(x) - 48 + 1;
                    }
                }
            }

            voisin = sontMonter ? voisin : 0;//le nombre de voisin n'est conservé que si l'on vient de remonté car si on descend dans une branche le nombre de voisin (rang n-1 ducoup) est différent de celui au rang n

            if (sontMonter)//S'il on vient de remonter d'une profondeur au niveau d'une branche on enleve les détails laissé auparavant lorsqu'on était à cette profondeur
            {
                mot = mot.Remove(mot.Length - 6);
            }

            if (tabFaceSup.Contains(motTest[0]))//pas nescessaire
            {
                motTest = motTest.Substring(profondeur);//si on est decendu (au rang n-1) mais qu' au rang n il y a avait plus de 1 possibilités alors motTest qui prendra seulement des lettres doit etre au test de rang n-1 (on enleve le nombre de lettre selon la profondeur de la branche dans laquelle on se trouve)

                char[] lettre = { motTest[0] };
                int n = tabFaceSup.IndexOfAny(lettre, 0);
                int plusieurPosiP = 0;

                if (voisin > 0)//si l'on vient de remonter à un niveau de profondeur on enleve les branches déjà examiner
                {
                    for (int i = 1; i < voisin; i++)
                    {
                        n = tabFaceSup.IndexOfAny(lettre, n + 1);
                    }
                    plusieurPosiP = voisin;
                }
                else
                {
                    while (n != -1)//on calcul le nombre de branche à calculer à ce rang
                    {
                        plusieurPosiP++;
                        n = tabFaceSup.IndexOfAny(lettre, n + 1);
                    }

                    n = tabFaceSup.IndexOfAny(lettre, 0);
                }

                int b = 0;
                if (profondeur == 0)
                {
                    tab[0] = n;
                    b = 1;
                }


                while (plusieurPosiP >= 1 && n != -1) //je fais un while car il est possible qu'une lettre apparaise plusieurs fois sur les cases adjacentes 
                {
                    for (int i = n % 4 - 1; i <= n % 4 + 1; i++)//vue que je travaille avec un string (tableau 1D de caractére) et que l'on veut gérer des cases adjacentes en 2D, je détermine le i et j autour de la case qui contient la lettre vérifié
                    {
                        for (int j = n / 4 - 1; j <= n / 4 + 1; j++)
                        {
                            if (i >= 0 && i < 4 && j >= 0 && j < 4)
                            {
                                if (tabFaceSup[i + j * 4] == motTest[1] && (i + j * 4) != n && !tab.Contains(i + j*4))
                                {
                                    if (motTest.Length == 2)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        tab[b == 0 ? profondeur :profondeur+1] = i + j * 4;

                                        if (plusieurPosiP > 1 || peutMonter)
                                        {
                                           
                                            return Test_Plateau(mot + Convert.ToString(profondeur) + "|" + (plusieurPosiP - 1) + "|",tab);//Si cette branche fonctionne mais qu'il y a d'autres possibilités ou que je peux remonter à ce même rang alors j'ajoute à la fin du mot certain détails pour que si je mreonte dans la branche je puisse tester les autres cas
                                        }
                                        else
                                        {
                                            return Test_Plateau(mot.Substring(1),tab);//Si cela fonctionne, que ce soit le seul choix et que je ne peut plus remonter alors je desccends
                                        }
                                    }
                                }
                            }
                        }
                    }
                    plusieurPosiP--;
                    n = tabFaceSup.IndexOfAny(lettre, n + 1);
                }
            }

            if (profondeur > 0 && peutMonter)
            {
                if (mot[mot.Length - 1] == '|' && mot[mot.Length - 3] == '|')//Pour prévenir des potentiels erreurs
                {
                    tab[profondeur] = -1;
                    return Test_Plateau(mot + "?",tab);//Si cette branche ne fonctionne pas et que je peux remonter, je remonte d'un rang (en utilisant un ? et en le supprimant au tour d'après)
                }
            }

            return false;
        }

    }
}
