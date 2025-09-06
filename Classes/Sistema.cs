using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MiniSO.Classes
{
    internal class Sistema
    {
        public Memoria memoria { get; set; }
        public Processador processador { get; set; }
        public Escalonador escalonador { get; set; }

        public List<Processo> processos = new List<Processo>();
        public bool executar = true;

        public void IniciarSistema(int memoriaTotal)
        {
            escalonador = new Escalonador("RR", 10);
            memoria = new Memoria(memoriaTotal);
            Random rand = new Random();
            while (executar)
            {
                foreach(var p in processos)
                {
                    if (p.estado == Estados.Finalizado)
                    {
                        memoria.liberar(p.tamanhoMemoria);
                    }
                }
               
                
                processos.Add(CriarProcesso(rand.Next(0, 100), (Prioridade)rand.Next(0, 2), rand.Next(50, 300)));
                escalonador.Escalonar(processos);
                
                System.Threading.Thread.Sleep(10000);
            }
        }

        public void EncerrarSistema()
        {
            executar = false;
        }

        public void ChamarEscalonador()
        {

        }
        public Processo CriarProcesso(int pid, Prioridade prioridade, int tamanho)
        {
            if (memoria.alocar(tamanho))
            {
                Processo p = new (pid, prioridade, tamanho);
                processos.Add(p);
                return p;


            }
            return null;
            
    
        }
    }
}
