using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloRevista;

namespace ClubedaLeitura.ModuloEmprestimo
{
    public class Emprestimo : IEntidade
    {
        private static int numeroId = 1;
        public int id { get; set; }
        public Amigo amigo { get; set; }
        public Revista revista { get; set; }
        public DateTime dataEmprestimo { get; set; }
        public DateTime dataDevolucao { get; set; }
        public StatusEmprestimo status { get; set; }

        public Emprestimo() { }
        public Emprestimo(Amigo amigo, Revista revista, DateTime dataEmprestimo, DateTime dataDevolucao)
        {
            this.id = numeroId++;
            this.amigo = amigo;
            this.revista = revista;
            this.dataEmprestimo = dataEmprestimo;
            this.dataDevolucao = dataDevolucao;
            this.status = StatusEmprestimo.Aberto;
        }

        public void Concluir()
        {
            status = StatusEmprestimo.Concluido;
        }

        public void VerificarAtraso()
        {
            if (status == StatusEmprestimo.Aberto && DateTime.Today > dataDevolucao)
                status = StatusEmprestimo.Atrasado;
        }
    }

    public enum StatusEmprestimo
    {
        Aberto,Concluido,Atrasado
    }
}