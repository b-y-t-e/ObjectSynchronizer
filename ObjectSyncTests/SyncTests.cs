using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectSync;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSync.Tests
{
    [TestClass()]
    public class SyncTests
    {
        [TestMethod()]
        public void ExecuteTest()
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
        public void ExecuteTest2()
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
    }
}