using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloRevista;

namespace ClubedaLeitura.ModuloEmprestimo
{
    public class RepositorioEmprestimo : RepositorioBase<Emprestimo>
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


        public void AtualizarStatusEmprestimos()
        {
            var emprestimos = SelecionarRegistros();

            foreach (var emprestimo in emprestimos)
            {
                if (emprestimo != null)
                    emprestimo.VerificarAtraso();
            }
        }

        public bool RevistaEstaEmprestada(Revista revista)
        {
            return SelecionarRegistros()
                .Any(e => e != null
                       && e.revista != null
                       && e.revista.id == revista.id
                       && (e.status == StatusEmprestimo.Aberto || e.status == StatusEmprestimo.Atrasado));
        }

        public void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0,-5} | {1,-20} | {2,-20} | {3,-12} | {4,-12} | {5,-10}",
                "ID", "Amigo", "Revista", "Empréstimo", "Devolução", "Status");
        }

        public void ImprimirRegistro(Emprestimo e)
        {
            ConsoleColor cor = Console.ForegroundColor;

            if (e.status.ToString() == "Atrasado")
                Console.ForegroundColor = ConsoleColor.Red;
            else if (e.status.ToString() == "Aberto")
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("{0,-5} | {1,-20} | {2,-20} | {3,-12:dd/MM/yyyy} | {4,-12:dd/MM/yyyy} | {5,-10}",
                e.id, e.amigo.nome, e.revista.titulo, e.dataEmprestimo, e.dataDevolucao, e.status);

            Console.ForegroundColor = cor;
        }
    }
}