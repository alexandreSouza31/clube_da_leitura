using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;

namespace ClubedaLeitura.ModuloMulta
{
    public class RepositorioMulta : RepositorioBase<Multa>
    {
        public List<Multa> SelecionarMultasPendentes()
        {
            return SelecionarRegistros()
                .Where(m => m != null && m.status == Multa.StatusMulta.Pendente)
                .ToList();
        }

        public List<Multa> SelecionarMultasPorAmigo(Amigo amigo)
        {
            return SelecionarRegistros()
                .Where(m => m != null && m.amigo != null && m.amigo.id == amigo.id)
                .ToList();
        }

        public bool AmigoPossuiMultasPendentes(Amigo amigo)
        {
            return SelecionarRegistros()
                .Any(m => m != null && m.amigo != null && m.amigo.id == amigo.id && m.status == Multa.StatusMulta.Pendente);
        }
    }
}