using ClubedaLeitura.Compartilhado;

namespace ClubedaLeitura.ModuloCaixa
{
    public class Caixa : IEntidade
    {
        private static int numeroId = 1;

        public int id { get; set; }
        public string etiqueta { get; set; }
        public string cor { get; set; }
        public int diasEmprestimo { get; set; }

        public Caixa(string etiqueta, string cor, int diasEmprestimo = 7)
        {
            this.id = numeroId++;
            this.etiqueta = etiqueta;
            this.cor = cor;
            this.diasEmprestimo = diasEmprestimo;
        }

        public Caixa() { }

        public override string ToString()
        {
            return etiqueta;
        }
    }
}