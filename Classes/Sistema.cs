using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Sistema
    {
        Queue<Processo> processosAlta = new Queue<Processo>();
        Queue<Processo> processosBaixa = new Queue<Processo>();
        public Memoria memoria { get; set; }
        public Processador processador { get; set; }

        public void IniciarSistema(int memoriaTotal, float frequenciaProcessador)
        {
            memoria = new Memoria(memoriaTotal);
            processador = new Processador(frequenciaProcessador);
        }

        public void EncerrarSistema()
        {

        }

        public void ChamarEscalonador()
        {

        }
        public void CriarProcesso(int pid, Prioridade prioridade, int tamanho)
        {
            if (memoria.alocar(tamanho))
            {
                Processo p = new (pid, prioridade, tamanho);
                if (p.prioridade == Prioridade.Alta)
                {
                    processosAlta.Enqueue(p);
                }
                else
                {
                    processosBaixa.Enqueue(p);
                }
                Console.WriteLine($"Processo {pid} criado na fila de {p.prioridade} prioridade");
            }
    
        }
    }
}
