using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniSO.Classes
{
    public interface INo
    {
        public string Nome { get; }
        public string DataCriacao { get; set; }
        public IDiretorio? Pai { get; }
        public string CaminhoCompleto();
    }

    public interface IDiretorio : INo
    {
        public List<INo> Filhos { get; }
        public int QuantidadeItens { get; set; }

        public void AdicionarFilho(INo no);
        public void RemoverFilho(INo no);
        
    }

    internal class Ficheiro
    {
    }

    public class Diretorio : IDiretorio
    {
        public string Nome { get; set; }
        public IDiretorio? Pai { get; set; }
        public List<INo> Filhos { get; set; }
        public int QuantidadeItens { get; set; }
        public string DataCriacao { get; set; }

        public Diretorio(string nome,  IDiretorio? pai = null)
        {
            Nome = nome;
            Pai = pai;
            Filhos = new List<INo>();
            DataCriacao = DateTime.Now.ToString();
        }

        public void AdicionarFilho(INo no)
        {
            Filhos.Add(no);
            QuantidadeItens = Filhos.Count();
        }
        public void RemoverFilho(INo no) { 
            Filhos.Remove(no);
            QuantidadeItens = Filhos.Count();
        }

        public string CaminhoCompleto()
        {
            return Pai == null ? Nome : $"{Pai.CaminhoCompleto()}/{Nome}";
        }

    }

    public class Arquivo : INo
    {
        public string Nome { get; set; }
        public IDiretorio? Pai { get; set; }
        public string Conteudo { get; set; }
        public string DataCriacao { get; set; }

        public Arquivo(string nome, IDiretorio? pai, string conteudo)
        {
            Nome = nome;
            Pai = pai;
            Conteudo = string.Empty;
            DataCriacao = DateTime.Now.ToString();
        }

        public void Escrever(string conteudo)
        {
            this.Conteudo = conteudo;
        }

        public string CaminhoCompleto()
        {
            return Pai == null ? Nome : $"{Pai.CaminhoCompleto()}/{Nome}";
        }
    }
}
