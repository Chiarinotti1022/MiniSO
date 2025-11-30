using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniSO.Classes
{
    internal class Memoria
    {
        public int Total { get; }
        private readonly List<Segmento> segmentos;

        public Memoria(int total)
        {
            Total = total;
            segmentos = new List<Segmento>();
        }

        public IReadOnlyList<Segmento> Segmentos => segmentos;

        public int Alocar(int tamanho)
        {
            if (tamanho <= 0) return -1;

            // ORDENAR POR BASE
            segmentos.Sort((a, b) => a.Base.CompareTo(b.Base));

            // 1) TENTAR BURACO ANTES DO PRIMEIRO SEGMENTO
            if (segmentos.Count == 0)
            {
                if (tamanho <= Total)
                {
                    var seg = new Segmento(0, tamanho);
                    segmentos.Add(seg);
                    return seg.Base;
                }
                return -1;
            }

            if (segmentos[0].Base >= tamanho)
            {
                var seg = new Segmento(0, tamanho);
                segmentos.Add(seg);
                return seg.Base;
            }

            // 2) TENTAR BURACOS ENTRE SEGMENTOS
            for (int i = 0; i < segmentos.Count - 1; i++)
            {
                int fimAtual = segmentos[i].Fim;
                int inicioProx = segmentos[i + 1].Base;

                int buraco = inicioProx - fimAtual;

                if (buraco >= tamanho)
                {
                    var seg = new Segmento(fimAtual, tamanho);
                    segmentos.Add(seg);
                    return seg.Base;
                }
            }

            // 3) TENTAR APÓS O ÚLTIMO SEGMENTO
            var ultimo = segmentos[^1];
            if (ultimo.Fim + tamanho <= Total)
            {
                var seg = new Segmento(ultimo.Fim, tamanho);
                segmentos.Add(seg);
                return seg.Base;
            }

            // SEM ESPAÇO
            return -1;
        }

        public void Liberar(int baseAddr)
        {
            var seg = segmentos.FirstOrDefault(s => s.Base == baseAddr);
            if (seg != null)
            {
                segmentos.Remove(seg);
            }
        }
    }
}
