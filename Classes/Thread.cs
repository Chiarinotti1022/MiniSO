using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Thread
    {
        public int tId { get; set; }
        public int pIdPai { get; set; }
        public int tamanho { get; set; }    
        public Processo.Estados estado { get; set; }
        public Processo.Prioridade prioridade { get; set; }

        public Thread(int tid, int pidPai, int tamanho, Processo.Estados estado, Processo.Prioridade prioridade)
        {
            this.prioridade = prioridade;
            this.estado = estado;
            this.tamanho = tamanho;
            this.tId = tid;
            this.pIdPai = pidPai;
        }

        public void Executar()
        {
            estado = Processo.Estados.Executando;
        }
        public void Bloquear()
        {
            estado = Processo.Estados.Bloqueado;
        }

    }
}
