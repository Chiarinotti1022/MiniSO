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
            lstLog = new ListBox();
            cbPolitica = new ComboBox();
            numericUpDown1 = new NumericUpDown();
            Quantum = new Label();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // lvProcessos
            // 
            lvProcessos.Columns.AddRange(new ColumnHeader[] { chPid, chEstado, chPrioridade, chMemoria, chPC });
            lvProcessos.FullRowSelect = true;
            lvProcessos.GridLines = true;
            lvProcessos.Location = new Point(198, 12);
            lvProcessos.Name = "lvProcessos";
            lvProcessos.Size = new Size(417, 426);
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
            buttonCriarProcesso.Location = new Point(12, 84);
            buttonCriarProcesso.Name = "buttonCriarProcesso";
            buttonCriarProcesso.Size = new Size(180, 30);
            buttonCriarProcesso.TabIndex = 1;
            buttonCriarProcesso.Text = "Criar Processo";
            // 
            // buttonIniciarSO
            // 
            buttonIniciarSO.Location = new Point(12, 12);
            buttonIniciarSO.Name = "buttonIniciarSO";
            buttonIniciarSO.Size = new Size(180, 30);
            buttonIniciarSO.TabIndex = 2;
            buttonIniciarSO.Text = "Iniciar Sistema";
            buttonIniciarSO.Click += buttonIniciarSO_Click_1;
            // 
            // buttonPararSO
            // 
            buttonPararSO.Location = new Point(12, 48);
            buttonPararSO.Name = "buttonPararSO";
            buttonPararSO.Size = new Size(180, 30);
            buttonPararSO.TabIndex = 3;
            buttonPararSO.Text = "Parar Sistema";
            buttonPararSO.Click += buttonPararSO_Click_1;
            // 
            // progressBarMemoria
            // 
            progressBarMemoria.Location = new Point(12, 315);
            progressBarMemoria.Name = "progressBarMemoria";
            progressBarMemoria.Size = new Size(180, 25);
            progressBarMemoria.TabIndex = 4;
            // 
            // lblMemoria
            // 
            lblMemoria.Location = new Point(12, 292);
            lblMemoria.Name = "lblMemoria";
            lblMemoria.Size = new Size(180, 20);
            lblMemoria.TabIndex = 5;
            lblMemoria.Text = "Uso de Memória: 0%";
            // 
            // lstLog
            // 
            lstLog.Location = new Point(621, 12);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(445, 424);
            lstLog.TabIndex = 6;
            // 
            // cbPolitica
            // 
            cbPolitica.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPolitica.Items.AddRange(new object[] { "RR", "PRIORIDADE" });
            cbPolitica.SelectedIndex = 0; // ⬅ aqui o default
            cbPolitica.Location = new Point(12, 120);
            cbPolitica.Name = "cbPolitica";
            cbPolitica.Size = new Size(180, 28);
            cbPolitica.TabIndex = 7;
            

            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(118, 158);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(74, 27);
            numericUpDown1.TabIndex = 8;
            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // Quantum
            // 
            Quantum.AutoSize = true;
            Quantum.Location = new Point(24, 160);
            Quantum.Name = "Quantum";
            Quantum.Size = new Size(73, 20);
            Quantum.TabIndex = 9;
            Quantum.Text = "Quantum:";
           
            // 
            // Form1
            // 
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1095, 450);
            Controls.Add(Quantum);
            Controls.Add(numericUpDown1);
            Controls.Add(lstLog);
            Controls.Add(lvProcessos);
            Controls.Add(buttonCriarProcesso);
            Controls.Add(buttonIniciarSO);
            Controls.Add(buttonPararSO);
            Controls.Add(progressBarMemoria);
            Controls.Add(lblMemoria);
            Controls.Add(cbPolitica);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "Form1";
            RightToLeftLayout = true;
            Text = "MiniSO - Simulador de Sistema Operacional";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
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
        private ListBox lstLog;
        private ComboBox cbPolitica;
        private NumericUpDown numericUpDown1;
        private Label Quantum;
    }
}
