using MiniSO.Classes;

namespace MiniSO
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            

            
        }

        private void labelteste_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicou");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Processo processo = new Processo(1, Prioridade.Alta, 100);
            txtB.Text = processo.CriarThreads();
        }
    }
}
