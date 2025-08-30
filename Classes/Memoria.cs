using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    internal class Memoria
    {
        public int total { get; set; }
        public int livre { get; set; }  


        public Memoria(int total) { 
            this.total = total;
            this.livre = this.total;

        }


        public bool alocar(int qtd)
        {
            if (livre - qtd >= 0)
            {
                livre -= qtd;
                Console.WriteLine($"Alocado {qtd}");
                return true; // cria o processo
            } else
            {
                Console.WriteLine($"Memória Cheia!");
                return false; // não cria o processo
            }
        }
        public void liberar(int qtd)
        {
            if (livre + qtd <= total) // evita estourar o total da memoria
            {
                livre += qtd;
                Console.WriteLine($"Liberado {qtd}");
            }
            else
            {
                Console.WriteLine("Erro: Liberar mais memória do que o total");
            }

        }
    }
}
