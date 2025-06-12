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
                    (e.Status == StatusEmprestimo.Aberto || e.Status == StatusEmprestimo.Atrasado))
                .ToList();
        }

        public List<Emprestimo> SelecionarEmprestimosFechados()
        {
            return SelecionarRegistros()
                .Where(e => e.Status.ToString() == "Concluído")
                .ToList();
        }


        public bool AmigoPossuiEmprestimoAtivo(Amigo amigo)
        {
            return SelecionarRegistros()
                .Any(e => e != null
                       && e.Amigo != null
                       && e.Status != null
                       && e.Amigo.Id == amigo.Id
                       && (e.Status.ToString() == "Aberto" || e.Status.ToString() == "Atrasado"));
        }


        public void AtualizarStatusEmprestimos()
        {
            var emprestimos = SelecionarRegistros();

            foreach (var emprestimo in emprestimos)
            {
                if (emprestimo == null)
                    continue;

                GerarMultaSeAtrasado(emprestimo);
            }
        }

        public void GerarMultaSeAtrasado(Emprestimo emprestimo)
        {
            if (emprestimo.Status == StatusEmprestimo.Aberto && DateTime.Today > emprestimo.DataDevolucao)
            {
                emprestimo.Status = StatusEmprestimo.Atrasado;

                if (emprestimo.Multa == null)
                {
                    TimeSpan atraso = DateTime.Today.Subtract(emprestimo.DataDevolucao);
                    decimal valorMulta = 2.00m * atraso.Days;
                    emprestimo.Multa = new Multa(valorMulta);
                }
            }
        }

        public bool RevistaEstaEmprestada(Revista revista)
        {
            return SelecionarRegistros()
                .Any(e => e != null
                       && e.Revista != null
                       && e.Revista.Id == revista.Id
                       && (e.Status == StatusEmprestimo.Aberto || e.Status == StatusEmprestimo.Atrasado));
        }

        public void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0,-5} | {1,-20} | {2,-20} | {3,-12} | {4,-12} | {5,-10} | {6,-10}",
                "ID", "Amigo", "Revista", "Empréstimo", "Devolução", "Status", "Multa");
        }

        public void ImprimirRegistro(Emprestimo e)
        {
            ConsoleColor cor = Console.ForegroundColor;

            if (e.Status.ToString() == "Atrasado")
                Console.ForegroundColor = ConsoleColor.Red;
            else if (e.Status.ToString() == "Aberto")
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            string valorMulta = e.Multa != null ? $"R$ {e.Multa.Valor:F2}" : "-";

            Console.WriteLine("{0,-5} | {1,-20} | {2,-20} | {3,-12:dd/MM/yyyy} | {4,-12:dd/MM/yyyy} | {5,-10} | {6,-10}",
                e.Id, e.Amigo.Nome, e.Revista.Titulo, e.DataEmprestimo, e.DataDevolucao, e.Status, valorMulta);

            Console.ForegroundColor = cor;
        }
    }
}