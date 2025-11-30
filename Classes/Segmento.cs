using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Segmento
    {
        public int Base { get; set; }
        public int Limite { get; set; }

        public Segmento(int baseAddr, int limit)
        {
            Base = baseAddr;
            Limite = limit;
        }

        public int Fim => Base + Limite;
    }
}
