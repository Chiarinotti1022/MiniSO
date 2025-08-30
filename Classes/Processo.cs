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
            Bloqueado = 3,
            Finalizado = 4
        }
        public enum Prioridade
        {
            Baixa = 0,
            Alta = 1
        }
        public int pId {  get; set; }
        public Estados estado { get; set; }
        public Prioridade prioridade { get; set; }
        public int tamanho { get; set; }

        public Processo(int pid, Estados estado, Prioridade prioridade, int tamanho)
        {
            this.pId = pid;
            this.estado = estado;
            this.prioridade = prioridade;
            this.tamanho = tamanho;
        }

        public void Executar() 
        {
            estado = Estados.Executando;
            Console.WriteLine($"Processo: {pId} executando");
        }
        public void Bloquear()
        {
            estado = Estados.Bloqueado;
            Console.WriteLine($"Processo: {pId} bloqueado");
        }
        public void Finalizar()
        {
            estado = Estados.Finalizado;
            Console.WriteLine($"Processo: {pId} finalizado");
        }
        public void CriarThread()
        {
        }
        public void FinalizarThread()
        {
        }
    }
}
