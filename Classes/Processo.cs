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
        public int PrioridadeDinamica { get; set; }
        public int AgeTicks { get; set; } = 0;

        public List<Thread> threads { get; set; }
        public int QuantumAtual { get; set; } = 0;

        public Escalonador EscalonadorInstance { get; set; }

        public int SegmentoBase { get; set; } = -1;
        public int SegmentoLimite { get; set; } = -1;
        public List<Segmento> TabelaSegmentos { get; } = new();




        public Processo(int pid, Prioridade prioridade, int tamanho)
        {
            this.pId = pid;
            this.estado = Estados.Pronto;
            this.prioridade = prioridade;
            this.tamanhoMemoria = tamanho;
            this.threads = new List<Thread>();
            this.PrioridadeDinamica = (int)prioridade;
            this.AgeTicks = 0;


        }

        //RR e Prioridade
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

                    // marca executado e pinta na UI
                    if (EscalonadorInstance.threadAtual?.tId != t.tId)
                    {
                        await EscalonadorInstance.RegistrarTrocaDeContextoThread(
                            EscalonadorInstance.threadAtual,
                            t
                        );
                    }


                    t.estado = Estados.Executando;
                    try { onUnitExecuted?.Invoke(); } catch { }

                    // espera pra nao bugar a UI
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

            // se todas as threads finalizaram finaliza o processo (libera memória depois no Sistema)
            if (threads.All(th => th.estado == Estados.Finalizado))
                Finalizar();
            else
                estado = Estados.Pronto;
        }


        //FCFS
        public async Task ExecutarFCFS(int delayPorUnidadeMs = 200, Action onUnitExecuted = null)
        {
            while (threads.Any(t => t.estado != Estados.Finalizado))
            {
                var threadsProntas = threads.Where(t => t.estado == Estados.Pronto).ToList();

                foreach (var t in threadsProntas)
                {
                    while (t.pc < t.countPc)
                    {
                        // entra em execução
                        if (EscalonadorInstance.threadAtual?.tId != t.tId)
                        {
                            await EscalonadorInstance.RegistrarTrocaDeContextoThread(
                                EscalonadorInstance.threadAtual,
                                t
                            );
                        }


                        t.estado = Estados.Executando;
                        bool terminou = t.ExecutarUnidade();

                        // atualiza UI após executar 1 instrução
                        try { onUnitExecuted?.Invoke(); } catch { }

                        // aguarda tempo de CPU
                        if (delayPorUnidadeMs > 0)
                            await Task.Delay(delayPorUnidadeMs);

                        // ajusta estado depois da execução
                        t.estado = terminou ? Estados.Finalizado : Estados.Pronto;
                    }

                    if (t.estado != Estados.Finalizado)
                        FinalizarThread(t);
                }

                if (threads.All(th => th.estado == Estados.Finalizado))
                    Finalizar();
                else
                    estado = Estados.Pronto;
            }
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
        public void Finalizar(Memoria memoria = null)
        {
            estado = Estados.Finalizado;

            // Se recebeu a instância de Memoria, entao, libera os segmentos e limpa estruturas.
            if (memoria != null)
            {
                foreach (var seg in TabelaSegmentos)
                    memoria.Liberar(seg.Base);

                // limpar threads e tabela somente depois de liberar a memória
                threads.Clear();
                TabelaSegmentos.Clear();
            }
            else
            {
                // sistema limpa memoria quando for apropriado
            }

            Console.WriteLine($"Processo: {pId} finalizado (memoria liberada: {memoria != null})");
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

        }
       
        }
    }

