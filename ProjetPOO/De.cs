using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;


namespace ProjetPOO
{
    public class De
    {
        private char[] lettreSde = new char[6];
        private char lettreChoix;

        public De(char[] lettreSde)
        {
            this.lettreSde = lettreSde;
        }

        public char LettreChoix
        {
            get { return lettreChoix; }
        }

        public char[] LettreSde
        {
            get { return lettreSde; }
            set { lettreSde = value; }
        }

        public void Lance(Random r)
        {
            lettreChoix = lettreSde[r.Next(6)];
        }

        public string toString()
        {
            string message = "Ce dé est composé par : ";

            foreach(char x in lettreSde)
            {
                message += Convert.ToString(x) + " | ";
            }

            return message + "\nSa lettre tirée est : " + Convert.ToString(lettreChoix);
        }
    }
}
