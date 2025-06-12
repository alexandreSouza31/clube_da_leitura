using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloRevista;

namespace ClubedaLeitura.ModuloReserva
{
    public class Reserva : IEntidade
    {
        private static int contadorId = 1;
        public int Id { get; set; }
        public Amigo Amigo { get; set; }
        public Revista Revista { get; set; }
        public DateTime DataReserva { get; set; }
        public StatusReserva Status { get; set; }

        public Reserva(Amigo amigo, Revista revista)
        {
            this.Id = contadorId++;
            this.Amigo = amigo;
            this.Revista = revista;
            this.DataReserva = DateTime.Today;
            this.Status = StatusReserva.Ativa;
        }

        public void Concluir()
        {
            this.Status = StatusReserva.Concluida;
        }
    }

    public enum StatusReserva
    {
        Ativa, Concluida
    }
}