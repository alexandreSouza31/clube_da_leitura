using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloEmprestimo;

namespace ClubedaLeitura.ModuloAmigo
{
    public class Amigo : IEntidade
    {
        private static int numeroId = 1;

        public int id { get; set; }
        public string nome { get; set; }
        public string nomeResponsavel { get; set; }
        public string telefone { get; set; }

        public Amigo(string nome, string nomeResponsavel, string telefone)
        {
            this.id = numeroId++;
            this.nome = nome;
            this.nomeResponsavel = nomeResponsavel;
            this.telefone = telefone;
        }

        public Amigo() { }

        public override string ToString()
        {
            return nome;
        }

        public List<Emprestimo> ObterEmprestimos(Emprestimo[] todosEmprestimos)
        {
            return todosEmprestimos
                    .Where(e => e != null && e.amigo.id == this.id)
                    .ToList();
        }
    }
}