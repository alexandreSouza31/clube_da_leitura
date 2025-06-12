using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloRevista;

namespace ClubedaLeitura.ModuloReserva
{
    public class RepositorioReserva : RepositorioBase<Reserva>
    {
        public bool ExisteReservaAtivaParaRevista(Revista revista)
        {
            return registros.Any(r =>
                r != null &&
                r.Revista != null &&
                r.Revista.Id == revista.Id &&
                r.Status == StatusReserva.Ativa);
        }


        public List<Reserva> SelecionarReservasAtivas()
        {
            return registros.Where(r => r != null && r.Status == StatusReserva.Ativa).ToList();
        }
    }
}