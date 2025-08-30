using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Processador
    {
        public enum Estados
        {
            Aguardando = 0,
            Executando = 1

        }
        public float freq { get; set; }
        public Estados estado { get; set; }
        
        public Processador(float freq) { 
            this.freq = freq;
            estado = Estados.Aguardando;
        }

        public void ExecutarProcesso(Processo p)
        {
            estado = Estados.Executando;

        }

        // Em caso de interrupção
        public void LiberarProcesso(Processo p)
        {

        }
    }
}
