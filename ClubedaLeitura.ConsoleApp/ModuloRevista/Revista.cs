using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloCaixa;

namespace ClubedaLeitura.ModuloRevista
{
    public class Revista : IEntidade
    {
        private static int numeroId = 1;

        public int id { get; set; }
        public string titulo { get; set; }
        public int numeroEdicao { get; set; }
        public int anoPublicacao { get; set; }
        public Caixa caixa { get; set; }

        public Revista(string titulo, int numeroEdicao, int anoPublicacao, Caixa caixa)
        {
            this.id = numeroId++;
            this.titulo = titulo;
            this.numeroEdicao = numeroEdicao;
            this.anoPublicacao = anoPublicacao;
            this.caixa = caixa;
        }

        public Revista() { }

        public override string ToString()
        {
            return titulo;
        }
    }
}