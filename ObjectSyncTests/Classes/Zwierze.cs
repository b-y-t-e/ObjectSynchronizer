using System;

namespace ObjectSync.Tests
{
    public class Zwierze
    {
        public String Rodzaj { get; set; }
        public String Imie { get; set; }
        public String ID { get; set; }

        public Zwierze()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}