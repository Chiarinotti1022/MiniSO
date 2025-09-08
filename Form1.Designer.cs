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
            components = new System.ComponentModel.Container();
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
            timer1 = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // lvProcessos
            // 
            lvProcessos.Columns.AddRange(new ColumnHeader[] { chPid, chEstado, chPrioridade, chMemoria, chPC });
            lvProcessos.FullRowSelect = true;
            lvProcessos.GridLines = true;
            lvProcessos.Location = new Point(12, 12);
            lvProcessos.Name = "lvProcessos";
            lvProcessos.Size = new Size(560, 426);
            lvProcessos.TabIndex = 0;
            lvProcessos.UseCompatibleStateImageBehavior = false;
            lvProcessos.View = View.Details;
            // 
            // chPid
            // 
            chPid.Text = "PID";
            chPid.Width = 50;
            // 
            // chEstado
            // 
            chEstado.Text = "Estado";
            chEstado.Width = 100;
            // 
            // chPrioridade
            // 
            chPrioridade.Text = "Prioridade";
            chPrioridade.Width = 100;
            // 
            // chMemoria
            // 
            chMemoria.Text = "Memória";
            chMemoria.Width = 80;
            // 
            // chPC
            // 
            chPC.Text = "PC";
            chPC.Width = 80;
            // 
            // buttonCriarProcesso
            // 
            buttonCriarProcesso.Location = new Point(590, 20);
            buttonCriarProcesso.Name = "buttonCriarProcesso";
            buttonCriarProcesso.Size = new Size(180, 30);
            buttonCriarProcesso.TabIndex = 1;
            buttonCriarProcesso.Text = "Criar Processo";
            // 
            // buttonIniciarSO
            // 
            buttonIniciarSO.Location = new Point(590, 60);
            buttonIniciarSO.Name = "buttonIniciarSO";
            buttonIniciarSO.Size = new Size(180, 30);
            buttonIniciarSO.TabIndex = 2;
            buttonIniciarSO.Text = "Iniciar Sistema";
            // 
            // buttonPararSO
            // 
            buttonPararSO.Location = new Point(590, 100);
            buttonPararSO.Name = "buttonPararSO";
            buttonPararSO.Size = new Size(180, 30);
            buttonPararSO.TabIndex = 3;
            buttonPararSO.Text = "Parar Sistema";
            // 
            // progressBarMemoria
            // 
            progressBarMemoria.Location = new Point(590, 160);
            progressBarMemoria.Name = "progressBarMemoria";
            progressBarMemoria.Size = new Size(180, 25);
            progressBarMemoria.TabIndex = 4;
            // 
            // lblMemoria
            // 
            lblMemoria.Location = new Point(590, 140);
            lblMemoria.Name = "lblMemoria";
            lblMemoria.Size = new Size(180, 20);
            lblMemoria.TabIndex = 5;
            lblMemoria.Text = "Uso de Memória: 0%";
            // 
            // Form1
            // 
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
        private System.Windows.Forms.Timer timer1;
    }
}
