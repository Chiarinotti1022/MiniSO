using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static MiniSO.Classes.Processador;

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
        public int QuantumAtual { get; set; } = 0;
        
        public Processo(int pid, Prioridade prioridade, int tamanho)
        {
            this.pId = pid;
            this.estado = Estados.Pronto;
            this.prioridade = prioridade;
            this.tamanhoMemoria = tamanho;
            this.threads = new List<Thread>();


        }

        public async Task ExecutarRR(int quantum, Action onUnitExecuted = null, int delayPorUnidadeMs = 200)
        {
            QuantumAtual = quantum;
            estado = Estados.Executando;

            int restante = quantum;

            while (restante > 0)
            {
                var threadsProntas = threads.Where(t => t.estado == Estados.Pronto).ToList();
                if (!threadsProntas.Any()) break;

                foreach (var t in threadsProntas.ToList())
                {
                    if (restante == 0) break;

                    // marca como executando e notifica UI (vai pintar verde)
                    t.estado = Estados.Executando;
                    try { onUnitExecuted?.Invoke(); } catch { }

                    // espera curto para dar chance ao UI atualizar e mostrar o estado
                    if (delayPorUnidadeMs > 0)
                        await Task.Delay(delayPorUnidadeMs);

                    // executa 1 unidade
                    bool terminou = t.ExecutarUnidade();

                    // marca estado de acordo
                    if (terminou)
                        t.estado = Estados.Finalizado;
                    else
                        t.estado = Estados.Pronto;

                    // notifica UI após executar a unidade
                    try { onUnitExecuted?.Invoke(); } catch { }

                    restante--;
                }
            }

            // se todas as threads finalizaram → finaliza o processo (libera memória depois no Sistema)
            if (threads.All(th => th.estado == Estados.Finalizado))
                Finalizar();
            else
                estado = Estados.Pronto;
        }




        // FinalizarThread agora só marca a thread como Finalizado
        public void FinalizarThread(Thread t)
        {
            t.estado = Estados.Finalizado;
            Console.WriteLine($"Thread {t.tId} finalizada");
            // não remover da lista threads
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
                    countPc: rand.Next(1, 30)
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

        }/*
        public void FinalizarThread(Thread t)
        {
            Console.WriteLine($"Thread: {t.tId} finalizada");
            threads.Remove(t);*/
        }
    }

