using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Escalonador
    {
        public string politica { get; set; }
        public int? quantum { get; set;}

        public Escalonador(string politica, int? quantum )
        {
            this.politica = politica;
            this.quantum = quantum;
        }

        public void Escalonar(List<Processo> processos)
        {
                List<Processo> processosProntos = processos.Where(p => p.estado == Estados.Pronto).ToList();
                List<Processo> processosBloqueados = processos.Where(p => p.estado == Estados.Bloqueado).ToList();
                List<Processo> processosExecutando = processos.Where(p => p.estado == Estados.Executando).ToList();

                if (politica == "RR")
                {
                    if (!quantum.HasValue)
                    {
                        //Valor default caso quantum não seja passado por parâmetro
                        quantum = 100;
                    }

                    foreach (var p in processosProntos)
                    {
                        p.ExecutarRR(quantum.Value);
                    }

                    

                }




                foreach (var p in processosBloqueados)
                {
                    if (new Random().NextDouble() > 0.5)
                    {
                        p.estado = Estados.Pronto;
                    }
                }

                foreach (var p in processosProntos)
                {
                    if (new Random().NextDouble() > 0.8)
                    {
                        p.estado = Estados.Bloqueado;
                    }
                }
            }

            


        }
      
    }

