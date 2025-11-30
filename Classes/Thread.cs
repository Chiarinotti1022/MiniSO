using System;

namespace MiniSO.Classes
{
    internal class Thread
    {
        public int tId { get; set; }
        public int pIdPai { get; set; }
        public int tamanho { get; set; }
        public Estados estado { get; set; }
        public Prioridade prioridade { get; set; }

        public int pc { get; set; }
        public int countPc { get; set; }

        public int SegmentBase { get; set; }
        public int SegmentLimit { get; set; }

        public Thread(int tid, int pidPai, int tamanho, Prioridade prioridade, int countPc)
        {
            this.prioridade = prioridade;
            this.estado = Estados.Pronto;
            this.tamanho = tamanho;
            this.tId = tid;
            this.pIdPai = pidPai;
            this.countPc = countPc;

            SegmentBase = -1;  // ainda não alocado
            SegmentLimit = 0;
        }

        public bool ExecutarUnidade()
        {
            // Verificação de memória
            if (SegmentBase >= 0)
            {
                int endereco = pc;

                if (endereco < 0 || endereco >= SegmentLimit)
                {
                    Console.WriteLine(
                        $"Erro de memória na Thread {tId}: endereço {endereco} fora dos limites (limit={SegmentLimit})."
                    );
                    return true; // encerra thread por falha de memória
                }
            }

            // Execução normal
            if (pc < countPc)
                pc++;

            return pc >= countPc;
        }
    }
}
