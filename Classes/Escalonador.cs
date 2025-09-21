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
        // fila usada pelo Round-Robin (RR).
        private Queue<Processo> filaRR = new Queue<Processo>();

        public event Action? ProcessoTrocado; // evento que Form vai ouvir

        public Escalonador(string politica, int quantum)
        {
            this.politica = string.IsNullOrEmpty(politica) ? "RR" : politica;
            this.quantum = quantum;
        }

        public async Task Escalonar(List<Processo> processos, int delayMs)
        {
            if (string.IsNullOrEmpty(politica)) politica = "RR";

            if (processos == null || processos.Count == 0)
                return;

            if (politica == "RR")
            {

                // enfileira novos processos Pronto que ainda não estão na fila
                foreach (var p in processos.Where(p => p.estado == Estados.Pronto && !filaRR.Contains(p)))
                    filaRR.Enqueue(p);

                if (filaRR.Count == 0) return;

                var processo = filaRR.Dequeue();

                // espera toda a execução do processo por unidade
                await processo.ExecutarRR(quantum, () => ProcessoTrocadoInvoke());

                // atualiza UI ao final do ciclo também (redundância)
                ProcessoTrocado?.Invoke();

                // re-enfileira se não finalizou
                if (processo.estado == Estados.Pronto)
                    filaRR.Enqueue(processo);

                await Task.Delay(delayMs);
            }
            else if (politica == "PRIORIDADE" || politica == "PR" || politica == "P")
            {
                var candidatos = processos.Where(p => p.estado == Estados.Pronto).ToList();
                if (candidatos.Count == 0) return;

                var selecionado = candidatos
                    .OrderByDescending(p => (int)p.prioridade)
                    .ThenBy(p => p.pId)
                    .First();

                // espera execução de cada um
                await selecionado.ExecutarRR(quantum, () => ProcessoTrocadoInvoke());

                // atualiza UI após o ciclo
                ProcessoTrocado?.Invoke();

                await Task.Delay(delayMs);
            }
            else if (politica == "FCFS")
            {
                //processos já são adicionados na ordem FCFS
                var candidatos = processos.Where(p => p.estado == Estados.Pronto).ToList();
                if (candidatos.Count == 0) return;

                var selecionado = candidatos.First();

                await selecionado.ExecutarFCFS(delayPorUnidadeMs: 200, onUnitExecuted: () => ProcessoTrocadoInvoke());

                ProcessoTrocado?.Invoke();
                await Task.Delay(delayMs);

            }
            else
            {
                // política desconhecida -> tratar como RR
                politica = "RR";
                await Escalonar(processos, delayMs);
            }
        }

        private void ProcessoTrocadoInvoke()
        {
            try { ProcessoTrocado?.Invoke(); } catch { }
        }
    }
}
