using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectSync;
using ObjectSyncTests.Classes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSync.Tests
{
    [TestClass()]
    public class SyncSimpleTests
    {
        [TestMethod()]
        public void kopiowanie_pol_w_klasie_powinno_dzialac_poprawnie()
        {
            Osoba osoba1 = new Osoba
            {
                Imie = "andrzej",
                Nazwisko = "pain"
            };
            Osoba osoba2 = new Osoba
            {

            };

            new Sync().
                Execute(osoba1, osoba2);

            Assert.AreEqual(osoba1.Nazwisko, osoba2.Nazwisko);
            Assert.AreEqual(osoba1.Imie, osoba2.Imie);
        }

        [TestMethod()]
        public void kopiowanie_kolekcji_gdy_pusta_docelowa_powinno_dzialac_poprawnie()
        {
            Osoba osoba1 = new Osoba
            {
                Imie = "andrzej",
                Nazwisko = "pain",
                Zwierzeta = new List<Zwierze>
                {
                    new Zwierze
                    {
                        Imie = "byte",
                        Rodzaj = "pies"
                    }
                }
            };
            Osoba osoba2 = new Osoba
            {

            };

            new Sync().
                Execute(osoba1, osoba2);

            Assert.AreNotEqual(osoba1.Zwierzeta, osoba2.Zwierzeta);
            Assert.AreEqual(osoba1.Zwierzeta.Count, osoba2.Zwierzeta.Count);
            Assert.AreEqual(osoba1.Zwierzeta[0].Imie, osoba2.Zwierzeta[0].Imie);
            Assert.AreEqual(osoba1.Zwierzeta[0].Rodzaj, osoba2.Zwierzeta[0].Rodzaj);
        }

        [TestMethod()]
        public void kopiowanie_kolekcji_powinno_dzialac_poprawnie()
        {
            Osoba osoba1 = new Osoba
            {
                Imie = "andrzej",
                Nazwisko = "pain",
                Zwierzeta = new List<Zwierze>
                {
                    new Zwierze
                    {
                        Imie = "byte",
                        Rodzaj = "pies"
                    }
                }
            };
            Osoba osoba2 = new Osoba
            {
                Zwierzeta = new List<Zwierze>
                {
                    new Zwierze
                    {
                        Imie = "",
                        Rodzaj = ""
                    }
                }
            };

            new Sync().
                Execute(osoba1, osoba2);

            Assert.AreNotEqual(osoba1.Zwierzeta, osoba2.Zwierzeta);
            Assert.AreEqual(osoba1.Zwierzeta.Count, osoba2.Zwierzeta.Count);
            Assert.AreEqual(osoba1.Zwierzeta[0].Imie, osoba2.Zwierzeta[0].Imie);
            Assert.AreEqual(osoba1.Zwierzeta[0].Rodzaj, osoba2.Zwierzeta[0].Rodzaj);
        }


        [TestMethod()]
        public void kopiowanie_obiektu_gdy_pusty_docelowy_w_klasie_powinno_dzialac_poprawnie()
        {
            Osoba osoba1 = new Osoba
            {
                Imie = "andrzej",
                Nazwisko = "pain",
                Adres = new Adres
                {
                    Miasto = "gdańsk",
                    Ulica = "długa"
                }
            };
            Osoba osoba2 = new Osoba
            {

            };

            new Sync().
                Execute(osoba1, osoba2);

            Assert.AreNotEqual(osoba1.Adres, osoba2.Adres);
            Assert.AreEqual(osoba1.Adres.Miasto, osoba2.Adres.Miasto);
            Assert.AreEqual(osoba1.Adres.Ulica, osoba2.Adres.Ulica);
        }

        [TestMethod()]
        public void kopiowanie_obiektu_w_klasie_powinno_dzialac_poprawnie()
        {
            Osoba osoba1 = new Osoba
            {
                Imie = "andrzej",
                Nazwisko = "pain",
                Adres = new Adres
                {
                    Miasto = "gdańsk",
                    Ulica = "długa"
                }
            };
            Osoba osoba2 = new Osoba
            {
                Adres = new Adres()
            };

            new Sync().
                Execute(osoba1, osoba2);

            Assert.AreNotEqual(osoba1.Adres, osoba2.Adres);
            Assert.AreEqual(osoba1.Adres.Miasto, osoba2.Adres.Miasto);
            Assert.AreEqual(osoba1.Adres.Ulica, osoba2.Adres.Ulica);
        }
    }
}