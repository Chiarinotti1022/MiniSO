using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Sistema
    {
        public Memoria memoria { get; set; }
        public Processador processador { get; set; }
        public Escalonador escalonador { get; set; }

        private readonly Random rand = new Random();
        public List<Processo> processos = new List<Processo>();
        private CancellationTokenSource cts; 

        public void IniciarSistema(int memoriaTotal)
        {
            escalonador = new Escalonador("RR", 10);
            memoria = new Memoria(memoriaTotal);
            cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {

                    foreach (var p in processos) { 
                        if(p.estado == Estados.Finalizado)
                        {
                            memoria.liberar(p.tamanhoMemoria);
                        }
                    }

                    /*
                    processos.RemoveAll(p =>
                    {
                        if (p.estado == Estados.Finalizado)
                        {
                            memoria.liberar(p.tamanhoMemoria);
                            return true;
                        }
                        return false;
                    });*/

                    /*
                    CriarProcesso(rand.Next(0, 100),
                                  (Prioridade)rand.Next(0, 2),
                                  rand.Next(50, 300));*/


                    
                    escalonador.Escalonar(processos, 1000);

                   
                    await Task.Delay(3000, cts.Token);
                }
            }, cts.Token);
        }

        public void EncerrarSistema()
        {
            cts?.Cancel();
        }

        public Processo CriarProcesso(int pid, Prioridade prioridade, int tamanho)
        {
            if (memoria.alocar(tamanho))
            {
                Processo p = new Processo(pid, prioridade, tamanho);
                p.CriarThreads();
                processos.Add(p);

                return p;
            }
            return null;
        }
    }
}
