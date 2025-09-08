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
            escalonador = new Escalonador("RR", 10);
            sistema.IniciarSistema(10000);

            // Evento para atualizar lista sempre que escalonador trocar de processo
            escalonador.ProcessoTrocado += () =>
            {
                BeginInvoke(new Action(() =>
                {
                    AtualizarListaProcessos();
                    AtualizarMemoria();
                }));
            };

            // Eventos dos botões
            buttonCriarProcesso.Click += buttonCriarProcesso_Click;
            buttonIniciarSO.Click += buttonIniciarSO_Click;
            buttonPararSO.Click += buttonPararSO_Click;
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

        private async void buttonIniciarSO_Click(object sender, EventArgs e)
        {
            buttonIniciarSO.Enabled = false;

            while (!IsDisposed)
            {
                // chama o escalonador com delay de 1 segundo
                await sistema.escalonador.Escalonar(sistema.processos, 1000);

                // Atualiza visual após cada troca de processo
                AtualizarListaProcessos();
                AtualizarMemoria();
            }


            buttonIniciarSO.Enabled = true;
        }

        private void buttonPararSO_Click(object sender, EventArgs e)
        {
            sistema.EncerrarSistema();
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
            int totalMemoria = sistema.memoria.total;
            int usada = totalMemoria - sistema.memoria.livre;

            progressBarMemoria.Maximum = totalMemoria;
            progressBarMemoria.Value = Math.Min(usada, totalMemoria);

            lblMemoria.Text = $"Uso de Memória: {usada}/{totalMemoria}";
        }
    }
}
