using ObjectSync;
using System;

namespace ObjectSyncTests.Classes2
{
    public class Zwierze
    {
        public String Rodzaj { get; set; }
        public String Imie { get; set; }
        [SyncKey]
        public String ID { get; set; }

        public Zwierze()
        {
            ID = Guid.NewGuid().ToString();
        }
    }
}