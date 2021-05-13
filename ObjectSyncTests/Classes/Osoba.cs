using System;
using System.Collections.Generic;

namespace ObjectSync.Tests
{
    public class Osoba
    {
        public String Imie { get; set; }
        public String Nazwisko { get; set; }
        public Adres Adres { get; set; }
        public List<Zwierze> Zwierzeta { get; set; }
        public String ID { get; set; }

        public Osoba()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}