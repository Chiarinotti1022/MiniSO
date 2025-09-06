namespace MiniSO
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lvProcessos = new ListView();
            chPid = new ColumnHeader();
            chEstado = new ColumnHeader();
            chPrioridade = new ColumnHeader();
            chMemoria = new ColumnHeader();
            chPC = new ColumnHeader();
            buttonCriarProcesso = new Button();
            buttonIniciarSO = new Button();
            buttonPararSO = new Button();
            progressBarMemoria = new ProgressBar();
            lblMemoria = new Label();
            SuspendLayout();

            // ListView
            lvProcessos.Location = new Point(12, 12);
            lvProcessos.Size = new Size(560, 426);
            lvProcessos.View = View.Details;
            lvProcessos.FullRowSelect = true;
            lvProcessos.GridLines = true;
            lvProcessos.Columns.AddRange(new ColumnHeader[] { chPid, chEstado, chPrioridade, chMemoria, chPC });

            chPid.Text = "PID";
            chPid.Width = 50;
            chEstado.Text = "Estado";
            chEstado.Width = 100;
            chPrioridade.Text = "Prioridade";
            chPrioridade.Width = 100;
            chMemoria.Text = "Memória";
            chMemoria.Width = 80;
            chPC.Text = "PC";
            chPC.Width = 80;

            // Buttons
            buttonCriarProcesso.Text = "Criar Processo";
            buttonCriarProcesso.Location = new Point(590, 20);
            buttonCriarProcesso.Size = new Size(180, 30);
            //buttonCriarProcesso.Click += buttonCriarProcesso_Click;

            buttonIniciarSO.Text = "Iniciar Sistema";
            buttonIniciarSO.Location = new Point(590, 60);
            buttonIniciarSO.Size = new Size(180, 30);
            //buttonIniciarSO.Click += buttonIniciarSO_Click;

            buttonPararSO.Text = "Parar Sistema";
            buttonPararSO.Location = new Point(590, 100);
            buttonPararSO.Size = new Size(180, 30);
            //buttonPararSO.Click += buttonPararSO_Click;

            // ProgressBar e Label
            progressBarMemoria.Location = new Point(590, 160);
            progressBarMemoria.Size = new Size(180, 25);

            lblMemoria.Location = new Point(590, 140);
            lblMemoria.Size = new Size(180, 20);
            lblMemoria.Text = "Uso de Memória: 0%";

            // Form
            ClientSize = new Size(800, 450);
            Controls.Add(lvProcessos);
            Controls.Add(buttonCriarProcesso);
            Controls.Add(buttonIniciarSO);
            Controls.Add(buttonPararSO);
            Controls.Add(progressBarMemoria);
            Controls.Add(lblMemoria);
            Name = "Form1";
            Text = "MiniSO - Simulador de Sistema Operacional";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView lvProcessos;
        private ColumnHeader chPid;
        private ColumnHeader chEstado;
        private ColumnHeader chPrioridade;
        private ColumnHeader chMemoria;
        private ColumnHeader chPC;
        private Button buttonCriarProcesso;
        private Button buttonIniciarSO;
        private Button buttonPararSO;
        private ProgressBar progressBarMemoria;
        private Label lblMemoria;
    }
}
