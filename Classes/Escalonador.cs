using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Escalonador
    {
        public string politica { get; set; }
        public int quantum { get; set; }

        // fila usada pelo Round-Robin (RR). Para PRIORIDADE não usamos filaRR persistentemente.
        private Queue<Processo> filaRR = new Queue<Processo>();

        public event Action? ProcessoTrocado; // evento que Form vai ouvir

        public Escalonador(string politica, int quantum)
        {
            this.politica = politica ?? "RR";
            this.quantum = quantum;
        }

        public async Task Escalonar(List<Processo> processos, int delayMs)
        {
            if (politica == null) politica = "RR";

            if (processos == null || processos.Count == 0)
                return;

            if (politica == "RR")
            {
                // Round-Robin: mantemos filaRR para preservar ordem entre ticks
                // Enfileira novos processos Pronto que ainda não estão na fila
                foreach (var p in processos.Where(p => p.estado == Estados.Pronto && !filaRR.Contains(p)))
                    filaRR.Enqueue(p);

                if (filaRR.Count == 0) return;

                var processo = filaRR.Dequeue();

                processo.ExecutarRR(quantum);

                ProcessoTrocado?.Invoke();

                // só re-enfileira se NÃO tiver finalizado
                if (processo.estado == Estados.Pronto)
                    filaRR.Enqueue(processo);

                await Task.Delay(delayMs);
            }
            else if (politica == "PRIORIDADE" || politica == "PR" || politica == "P")
            {
                // Prioridade: seleciona, dentre os Prontos, o de maior prioridade (Alta > Baixa).
                // Em caso de empate, usa menor pId (ou FIFO implícito pela lista).
                var candidatos = processos.Where(p => p.estado == Estados.Pronto).ToList();
                if (candidatos.Count == 0) return;

                // Prioridade: enum Prioridade { Baixa = 0, Alta = 1 }
                var selecionado = candidatos
                    .OrderByDescending(p => (int)p.prioridade) // Alta (1) primeiro
                    .ThenBy(p => p.pId) // tie-breaker
                    .First();

                // Executa com quantum (mantemos fatias de tempo, para comportamento similar ao RR)
                selecionado.ExecutarRR(quantum);

                ProcessoTrocado?.Invoke();

                // se selecionado ficou Pronto (não finalizou) — na próxima chamada ele pode ser escolhido novamente,
                // dependendo de prioridades existentes. Não precisamos re-enfileirar nada aqui.
                await Task.Delay(delayMs);
            }
            else
            {
                // política desconhecida: trata como RR por segurança
                await Escalonar(processos, delayMs);
            }
        }
    }
}
