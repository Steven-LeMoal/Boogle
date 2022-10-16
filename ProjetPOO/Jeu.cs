using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Collections.ObjectModel;


namespace ProjetPOO
{
    public class Jeu
    {
        private List<Dictionnaire> mondico;//Dans mon dictionnaire cs j'ai créer une SortedList<int,string[]> dico ce qui me permet de mieux manipuler les données donc je n'est pas besoin de tableau de dictionnaire
        private Plateau monplateau;

        public Jeu()
        {
            mondico = FichierDico("MotsPossibles.txt");
            monplateau = NewDe(FichierDe("Des.txt"));
        }


        public List<Dictionnaire> FichierDico(string str)
        {
            List<Dictionnaire> mondico = new List<Dictionnaire>();

            try
            {
                int longueur = 2; //premiere longueur du fichier
                List<string> listLong = new List<string>();
                int j = 0;

                using (StreamReader sr = new StreamReader(str))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Replace(" ", "").Length > 2 || Convert.ToInt32(line.Replace(" ", "")) == 2)
                        {
                            for (int i = 0; i < line.Length; i++)
                            {
                                if (line[i] == ' ')
                                {
                                    j++;
                                }
                                else
                                {
                                    if ((i - j) % longueur == 0)//on regarde si on arrive sur un nouveau mot pour ne pas avoir d'erreur 
                                    {
                                        listLong.Insert(j, Convert.ToString(line[i]));
                                    }
                                    else
                                    {
                                        listLong[j] += line[i];
                                    }
                                }
                            }
                        }
                        else
                        {
                            mondico.Add(new Dictionnaire(listLong, longueur, "Français"));
                            listLong = new List<string>();
                            j = 0;
                            longueur = Convert.ToInt32(line.Replace(" ", ""));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pue etre lue car :");
                Console.WriteLine(e.Message);
            }

            return mondico;
        }

        public Plateau NewDe(De[] tabDe)
        {
            Random r = new Random();
            string faceSup = "";
            for (int i = 0; i < 16; i++)
            {
                tabDe[i].Lance(r);
                faceSup += tabDe[i].LettreChoix;
            }

            return new Plateau(tabDe, faceSup);
        }


        public De[] FichierDe(string str)
        {
            De[] tabDe = new De[16];

            try
            {
                using (StreamReader sr = new StreamReader(str))
                {
                    string line;
                    int j = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        tabDe[j] = new De(line.Replace(";", "").ToCharArray());//toCharArray par rapport aux choix de mots tableau de dé
                        j++;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Le fichier n'a pas pue etre lue car :");
                Console.WriteLine(e.Message);
            }

            return tabDe;
        }
        public static void AffichageNotation()
        {
            string[] taille = new string[6] { "Taille du mot", "3", "4", "5", "6", "7+" };
            string[] point = new string[6] { "Points       ", "2", "3", "4", "5", "11" };

            foreach (string x in taille)
            {
                Console.Write(" {0,5} ", x);
                Console.Write("|");
            }

            Console.WriteLine("");

            foreach (string x in point)
            {
                Console.Write(" {0,5} ", x);
                Console.Write("|");
            }

            Console.WriteLine("\n");
        }
        public static bool TestTime(DateTime date1, DateTime date2, int max)//On va partir sur 10 minutes
        {
            TimeSpan interval = date2 - date1;

            if (interval.Minutes >= max)
            {
                return false;
            }

            return true;
        }

        public static bool TestMot(Jeu game, string mot, Joueur p1, Joueur p2)//on considère qi'il n'y a que 2 joueur
        {
            
            bool testContain = p1.Contain(mot) || p2.Contain(mot);
            bool adjacent = game.monplateau.Test_Plateau(mot);

            if (testContain || !adjacent|| !game.mondico[mot.Length - 2].RechDichoRecursif(0, game.mondico[mot.Length - 2].Dico.Count() - 1, mot))//les dictionnaires sont rangées par longueur de 2 à 15 (dans l'ordre croissant : mot.Length - 2) en partant de 0
            {
                if (testContain)
                {
                    Console.WriteLine("Le mot a déjà était utilisé....");
                }
                else if (!game.monplateau.Test_Plateau(mot))
                {
                    Console.WriteLine("Le mot saisit ne correspond à aucune combinaison sur le plateau...");
                }
                else
                {
                    Console.WriteLine("Le mot saisir n'existe pas dans notre liste de mot....");
                }
            }
            else
            {
                return true;
            }

            return false;

        }

        public static bool tailleMot(string mot)
        {
            if (mot.Length < 3 || mot.Length > 15)
            {
                if (mot.Length < 3)
                {
                    Console.WriteLine("Le mot saisit à une taille trop petite (doit être supérieur à 2 lettres)...");
                }
                else
                {
                    Console.WriteLine("Le mot saisit à une taille trop grande (doit être inférieur à 16 lettres)...");
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        private static string EntreString(string message)
        {
            string nom;
            do
            {
                Console.WriteLine(message);
                nom = Convert.ToString(Console.ReadLine());
            } while (nom == "");


            return nom;
        }

        public static void AffichageResultat(Joueur p1, Joueur p2)
        {
            if (p1.Score > p2.Score)
            {
                Console.WriteLine("{0} est le grand Gagnant....", p1.Nom);
            }
            else if (p1.Score < p2.Score)
            {
                Console.WriteLine("{0} est le grand Gagnant....", p2.Nom);
            }
            else
            {
                Console.WriteLine("Les 2 joueurs sont à égalité .....");
            }

            Console.WriteLine(p1.toString() + "\n" + p2.toString());
        }

        public static int Add_Score(string mot)
        {
            int taille = mot.Length;
            int score = 0;
            int[] tabScore = new int[5] { 2, 3, 4, 5, 11 };

            for (int i = 0; i < 5; i++)
            {
                if (tabScore[i] + 1 == taille)
                {
                    score = tabScore[i];
                    break;
                }
                else if (i == 4)
                {
                    score = 11;
                    break;
                }

            }

            return score;
        }

        public static void IA(Jeu game, Joueur iA, SortedList<string, List<string>> listePrefixe, DateTime debutTour)//On va utiliser la méthode des prefixe pour solver la recherche de données
        {
            string mot;
            int i = 0;

            while(TestTime(debutTour, DateTime.Now, 1) && i < listePrefixe.Count)
            {
                string prefixe = listePrefixe.Keys[i];
                if (game.monplateau.Test_Plateau(prefixe))
                {
                    for(int j = 0; j < listePrefixe.ElementAt(i).Value.Count(); j++)
                    {
                        mot = listePrefixe.ElementAt(i).Value[j];
                        //Console.WriteLine(mot);
                        if (!iA.Contain(mot) && game.monplateau.Test_Plateau(mot))
                        {
                            iA.Add_Mot(mot);
                            iA.Score += Add_Score(mot);
                        }
                    }
                }
                i++;
            }
       
        }

        public static SortedList<string, List<string>> PrefixeDico(Jeu game)
        {
            SortedList<string, List<string>> liste = new SortedList<string, List<string>>();

            foreach(Dictionnaire x in game.mondico)//On créer une liste avec comme clé en entré un préfixe et en value une liste de mot commçant par ce préfixe
            {
                if(x.Dico[0].Length != 2)
                {
                    for (int i = 0; i < x.Dico.Count(); i++)
                    {
                        if (liste.ContainsKey(x.Dico[i].Remove(2)))//on teste si le prefixe existe deja le préfixe sinon on l'ajoute
                        {
                            liste.ElementAt(liste.IndexOfKey(x.Dico[i].Remove(2))).Value.Add(x.Dico[i]);
                        }
                        else
                        {
                            List<string> mot = new List<string>();
                            mot.Add(x.Dico[i]);
                            liste.Add(x.Dico[i].Remove(2), mot);
                        }
                    }
                }
            }

            return liste;

        }


        public static void Game(Jeu game, Joueur p1, Joueur p2, bool ia)
        {
            Console.Clear();

            SortedList<string, List<string>> listePrefixe = new SortedList<string, List<string>>();
            if (ia)
            {
                listePrefixe = PrefixeDico(game);
            }

            DateTime debut = DateTime.Now;
            Console.WriteLine("Le compte à rebours de 1 mintues est lancé.....");
            int a = 0;//va servir de variable pour changer le joueur qui va jouer

            

            do
            {
                Joueur player = a == 0 ? p1 : p2;
                a = a == 1 ? 0 : 1;

                Console.WriteLine("\nC'est au tour de {0} de jouer.\n", player.Nom);

                Console.WriteLine(game.monplateau.toString());

                DateTime  debutTour = DateTime.Now;

                do
                {
                    if (ia && player == p2)
                    {
                        Console.WriteLine("Recherche de mot...");
                        IA(game, player, listePrefixe, debutTour);
                        Console.WriteLine(player.toString());
                        break;
                    }
                    else
                    {
                        string mot = EntreString("\nSaissiez un nouveau mot trouvé... : ").ToUpper();

                        if (!tailleMot(mot) || !TestMot(game, mot, p1, p2) || !TestTime(debutTour, DateTime.Now, 1))
                        {
                            if (!TestTime(debutTour, DateTime.Now, 1))//temps par tour
                            {
                                Console.WriteLine("La minute de durée étant dépassé le dernier mot saisit après cette durée n'a pas été comptabilisé");
                            }
                        }
                        else
                        {
                            player.Add_Mot(mot);
                            player.Score += Add_Score(mot);
                            Console.WriteLine(player.toString());
                        }
                    }
                } while (TestTime(debutTour, DateTime.Now, 1));

                //Console.Clear();

                game.monplateau = game.NewDe(game.monplateau.TabDe);

            } while (TestTime(debut, DateTime.Now, 10));

            Console.WriteLine("La partie est terminé ...");//si la fenetre n'affiche pas cela et se ferme (regarder dans vos icones actifs en bas de votre ecran : j'avais mis une priotié sur Visual Sudio dans le Gestionnaire de tache c'est pour cela que des qu'on est arrié à la fin du programme la fenetre de commande s'est mis dérriere visual studio sinon je ne sais pas ce que c'est

            AffichageResultat(p1, p2);

        }



        static void Main(string[] args)
        {
            Jeu game = new Jeu();

            Console.WriteLine("              -------Bienvenue dans le jeu du Boogle------- \n" +
               "   Le jeu commence par le mélange d’un plateau (carré) de 16 dés à 6 faces. Chaque dé possède une lettre différente sur chacune de ses faces.\n" +
               "   Les dés sont lancés sur le plateau 4 par 4, et seule leur face supérieure est visible. \n" +
               "   Après cette opération, un compte à rebours de 10 minutes est lancé qui établit la durée de la partie.\n" +
               "   Chaque joueur joue l’un après l’autre pendant un laps de temps de 1 mn pour trouver des mots pouvant être formés à partir de lettres adjacentes du plateau.\n" +
               "   Par adjacente, il est sous - entendu horizontalement, verticalement ou en diagonale .\n" +
               "   Les mots doivent être de 3 lettres au minimum (15 max) et ne peut pas avoir déjà était trouvé dans la partie. \n" +
               "   En fonction de la taille du mot les points suivants sont octroyés. \n" +
               "-----------------------------------------------------------------------------------------------------\n");

            AffichageNotation();

            //Je suis partie du principe que dans le jeu il y avait 2 joueur comme énoncé dans la partie Exercice 5 de l'énoncé du Problème sur DVO

            Joueur p1 = new Joueur(EntreString("Veuillez saisir le nom du joueur 1 : "));

            int val;
            Console.WriteLine("Voulez-vous jouez contre une IA (niveau Impossible uniquement) ?");
            do
            {
                Console.WriteLine("Veuillez saisir 1 (pour oui) ou 0 (pour non) : ");
                val = Convert.ToInt32(Console.ReadLine());

            } while (val != 1 && val != 0);

            bool ia = val == 1 ? true : false;

            Joueur p2;

            if (ia)
            {
                p2 = new Joueur("IA-0-LOSE");
            }
            else
            {
                p2 = new Joueur(EntreString("Veuillez saisir le nom du joueur 2 : "));
            }

            Game(game, p1, p2,ia);

        }
    }
}
