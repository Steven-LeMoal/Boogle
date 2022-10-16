using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ProjetPOO
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Score_MotABC_Retourne2()
        {
            string mot = "abc";
            int result = Jeu.Add_Score(mot);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Contain_MotABC_RetourneFalse()
        {
            string mot = "abc";
            Joueur steven = new Joueur("Steven");
            bool result = steven.Contain(mot);
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public void TailleMot_MotAB_RetourneFalse()
        {
            string mot = "ab";
            bool result = Jeu.tailleMot(mot);
            Assert.AreEqual(false, result);
        }


        [TestMethod]
        public void TestTime_RetourneFalse()
        {
            DateTime now1 = DateTime.Now;
            int max = 10;//10 minutes
            DateTime now2 = DateTime.Now;

            bool result = Jeu.TestTime(now1, now2, max);
            Assert.AreEqual(false,!result);
        }

        [TestMethod]
        public void Add_Mot_MotABC_RetourneFalse()
        {
            string mot = "abc";
            Joueur steven = new Joueur("Steven");
            steven.Add_Mot(mot);
            Assert.AreEqual("abc", steven.MotUsee[0]);

        }
    }
}
