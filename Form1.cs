using MiniSO.Classes;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Reflection;

namespace MiniSO
{
    public partial class Form1 : Form
    {
        Sistema sistema;
        Escalonador escalonador;

        private int contadorTrocas = 0;

        public Form1()
        {
            InitializeComponent();

            sistema = new Sistema();

            // handlers
            buttonCriarProcesso.Click += buttonCriarProcesso_Click;
            cbPolitica.SelectedIndexChanged += cbPolitica_SelectedIndexChanged;
            lvProcessos.SelectedIndexChanged += lvProcessos_SelectedIndexChanged;

            // inicializa combo
            cbPolitica.Items.Clear();
            cbPolitica.Items.AddRange(new object[] { "RR", "PRIORIDADE", "FCFS" });
            cbPolitica.SelectedIndex = 0;
        }

        private void buttonCriarProcesso_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            int pid = rand.Next(1, 100000);
            Prioridade prioridade = (Prioridade)rand.Next(0, 2);
            int tamanho = rand.Next(50, 300);

            var p = sistema.CriarProcesso(pid, prioridade, tamanho);

            // log compacto (opção 2)
            if (p == null)
            {
                AddLog($"[{DateTime.Now:HH:mm:ss}] Erro ao criar processo.");
            }
            else
            {
                if (p.estado == Estados.Pronto)
                {
                    string segInfo = GetSegmentInfoCompact(p);
                    AddLog($"[{DateTime.Now:HH:mm:ss}] [P{p.pId}] Alocado: {segInfo}");
                }
                else if (p.estado == Estados.Bloqueado)
                {
                    string segInfo = GetSegmentInfoCompact(p);
                    AddLog($"[{DateTime.Now:HH:mm:ss}] [P{p.pId}] BLOQUEADO ao criar (mem: {p.tamanhoMemoria})");
                }
                else
                {
                    AddLog($"[{DateTime.Now:HH:mm:ss}] [P{p.pId}] Criado estado={p.estado} (mem: {p.tamanhoMemoria})");
                }
            }

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
                item.SubItems.Add(p.AgeTicks.ToString());

                switch (p.estado)
                {
                    case Estados.Executando: item.BackColor = Color.LightGreen; break;
                    case Estados.Pronto: item.BackColor = Color.LightYellow; break;
                    case Estados.Bloqueado: item.BackColor = Color.Orange; break;
                    case Estados.Finalizado: item.BackColor = Color.LightGray; break;
                }

                lvProcessos.Items.Add(item);

                // Threads do processo (mostra a mesma info de antes)
                foreach (var t in p.threads)
                {
                    var tItem = new ListViewItem($"  └ T{t.tId}");
                    tItem.SubItems.Add(t.estado.ToString());
                    tItem.SubItems.Add(""); // prioridade opcional
                    tItem.SubItems.Add(t.tamanho.ToString());
                    tItem.SubItems.Add($"{t.pc}/{t.countPc} ");

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

            int totalMemoria = sistema.memoria.Total;

            // soma de todos os limites já alocados
            int usada = sistema.memoria.Segmentos.Sum(s => s.Limite);

            progressBarMemoria.Maximum = totalMemoria;
            progressBarMemoria.Value = Math.Min(usada, totalMemoria);

            lblMemoria.Text = $"Uso de Memória: {usada}/{totalMemoria}";
        }


        private void buttonIniciarSO_Click_1(object sender, EventArgs e)
        {
            buttonIniciarSO.Enabled = false;
            numericUpDown1.Enabled = false;

            contadorTrocas = 0;
            if (lblTrocas != null)
                lblTrocas.Text = "Trocas de Contexto: 0";

            if (sistema.escalonador == null)
            {
                // ativa gerador automático a cada 5s
                string politica = "RR";
                if (cbPolitica != null && cbPolitica.SelectedItem != null)
                    politica = cbPolitica.SelectedItem.ToString();

                if (sistema.escalonador == null)
                {
                    int quantumInicial = (int)numericUpDown1.Value;
                    if (quantumInicial <= 0)
                        quantumInicial = 10;
                    sistema.IniciarSistema(1024, autoCriarIntervalMs: 5000, politica: politica, quantum: quantumInicial);

                    escalonador = sistema.escalonador;
                    numericUpDown1.Value = escalonador.quantum;
                    if (numericOverhead != null)
                    {
                        escalonador.TempoDeTrocaDeContexto = (int)numericOverhead.Value;
                    }
                }
                else
                {
                    // se o sistema já está rodando e o usuário mudou a combo, atualiza a política do escalonador em runtime
                    sistema.escalonador.politica = politica;
                }
                escalonador = sistema.escalonador;

                // conecta eventos do sistema para log e UI
                escalonador.ProcessoTrocado -= OnProcessoTrocado;
                escalonador.ProcessoTrocado += OnProcessoTrocado;

                sistema.ProcessoFinalizado -= OnProcessoFinalizado;
                sistema.ProcessoFinalizado += OnProcessoFinalizado;

                sistema.ProcessoDesbloqueado -= OnProcessoDesbloqueado;
                sistema.ProcessoDesbloqueado += OnProcessoDesbloqueado;
            }

            // garante o texto correto do botão de pausa
            buttonPararSO.Text = sistema.IsPaused ? "Continuar Sistema" : "Parar Sistema";

            //cria um processo inicial antes do loop rodar no background
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
                    numericUpDown1.Enabled = true;
                }));
            });
        }

        private void OnProcessoTrocado()
        {
            BeginInvoke(new Action(() =>
            {
                contadorTrocas++;
                lblTrocas.Text = $"Trocas de Contexto: {contadorTrocas}";

                AtualizarListaProcessos();
                AtualizarMemoria();
            }));
        }

        private void OnProcessoFinalizado(Processo p)
        {
            BeginInvoke(new Action(() =>
            {
                // compact log: processo finalizado + segmentos (se houver)
                string segInfo = GetSegmentInfoCompact(p);
                if (lstLog != null)
                {
                    lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] [P{p.pId}] Finalizado (mem: {p.tamanhoMemoria}) {segInfo}");
                    lstLog.TopIndex = Math.Max(0, lstLog.Items.Count - 1);
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
                return;
            }

            if (!sistema.IsPaused)
            {
                // pausa a simulação e o gerador
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

        private void OnProcessoDesbloqueado(Processo p)
        {
            BeginInvoke(new Action(() =>
            {
                string segInfo = GetSegmentInfoCompact(p);
                if (lstLog != null)
                {
                    lstLog.Items.Add($"[{DateTime.Now:HH:mm:ss}] [P{p.pId}] DESBLOQUEADO -> Alocado: {segInfo}");
                    if (lstLog.Items.Count > 0) lstLog.TopIndex = lstLog.Items.Count - 1;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[LOG] Processo P{p.pId} desbloqueado");
                }

                AtualizarListaProcessos();
                AtualizarMemoria();
            }));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int q = (int)numericUpDown1.Value;
            if (q <= 0) q = 10;

            if (escalonador != null)
                escalonador.quantum = q;

            if (sistema != null && sistema.escalonador != null)
                sistema.escalonador.quantum = q;
        }

        private void cbPolitica_SelectedIndexChanged(object sender, EventArgs e)
        {
            string politica = cbPolitica.SelectedItem?.ToString();

            if (politica == "FCFS")
            {
                numericUpDown1.Enabled = false;
                numericUpDown1.Value = 0;
            }
            else
            {
                numericUpDown1.Enabled = true;
                if (numericUpDown1.Value <= 0)
                    numericUpDown1.Value = 10;
            }
        }

        // configuração de tempo de troca de contexto
        private void numericOverhead_ValueChanged(object sender, EventArgs e)
        {
            if (escalonador != null)
                escalonador.TempoDeTrocaDeContexto = (int)numericOverhead.Value;

            if (sistema != null && sistema.escalonador != null)
                sistema.escalonador.TempoDeTrocaDeContexto = (int)numericOverhead.Value;
        }

        private void lvProcessos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // opcional: exibir detalhes do processo selecionado no log quando clicado
            if (lvProcessos.SelectedItems.Count == 0) return;
            var text = lvProcessos.SelectedItems[0].Text; // ex: "P23" ou "  └ T2"
            if (!text.StartsWith("P")) return;

            int pid;
            if (!int.TryParse(text.Substring(1), out pid)) return;

            var p = sistema.processos.FirstOrDefault(x => x.pId == pid);
            if (p == null) return;

            string segInfo = GetSegmentInfoCompact(p);
            AddLog($"[{DateTime.Now:HH:mm:ss}] [P{p.pId}] Detalhes: estado={p.estado} mem={p.tamanhoMemoria} {segInfo}");
        }

        // ---------- Helpers de log / reflexão ----------

        private void AddLog(string linha)
        {
            if (lstLog != null)
            {
                lstLog.Items.Add(linha);
                lstLog.TopIndex = Math.Max(0, lstLog.Items.Count - 1);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(linha);
            }
        }

        /// <summary>
        /// Tenta extrair informação de segmentos do Processo (via reflexão) e retorna em formato compacto:
        /// Ex: "Base=320 Limit=140; Base=460 Limit=60"
        /// Se não encontrar, retorna "mem:<tamanho>"
        /// </summary>
        private string GetSegmentInfoCompact(Processo p)
        {
            if (p == null) return "";

            try
            {
                var type = p.GetType();

                // busca por propriedade que tenha "segment" no nome (Segmentos, TabelaSegmentos, Segments...)
                var segProp = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .FirstOrDefault(pr => pr.Name.IndexOf("segment", StringComparison.OrdinalIgnoreCase) >= 0
                                                     || pr.Name.IndexOf("segmento", StringComparison.OrdinalIgnoreCase) >= 0
                                                     || pr.Name.IndexOf("table", StringComparison.OrdinalIgnoreCase) >= 0);

                if (segProp != null)
                {
                    var segVal = segProp.GetValue(p);
                    if (segVal is IEnumerable segEnumerable)
                    {
                        var parts = new System.Collections.Generic.List<string>();
                        foreach (var seg in segEnumerable)
                        {
                            if (seg == null) continue;
                            
                            var segType = seg.GetType();
                            var baseProp = segType.GetProperty("Base") ?? segType.GetProperty("base") ?? segType.GetProperty("Start") ?? segType.GetProperty("Address");
                            var limitProp = segType.GetProperty("Limite") ?? segType.GetProperty("limite") ?? segType.GetProperty("Length") ?? segType.GetProperty("Size");

                            var baseVal = baseProp != null ? baseProp.GetValue(seg)?.ToString() : null;
                            var limitVal = limitProp != null ? limitProp.GetValue(seg)?.ToString() : null;

                            if (baseVal != null || limitVal != null)
                            {
                                parts.Add($"Base={baseVal ?? "?"} Limit={limitVal ?? "?"}");
                            }
                            else
                            {
                                // fallback: ToString()
                                parts.Add(seg.ToString());
                            }
                        }

                        if (parts.Count > 0)
                            return string.Join(" ; ", parts);
                    }
                }

                // fallback: procurar propriedades individuais no Processo (SegmentBase / SegmentLimit)
                var baseP = type.GetProperty("SegmentoBase") ?? type.GetProperty("BaseSegmento");
                var limitP = type.GetProperty("SegmentoLimite") ?? type.GetProperty("LimiteSegmento");
                if (baseP != null && limitP != null)
                {
                    var b = baseP.GetValue(p)?.ToString();
                    var l = limitP.GetValue(p)?.ToString();
                    return $"Base={b} Limit={l}";
                }

                // fallback final: talvez exista LastSegmentFault ou LastFault a mostrar
                var lastFaultProp = type.GetProperty("LastSegmentFault") ?? type.GetProperty("LastFault");
                if (lastFaultProp != null)
                {
                    var fault = lastFaultProp.GetValue(p);
                    if (fault != null)
                        return $"fault={fault}";
                }
            }
            catch { /* ignore reflexão */ }

            // se nada encontrado, retorna simplesmente o tamanho
            return $"mem:{p.tamanhoMemoria}";
        }
    }
}
