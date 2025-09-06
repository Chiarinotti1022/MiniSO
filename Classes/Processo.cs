using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    public enum Estados
    {
        Pronto = 1,
        Executando = 2,
        Bloqueado = 3,
        Finalizado = 4,
    }
    public enum Prioridade
    {
        Baixa = 0,
        Alta = 1
    }
    internal class Processo
    {   
        
        public int pId {  get; set; }
        public Estados estado { get; set; }
        public Prioridade prioridade { get; set; }
        public int tamanhoMemoria { get; set; }

        public List<Thread> threads { get; set; }

        public Processo(int pid, Prioridade prioridade, int tamanho)
        {
            this.pId = pid;
            this.estado = Estados.Pronto;
            this.prioridade = prioridade;
            this.tamanhoMemoria = tamanho;
            this.threads = new List<Thread>();
        }

        public void ExecutarRR(int quantum)
        {
            this.estado = Estados.Executando;
            
            var threadsProntas = threads.Where(t => t.estado == Estados.Pronto).ToList();

            if (threadsProntas.Count == 0)
                return;

            int quantumPorThread = Math.Max(1, quantum / threadsProntas.Count);

            foreach (var t in threadsProntas)
            {
                int unidades = 0;
                while (unidades < quantumPorThread && t.pc < t.countPc)
                {
                   
                    t.Executar();
                    unidades++;
                }
                

                Console.WriteLine($"Thread {t.tId} executou {unidades} unidades; pc={t.pc}/{t.countPc}");

                if (t.pc >= t.countPc)
                {
                    t.estado = Estados.Finalizado;
                    FinalizarThread(t);
                    Console.WriteLine($"Thread {t.tId} finalizada");
                } else
                {
                    t.estado = Estados.Pronto;
                }
            }
            
            if (threads.All(th => th.estado == Estados.Finalizado))
            {
                this.estado = Estados.Finalizado;
                Console.WriteLine($"Processo {pId} finalizado");
            } else
            {
                this.estado = Estados.Pronto;
            }

            
        }


        public void Bloquear()
        {
            estado = Estados.Bloqueado;
            Console.WriteLine($"Processo: {pId} bloqueado");
        }
        public void Finalizar()
        {
            estado = Estados.Finalizado;
            threads.Clear();
            Console.WriteLine($"Processo: {pId} finalizado");
        }
        public string CriarThreads()
        {
            Random rand = new Random();
            string s = "";
            int memoriaRestante = this.tamanhoMemoria;
            int qtdThreads = rand.Next(1, 5);
            memoriaRestante -= qtdThreads;

            for (int i = 0; i < qtdThreads; i++)
            {
                int novoTid = threads.Count > 0 ? threads.Max(t => t.tId) + 1 : 1;
                int maxMemoriaThread = ((i == qtdThreads - 1) ? memoriaRestante : rand.Next(1, memoriaRestante - (qtdThreads - i - 1) + 1))+1;
                memoriaRestante -= (i == qtdThreads - 1) ? maxMemoriaThread : (maxMemoriaThread - 1);


                Thread t = new Thread(
                    tid: novoTid,
                    pidPai: this.pId,
                    tamanho: maxMemoriaThread,
                    prioridade: (Prioridade)rand.Next(0, 2),
                    countPc: rand.Next(1, 10)
                    );
                s +=($"""
                    Thread: 
                    T id: {t.tId}
                    P id: {t.pIdPai}
                    T tamanho: {t.tamanho}
                    T prioridade: {t.prioridade}
                    T countPc: {t.countPc}
                    
                    """);
                threads.Add(t);
            }
            return s;

        }
        public void FinalizarThread(Thread t)
        {
            Console.WriteLine($"Thread: {t.tId} finalizada");
            threads.Remove(t);
        }
    }
}
