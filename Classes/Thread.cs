using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Thread
    {
        public int tId { get; set; }
        public int pIdPai { get; set; }
        public int tamanho { get; set; }
        public Estados estado { get; set; }
        public Prioridade prioridade { get; set; }

        public int pc { get; set; }
        public int countPc { get; set; }

        public Thread(int tid, int pidPai, int tamanho, Prioridade prioridade, int countPc)
        {
            this.prioridade = prioridade;
            this.estado = Estados.Pronto;
            this.tamanho = tamanho;
            this.tId = tid;
            this.pIdPai = pidPai;
            this.countPc = countPc;
        }

        // executa apenas 1 unidade e retorna true se terminou
        public bool ExecutarUnidade()
        {
            if (this.pc < countPc)
                pc += 1;

            return pc >= countPc;
        }

        // opcional: manter Executar() caso algo o use — mas agora preferimos ExecutarUnidade()
        public void Executar()
        {
            ExecutarUnidade();
        }
    }
}

