using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;

namespace ClubedaLeitura.ModuloEmprestimo
{
    class RepositorioEmprestimo : RepositorioBase<Emprestimo>
    {
        public List<Emprestimo> SelecionarEmprestimosAbertos()
        {
            return SelecionarRegistros()
                .Where(e => e != null &&
                    (e.status == StatusEmprestimo.Aberto || e.status == StatusEmprestimo.Atrasado))
                .ToList();
        }

        public List<Emprestimo> SelecionarEmprestimosFechados()
        {
            return SelecionarRegistros()
                .Where(e => e.status.ToString() == "Concluído")
                .ToList();
        }


        public bool AmigoPossuiEmprestimoAtivo(Amigo amigo)
        {
            return SelecionarRegistros()
                .Any(e => e != null
                       && e.amigo != null
                       && e.status != null
                       && e.amigo.id == amigo.id
                       && (e.status.ToString() == "Aberto" || e.status.ToString() == "Atrasado"));
        }
    }
}