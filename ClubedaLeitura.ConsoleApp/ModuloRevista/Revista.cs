using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloCaixa;
using ClubedaLeitura.ModuloReserva;

namespace ClubedaLeitura.ModuloRevista
{
    public class Revista : IEntidade
    {
        private static int numeroId = 1;

        public int Id { get; set; }
        public string Titulo { get; set; }
        public int NumeroEdicao { get; set; }
        public int AnoPublicacao { get; set; }
        public Caixa Caixa { get; set; }
        public StatusRevista Status { get; set; } = StatusRevista.Disponivel;

        public Revista(string titulo, int numeroEdicao, int anoPublicacao, Caixa caixa)
        {
            this.Id = numeroId++;
            this.Titulo = titulo;
            this.NumeroEdicao = numeroEdicao;
            this.AnoPublicacao = anoPublicacao;
            this.Caixa = caixa;
            this.Status = StatusRevista.Disponivel;
        }

        public Revista() { }

        public override string ToString()
        {
            return Titulo;
        }

        public enum StatusRevista
        {
            Disponivel, Emprestada, Reservada
        }

        public bool PossuiReservaAtiva(List<Reserva> reservas)
        {
            return reservas.Any(r => r.Revista.Id == this.Id && r.Status == StatusReserva.Ativa);
        }
    }
}