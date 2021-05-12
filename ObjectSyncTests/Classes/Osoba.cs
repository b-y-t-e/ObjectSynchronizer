using System;
using System.Collections.Generic;

namespace ObjectSync.Tests
{
    public class Osoba
    {
        public String Imie { get; set; }

        public String Nazwisko { get; set; }

        public List<Zwierze> Zwierzeta = new List<Zwierze>();
    }
}