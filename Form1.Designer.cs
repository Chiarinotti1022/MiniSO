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
            SuspendLayout();
            // 
            // lvProcessos
            // 
            lvProcessos.Columns.AddRange(new ColumnHeader[] { chPid, chEstado, chPrioridade, chMemoria, chPC });
            lvProcessos.FullRowSelect = true;
            lvProcessos.GridLines = true;
            lvProcessos.Location = new Point(12, 12);
            lvProcessos.Name = "lvProcessos";
            lvProcessos.Size = new Size(560, 300);
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
            buttonIniciarSO.Click += buttonIniciarSO_Click_1;
            // 
            // buttonPararSO
            // 
            buttonPararSO.Location = new Point(590, 100);
            buttonPararSO.Name = "buttonPararSO";
            buttonPararSO.Size = new Size(180, 30);
            buttonPararSO.TabIndex = 3;
            buttonPararSO.Text = "Parar Sistema";
            buttonPararSO.Click += buttonPararSO_Click_1;
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
            // lstLog
            // 
            lstLog.ItemHeight = 15;
            lstLog.Location = new Point(12, 320);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(560, 109);
            lstLog.TabIndex = 6;
            // 
            // Form1
            // 
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(800, 450);
            Controls.Add(lstLog);
            Controls.Add(lvProcessos);
            Controls.Add(buttonCriarProcesso);
            Controls.Add(buttonIniciarSO);
            Controls.Add(buttonPararSO);
            Controls.Add(progressBarMemoria);
            Controls.Add(lblMemoria);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "Form1";
            RightToLeftLayout = true;
            Text = "MiniSO - Simulador de Sistema Operacional";
            // cbPolitica
            cbPolitica = new ComboBox();
            cbPolitica.DropDownStyle = ComboBoxStyle.DropDownList;
            cbPolitica.Items.AddRange(new object[] { "RR", "PRIORIDADE" });
            cbPolitica.SelectedIndex = 0; // RR por padrão
            cbPolitica.Location = new Point(590, 200);
            cbPolitica.Name = "cbPolitica";
            cbPolitica.Size = new Size(180, 23);
            cbPolitica.TabIndex = 7;
            Controls.Add(cbPolitica);
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
        private ListBox lstLog;
        private ComboBox cbPolitica;
    }
}
