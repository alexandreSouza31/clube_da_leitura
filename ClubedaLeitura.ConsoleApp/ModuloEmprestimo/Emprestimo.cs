using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloMulta;
using ClubedaLeitura.ModuloRevista;
using static ClubedaLeitura.ModuloRevista.Revista;

namespace ClubedaLeitura.ModuloEmprestimo
{
    public class Emprestimo : IEntidade
    {
        private static int numeroId = 1;
        public int Id { get; set; }
        public Amigo Amigo { get; set; }
        public Revista Revista { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataDevolucao { get; set; }
        public StatusEmprestimo Status { get; set; }
        public Multa Multa { get; set; }
        public Emprestimo() { }
        public Emprestimo(Amigo amigo, Revista revista, DateTime dataEmprestimo, DateTime dataDevolucao)
        {
            this.Id = numeroId++;
            this.Amigo = amigo;
            this.Revista = revista;
            this.DataEmprestimo = dataEmprestimo;
            this.DataDevolucao = dataDevolucao;
            this.Status = StatusEmprestimo.Aberto;
        }

        public void Concluir()
        {
            this.Status = StatusEmprestimo.Concluido;

            if (this.Revista != null)
                this.Revista.Status = StatusRevista.Disponivel;
        }

        public void VerificarAtraso()
        {
            if (Status == StatusEmprestimo.Aberto && DateTime.Today > DataDevolucao)
                Status = StatusEmprestimo.Atrasado;
        }
    }
        public enum StatusEmprestimo
        {
            Aberto, Concluido, Atrasado
        }
    }