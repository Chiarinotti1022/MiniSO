using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Processo
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
        public int pID {  get; set; }
        public Estados estado { get; set; }
        public Prioridade prioridade { get; set; }
        public int tamanho { get; set; }

        public Processo(int pid, Estados estado, Prioridade prioridade, int tamanho)
        {
            this.pID = pid;
            this.estado = estado;
            this.prioridade = prioridade;
            this.tamanho = tamanho;
        }

        public void executar() 
        {
        }
        public void bloquear()
        {
        }
        public void criarThread()
        {
        }
        public void finalizarThread()
        {
        }
    }
}
