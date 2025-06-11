using ClubedaLeitura.Compartilhado;

namespace ClubedaLeitura.ModuloCaixa
{
    public class Caixa : IEntidade
    {
        private static int numeroId = 1;

        public int Id { get; set; }
        public string Etiqueta { get; set; }
        public string Cor { get; set; }
        public int DiasEmprestimo { get; set; }

        public Caixa(string etiqueta, string cor, int diasEmprestimo = 7)
        {
            this.Id = numeroId++;
            this.Etiqueta = etiqueta;
            this.Cor = cor;
            this.DiasEmprestimo = diasEmprestimo;
        }

        public Caixa() { }

        public override string ToString()
        {
            return Etiqueta;
        }
    }
}