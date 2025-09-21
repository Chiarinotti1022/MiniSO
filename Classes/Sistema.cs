using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static MiniSO.Classes.Processador;

namespace MiniSO.Classes
{
    internal class Sistema
    {
        public Memoria memoria { get; set; }
        public Processador processador { get; set; }
        public Escalonador escalonador { get; set; }

        private readonly Random rand = new Random();
        public List<Processo> processos = new List<Processo>();
        public CancellationTokenSource cts;
        public readonly object ProcessosLock = new object();

        private readonly ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true); // set = running, reset = paused

        // estado do gerador
        private Task geradorTask;
        private CancellationTokenSource geradorCts;

        // evento quando um processo finaliza (opcional)
        public event Action<Processo>? ProcessoFinalizado;

        // evento quando um processo é desbloqueado (memória disponível)
        public event Action<Processo>? ProcessoDesbloqueado;

        public bool IsStarted => cts != null && !cts.IsCancellationRequested;
        public bool IsPaused => !pauseEvent.IsSet;

        public void IniciarSistema(int memoriaTotal, int autoCriarIntervalMs = 0, string politica = "RR")
        {
            // se já iniciado, não reinicia outro loop
            if (cts != null && !cts.IsCancellationRequested) return;

            escalonador = new Escalonador(politica, 10); // passa politica escolhida
            memoria = new Memoria(memoriaTotal);
            cts = new CancellationTokenSource();
            pauseEvent.Set(); // garante que esteja em modo "running"

            Task.Run(async () =>
            {
                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        // aguarda se estiver pausado
                        pauseEvent.Wait(cts.Token);

                        // pega finalizados
                        List<Processo> finalizados;
                        lock (ProcessosLock)
                        {
                            finalizados = processos.FindAll(p => p.estado == Estados.Finalizado);
                        }

                        if (finalizados.Count > 0)
                        {
                            // notifica antes de remover
                            foreach (var fin in finalizados)
                            {
                                try { ProcessoFinalizado?.Invoke(fin); } catch { }
                            }

                            // remove e libera memória
                            lock (ProcessosLock)
                            {
                                foreach (var fin in finalizados)
                                {
                                    memoria.liberar(fin.tamanhoMemoria);
                                    processos.Remove(fin);
                                }
                            }

                           //após liberar, tenta desbloquear processos bloqueados
                            List<Processo> desbloqueados = new List<Processo>();
                            lock (ProcessosLock)
                            {
                                // copia a lista para evitar modificar durante iteração
                                var bloqueados = processos.Where(p => p.estado == Estados.Bloqueado).ToList();
                                foreach (var b in bloqueados)
                                {
                                    // tenta alocar agora que liberamos memória
                                    if (memoria.alocar(b.tamanhoMemoria))
                                    {
                                        b.estado = Estados.Pronto;
                                        desbloqueados.Add(b);
                                    }
                                    else
                                    {
                                        // se não foi possível alocar para esse bloqueado, provavelmente não há mais memória
                                        // continue para próximos (ou break); aqui continuamos para tentar todos
                                    }
                                }
                            }

                            // notifica fora do lock para não bloquear o loop
                            foreach (var d in desbloqueados)
                            {
                                try { ProcessoDesbloqueado?.Invoke(d); } catch { }
                            }
                        }


                        // verifica se tem processos; se não, espera e volta (não encerra)
                        bool temProcessos;
                        lock (ProcessosLock)
                        {
                            temProcessos = processos.Count > 0;
                        }

                        if (!temProcessos)
                        {
                            await Task.Delay(300, cts.Token);
                            continue;
                        }

                        // snapshot para escalonador
                        List<Processo> snapshot;
                        lock (ProcessosLock)
                        {
                            snapshot = new List<Processo>(processos);
                        }

                        // antes de chamar escalonador, respeita pause
                        pauseEvent.Wait(cts.Token);
                        await escalonador.Escalonar(snapshot, 1000);

                        await Task.Delay(300, cts.Token);
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Erro no loop do Sistema: " + ex);
                }
            }, cts.Token);

            // inicia gerador automático se solicitado
            if (autoCriarIntervalMs > 0)
            {
                StartGerador(autoCriarIntervalMs);
            }
        }

        public void EncerrarSistema()
        {
            try { StopGerador(); } catch { }
            cts?.Cancel();
        }

        // ---------- Pause / Resume ----------
        public void PauseSistema()
        {
            pauseEvent.Reset(); // trava o loop e o gerador
        }

        public void ResumeSistema()
        {
            pauseEvent.Set();   // libera o loop e o gerador
        }

        // ---------- Gerador controlável ----------
        public void StartGerador(int autoCriarIntervalMs)
        {
            // se já rodando, ignora
            if (geradorTask != null && !geradorTask.IsCompleted && geradorCts != null && !geradorCts.IsCancellationRequested)
                return;

            geradorCts = new CancellationTokenSource();

            geradorTask = Task.Run(async () =>
            {
                try
                {
                    while (!geradorCts.Token.IsCancellationRequested && !cts.Token.IsCancellationRequested)
                    {
                        // respeita pause
                        pauseEvent.Wait(geradorCts.Token);

                        int pid = rand.Next(1, 100000);
                        Prioridade pr = (Prioridade)rand.Next(0, 2);
                        int tamanho = rand.Next(50, 300);

                        try
                        {
                            // CriarProcesso já faz lock e alocação de memória
                            var p = CriarProcesso(pid, pr, tamanho);
                        }
                        catch { }

                        await Task.Delay(autoCriarIntervalMs, geradorCts.Token);
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine("Gerador erro: " + ex); }
            }, geradorCts.Token);
        }

        public void StopGerador()
        {
            try { geradorCts?.Cancel(); } catch { }
            geradorTask = null;
            geradorCts = null;
        }

        // ---------- Criar processo ----------
        public Processo CriarProcesso(int pid, Prioridade prioridade, int tamanho)
        {
            lock (ProcessosLock)
            {
                if (memoria == null) return null; // sistema não iniciado

                Processo p = new Processo(pid, prioridade, tamanho);
                p.CriarThreads();

                // tenta alocar memória; se der certo => Pronto; senão => Bloqueado
                if (memoria.alocar(tamanho))
                {
                    p.estado = Estados.Pronto;
                }
                else
                {
                    p.estado = Estados.Bloqueado;
                    // não aloca nada e mantém o processo na lista aguardando memória
                    // (você já terá log de bloqueio se quiser)
                }

                processos.Add(p);
                return p;
            }
        }

    }
}
