using MiniSO.Classes;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            sistema.IniciarSistema(10000);
            escalonador = new Escalonador("RR", 10);

            // Conecta os eventos dos botões
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
            sistema.executar = true;

            while (sistema.executar)
            {
                escalonador.Escalonar(sistema.processos);

                AtualizarListaProcessos();
                AtualizarMemoria();

                await Task.Delay(500);
            }

            buttonIniciarSO.Enabled = true;
        }

        private void buttonPararSO_Click(object sender, EventArgs e)
        {
            sistema.executar = false;
        }

        private void AtualizarListaProcessos()
        {
            lvProcessos.Items.Clear();

            foreach (var p in sistema.processos)
            {
                var item = new ListViewItem(p.pId.ToString());
                item.SubItems.Add(p.estado.ToString());
                item.SubItems.Add(p.prioridade.ToString());
                item.SubItems.Add(p.tamanhoMemoria.ToString());
                item.SubItems.Add(""); // Threads não usam PC nesta linha
                lvProcessos.Items.Add(item);

                foreach (var t in p.threads)
                {
                    var tItem = new ListViewItem("   └ " + t.tId);
                    tItem.SubItems.Add(t.estado.ToString());
                    tItem.SubItems.Add(""); // Pode colocar prioridade da thread se quiser
                    tItem.SubItems.Add(t.tamanho.ToString());
                    tItem.SubItems.Add($"{t.pc}/{t.countPc}");
                    lvProcessos.Items.Add(tItem);
                }
            }
        }

        private void AtualizarMemoria()
        {
            int totalMemoria = sistema.memoria.total;
            int usada = sistema.processos.Sum(p => p.estado != Estados.Finalizado ? p.tamanhoMemoria : 0);

            progressBarMemoria.Maximum = totalMemoria;
            progressBarMemoria.Value = Math.Min(usada, totalMemoria);

            lblMemoria.Text = $"Uso de Memória: {usada}/{totalMemoria}";
        }
    }
}
