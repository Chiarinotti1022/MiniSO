using MiniSO.Classes;

internal class Escalonador
{
    public string politica { get; set; }
    public int quantum { get; set; }
    private Queue<Processo> filaRR = new Queue<Processo>();
    public event Action? ProcessoTrocado; // evento que Form vai ouvir

    public Escalonador(string politica, int quantum)
    {
        this.politica = politica;
        this.quantum = quantum;
    }

    public async Task Escalonar(List<Processo> processos, int delayMs)
    {
        if (politica != "RR") return;

        if (processos == null || processos.Count == 0)
            return;

        // Enfileira novos processos
        foreach (var p in processos.Where(p => p.estado == Estados.Pronto && !filaRR.Contains(p)))
            filaRR.Enqueue(p);

        if (filaRR.Count == 0) return;

        var processo = filaRR.Dequeue();
        processo.ExecutarRR(quantum);

        ProcessoTrocado?.Invoke();

        // só re-enfileira se NÃO tiver finalizado
        if (processo.estado == Estados.Pronto)
            filaRR.Enqueue(processo);

        await Task.Delay(delayMs);
    }
}
