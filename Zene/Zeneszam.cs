using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zene
{
    internal class Zeneszam
    {
        public int Csatorna { get; }
        public TimeSpan Kezdete { get; }
        public TimeSpan Vege { get; }
        public string Azonosito { get; }
        public string Eloado { get; }
        public string Cim { get; }

        public Zeneszam(int csatorna, int perc, int masodperc, string azonosito)
        {
            Csatorna = csatorna;
            Kezdete = new TimeSpan(0, perc, masodperc);
            Vege = new TimeSpan(0, perc, masodperc) + new TimeSpan(0, 30, 0); 
            Azonosito = azonosito;
            string[] eloadoCim = azonosito.Split(':');
            Eloado = eloadoCim[0];
            Cim = eloadoCim[1];
        }
    }
}
