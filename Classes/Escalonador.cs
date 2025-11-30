using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public event Action? ProcessoTrocado; // evento que chama Form

        public int TrocasDeContexto { get; private set; } = 0;
        public int TempoDeTrocaDeContexto { get; set; } = 50;// custo de troca em ms
        public Thread? threadAtual = null;
        Processo ultimoProcessoExecutado = null;




        public Escalonador(string politica, int quantum)
        {
            this.politica = string.IsNullOrEmpty(politica) ? "RR" : politica;
            this.quantum = quantum;
        }

        //simula troca de contexto + contador
        private async Task RegistrarTrocaDeContexto()
        {
            TrocasDeContexto++;

            int custo = TempoDeTrocaDeContexto;
            if (custo < 0) custo = 0;

            await Task.Delay(custo);

            ProcessoTrocado?.Invoke();
        }

        public async Task RegistrarTrocaDeContextoThread(Thread? saindo, Thread entrando)
        {

            bool houveTroca = saindo != null && saindo.tId != entrando.tId;

            if (houveTroca)
            {
                TrocasDeContexto++;
                int custo = TempoDeTrocaDeContexto;
                if (custo < 0) custo = 0;

                await Task.Delay(custo);
            }

            threadAtual = entrando;

            ProcessoTrocado?.Invoke();
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

                // só troca se mudou o processo
                if (ultimoProcessoExecutado == null || ultimoProcessoExecutado.pId != processo.pId)
                {
                    await RegistrarTrocaDeContexto();
                    threadAtual = null;

                }

                ultimoProcessoExecutado = processo;


                // espera toda a execução do processo por unidade
                processo.EscalonadorInstance = this;
                await processo.ExecutarRR(quantum, () => ProcessoTrocadoInvoke());

                // atualiza UI ao final do ciclo também (redundância)
                ProcessoTrocado?.Invoke();

                // re-enfileira se não finalizou
                if (processo.estado == Estados.Pronto)
                {
                    filaRR.Enqueue(processo);
                }
                    

                await Task.Delay(delayMs);
            }
            else if (politica == "PRIORIDADE" || politica == "PR" || politica == "P")
            {
                Envelhecer(processos);
                var candidatos = processos.Where(p => p.estado == Estados.Pronto).ToList();
                if (candidatos.Count == 0) return;

                var selecionado = candidatos
                    .OrderByDescending(p => (int)p.PrioridadeDinamica)
                    .ThenBy(p => p.pId)
                    .First();

                // espera execução de cada um
                await RegistrarTrocaDeContexto();
                selecionado.EscalonadorInstance = this;
                await selecionado.ExecutarRR(quantum, () => ProcessoTrocadoInvoke());
                selecionado.PrioridadeDinamica = (int)selecionado.prioridade;
                selecionado.AgeTicks = 0;

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

                if (ultimoProcessoExecutado == null || ultimoProcessoExecutado.pId != selecionado.pId)
                {
                    await RegistrarTrocaDeContexto();
                }
                selecionado.EscalonadorInstance = this;
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

        private void Envelhecer(List<Processo> processos)
        {
            foreach (var p in processos)
            {
                // só processos prontos sofrem aging
                if (p.estado == Estados.Pronto)
                {
                    p.AgeTicks++;

                    // a cada ciclos, aumenta o tick de envelhecimento
                    if (p.AgeTicks >= 3)
                    {
                        if (p.PrioridadeDinamica < 1) // 1 = prioridade alta
                            p.PrioridadeDinamica++;

                        p.AgeTicks = 0;
                    }
                }
            }
        }



        private void ProcessoTrocadoInvoke()
        {
            try { ProcessoTrocado?.Invoke(); } catch { }
        }
    }
}
