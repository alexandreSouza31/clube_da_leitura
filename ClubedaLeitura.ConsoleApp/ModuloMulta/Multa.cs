using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloEmprestimo;
using ClubedaLeitura.ModuloRevista;
using static ClubedaLeitura.ModuloRevista.Revista;

namespace ClubedaLeitura.ModuloMulta
{
    public class Multa : IEntidade
    {
        private static int numeroId = 1;

        public int id { get; set; }
        public decimal valorAcumulado { get; set; }
        public StatusMulta status { get; set; }

        public Emprestimo emprestimo { get; set; }
        public Amigo amigo { get; set; }
        public Revista revista { get; set; }

        public Multa() { }
        public Multa(Emprestimo emprestimo, Amigo amigo, Revista revista)
        {
            this.id = numeroId++;
            this.emprestimo = emprestimo;
            this.amigo = amigo;
            this.revista = revista;
            this.status = StatusMulta.Pendente;

            VerificarAtraso();
        }

        public void VerificarAtraso()
        {
            if (emprestimo == null)
                return;

            if (DateTime.Today > emprestimo.dataDevolucao)
            {
                int diasAtraso = (DateTime.Today - emprestimo.dataDevolucao).Days;
                valorAcumulado = diasAtraso * 2.00m;
            }
        }

        public void Quitar()
        {
            status = StatusMulta.Quitada;

            if (revista != null) revista.status = StatusRevista.Disponivel;
        }

        public enum StatusMulta
        {
            Pendente,
            Quitada
        }
    }
}