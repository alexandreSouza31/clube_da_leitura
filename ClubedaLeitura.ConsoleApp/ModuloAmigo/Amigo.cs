using ClubedaLeitura.Compartilhado;

namespace ClubedaLeitura.ModuloAmigo
{
    public class Amigo : IEntidade
    {
        private static int numeroId = 1;

        public int Id { get; set; }
        public string Nome { get; set; }
        public string NomeResponsavel { get; set; }
        public string Telefone { get; set; }

        public Amigo(string nome, string nomeResponsavel, string telefone)
        {
            this.Id = numeroId++;
            this.Nome = nome;
            this.NomeResponsavel = nomeResponsavel;
            this.Telefone = telefone;
        }

        public Amigo() { }

        public override string ToString()
        {
            return Nome;
        }     
    }
}