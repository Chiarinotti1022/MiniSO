using MiniSO.Classes;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace MiniSO
{
    public partial class Form1 : Form
    {
        Sistema sistema;
        Escalonador escalonador;

        public Form1()
        {
            InitializeComponent();

            sistema = new Sistema();

            buttonCriarProcesso.Click += buttonCriarProcesso_Click;
        }

        private void buttonCriarProcesso_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int pid = rand.Next(1, 1000);
            Prioridade prioridade = (Prioridade)rand.Next(0, 2);
            int tamanho = rand.Next(50, 300);

            sistema.CriarProcesso(pid, prioridade, tamanho);
            AtualizarListaProcessos();
            AtualizarMemoria();
        }

        private void AtualizarListaProcessos()
        {
            lvProcessos.BeginUpdate();
            lvProcessos.Items.Clear();

            var processosOrdenados = sistema.processos
                .OrderByDescending(p => p.estado == Estados.Executando ? 3 :
                                        p.estado == Estados.Pronto ? 2 :
                                        p.estado == Estados.Bloqueado ? 1 : 0)
                .ThenBy(p => p.pId);

            foreach (var p in processosOrdenados)
            {
                var item = new ListViewItem($"P{p.pId}");
                item.SubItems.Add(p.estado.ToString());
                item.SubItems.Add(p.prioridade.ToString());
                item.SubItems.Add(p.tamanhoMemoria.ToString());
                item.SubItems.Add(p.QuantumAtual.ToString());

                switch (p.estado)
                {
                    case Estados.Executando: item.BackColor = Color.LightGreen; break;
                    case Estados.Pronto: item.BackColor = Color.LightYellow; break;
                    case Estados.Bloqueado: item.BackColor = Color.LightCoral; break;
                    case Estados.Finalizado: item.BackColor = Color.LightGray; break;
                }

                lvProcessos.Items.Add(item);

                // Threads do processo
                foreach (var t in p.threads)
                {
                    var tItem = new ListViewItem($"  └ T{t.tId}");
                    tItem.SubItems.Add(t.estado.ToString());
                    tItem.SubItems.Add(""); // prioridade opcional
                    tItem.SubItems.Add(t.tamanho.ToString());
                    tItem.SubItems.Add($"{t.pc}/{t.countPc}");

                    switch (t.estado)
                    {
                        case Estados.Executando: tItem.BackColor = Color.LightGreen; break;
                        case Estados.Pronto: tItem.BackColor = Color.LightYellow; break;
                        case Estados.Bloqueado: tItem.BackColor = Color.LightCoral; break;
                        case Estados.Finalizado: tItem.BackColor = Color.LightGray; break;
                    }

                    lvProcessos.Items.Add(tItem);
                }
            }

            lvProcessos.EndUpdate();
        }

        private void AtualizarMemoria()
        {
            if (sistema.memoria == null) return;

            int totalMemoria = sistema.memoria.total;
            int usada = totalMemoria - sistema.memoria.livre;

            progressBarMemoria.Maximum = totalMemoria;
            progressBarMemoria.Value = Math.Min(usada, totalMemoria);

            lblMemoria.Text = $"Uso de Memória: {usada}/{totalMemoria}";
        }

        private void buttonIniciarSO_Click_1(object sender, EventArgs e)
        {
            buttonIniciarSO.Enabled = false;

            if (sistema.escalonador == null)
            {
                // ativa gerador automático a cada 2s como exemplo
                sistema.IniciarSistema(10000, autoCriarIntervalMs: 2000);
                escalonador = sistema.escalonador;

                escalonador.ProcessoTrocado -= OnProcessoTrocado;
                escalonador.ProcessoTrocado += OnProcessoTrocado;

                sistema.ProcessoFinalizado -= OnProcessoFinalizado;
                sistema.ProcessoFinalizado += OnProcessoFinalizado;
            }

            // garante o texto correto do botão de pausa
            buttonPararSO.Text = sistema.IsPaused ? "Continuar Sistema" : "Parar Sistema";

            // opcional: cria um processo inicial antes do loop rodar no background
            if (sistema.processos.Count == 0)
                buttonCriarProcesso_Click(sender, e);

            // monitora cancelamento para reabilitar o botão
            Task.Run(async () =>
            {
                while (sistema.cts != null && !sistema.cts.Token.IsCancellationRequested)
                    await Task.Delay(500);

                BeginInvoke(new Action(() =>
                {
                    buttonIniciarSO.Enabled = true;
                }));
            });
        }

        private void OnProcessoTrocado()
        {
            BeginInvoke(new Action(() =>
            {
                AtualizarListaProcessos();
                AtualizarMemoria();
            }));
        }

        private void OnProcessoFinalizado(Processo p)
        {
            BeginInvoke(new Action(() =>
            {
                // adiciona ao log visual (ListBox)
                if (lstLog != null)
                {
                    lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] Processo P{p.pId} finalizado (mem: {p.tamanhoMemoria})");
                    // mantém o último item visível
                    lstLog.TopIndex = lstLog.Items.Count - 1;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[LOG] Processo P{p.pId} finalizado");
                }

                AtualizarListaProcessos();
                AtualizarMemoria();
            }));
        }

        private void buttonPararSO_Click_1(object sender, EventArgs e)
        {
            if (sistema == null) return;

            // se sistema não iniciado ainda, ignora
            if (!sistema.IsStarted)
            {
                // talvez você queira iniciar o sistema ao clicar aqui; por enquanto apenas retorna
                return;
            }

            if (!sistema.IsPaused)
            {
                // pausa a simulação (e o gerador)
                sistema.PauseSistema();
                buttonPararSO.Text = "Continuar Sistema";
            }
            else
            {
                // retoma a simulação
                sistema.ResumeSistema();
                buttonPararSO.Text = "Parar Sistema";
            }
        }
    }
}
