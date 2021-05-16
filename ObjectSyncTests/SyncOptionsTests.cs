using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectSync;
using ObjectSyncTests.Classes2;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectSync.Tests
{
    [TestClass()]
    public class SyncOptionsTests
    {
        [TestMethod()]
        public void dopasowanie_po_id_opcje()
        {
            Osoba osoba1 = new Osoba
            {
                Imie = "andrzej",
                Nazwisko = "pain",
                Zwierzeta = new List<Zwierze>
                {
                    new Zwierze
                    {
                        ID = "1",
                        Imie = "byte1",
                        Rodzaj = "pies1"
                    },
                    new Zwierze
                    {
                        ID = "2",
                        Imie = "byte2",
                        Rodzaj = "pies2"
                    }
                }
            };
            Osoba osoba2 = new Osoba
            {
                Zwierzeta = new List<Zwierze>
                {
                    new Zwierze
                    {
                        ID = "1",
                        Imie = "byte1",
                        Rodzaj = "pies1"
                    }
                }
            };

            SyncOptions options = new SyncOptions
            {
                TypeKeyDelegate = type => "ID"
            };

            new Sync().
                Execute(osoba1, osoba2, options);

            Assert.AreNotEqual(osoba1.Zwierzeta, osoba2.Zwierzeta);
            Assert.AreEqual(osoba1.Zwierzeta.Count, osoba2.Zwierzeta.Count);
            Assert.AreEqual(osoba1.Zwierzeta.Count, 2);
            for (var i = 0; i < osoba1.Zwierzeta.Count; i++)
            {
                Assert.AreEqual(osoba1.Zwierzeta[i].Imie, osoba2.Zwierzeta[i].Imie);
                Assert.AreEqual(osoba1.Zwierzeta[i].Rodzaj, osoba2.Zwierzeta[i].Rodzaj);
            }
        }


        [TestMethod()]
        public void omijanie_nazwiska_oraz_rodzaju()
        {
            Osoba osoba1 = new Osoba
            {
                Imie = "andrzej",
                Nazwisko = "pain",
                Zwierzeta = new List<Zwierze>
                {
                    new Zwierze
                    {
                        ID = "1",
                        Imie = "byte1",
                        Rodzaj = "pies1"
                    }
                }
            };
            Osoba osoba2 = new Osoba
            {
                Zwierzeta = new List<Zwierze>
                {
                    new Zwierze
                    {
                        ID = "1",
                        Imie = "stare imie",
                        Rodzaj = "stary rodzaj"
                    }
                }
            };

            SyncOptions options = new SyncOptions
            {
                TypeKeyDelegate = type => "ID",
                TypeFieldFilterDelegate = (type, prop) => !new[] { "Nazwisko", "Rodzaj" }.Contains(prop)
            };

            new Sync().
                Execute(osoba1, osoba2, options);

            Assert.AreNotEqual(osoba1.Zwierzeta, osoba2.Zwierzeta);
            Assert.AreEqual(osoba1.Zwierzeta.Count, osoba2.Zwierzeta.Count);
            Assert.AreEqual(osoba1.Zwierzeta.Count, 1);
            for (var i = 0; i < osoba1.Zwierzeta.Count; i++)
            {
                Assert.AreEqual(osoba1.Zwierzeta[i].Imie, osoba2.Zwierzeta[i].Imie);
                Assert.AreNotEqual(osoba1.Zwierzeta[i].Rodzaj, osoba2.Zwierzeta[i].Rodzaj);
            }
        }
    }
}