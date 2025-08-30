using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Thread
    {
        public enum Estados
        {
            Criado = 0,
            Pronto = 1,
            Executando = 2,
            Aguardando = 3,
            Finalizado = 4
        }
        public enum Prioridade
        {
            Baixa = 0,
            Media = 1,
            Alta = 2
        }
        public int tId { get; set; }
        public int pIdPai { get; set; }
        public int tamanho { get; set; }    
        public Estados estado { get; set; }
        public Prioridade prioridade { get; set; }

        public Thread(int tid, int pidPai, int tamanho, Estados estado, Prioridade prioridade)
        {
            this.prioridade = prioridade;
            this.estado = estado;
            this.tamanho = tamanho;
            this.tId = tid;
            this.pIdPai = pidPai;
        }

        public void Executar()
        {
        }
    }
}
