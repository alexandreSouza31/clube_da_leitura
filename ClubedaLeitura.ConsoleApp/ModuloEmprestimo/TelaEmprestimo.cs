using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloRevista;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ModuloEmprestimo
{
    internal class TelaEmprestimo : TelaBase<Emprestimo>
    {
        private readonly RepositorioEmprestimo repositorioEmprestimo;
        private readonly RepositorioAmigo repositorioAmigo;
        private readonly RepositorioRevista repositorioRevista;
        private readonly TelaAmigo telaAmigo;
        private readonly TelaRevista telaRevista;
        private readonly Direcionar direcionar = new();

        public TelaEmprestimo(RepositorioEmprestimo repositorioEmprestimo,
                              RepositorioAmigo repositorioAmigo,
                              RepositorioRevista repositorioRevista,
                              TelaAmigo telaAmigo,
                              TelaRevista telaRevista)
            : base("Empréstimo", repositorioEmprestimo)
        {
            this.repositorioEmprestimo = repositorioEmprestimo;
            this.repositorioAmigo = repositorioAmigo;
            this.repositorioRevista = repositorioRevista;
            this.telaAmigo = telaAmigo;
            this.telaRevista = telaRevista;
        }

        public void ExecutarMenu()
        {
            var menu = new TelaMenuEntidadeBase<Emprestimo>(this);

            bool continuar = true;
            while (continuar)
                continuar = menu.ExecutarMenuEntidade();
        }

        public override Emprestimo CriarInstanciaVazia()
        {
            return new Emprestimo();
        }

        protected override Emprestimo ObterNovosDados(Emprestimo dadosOriginais, bool editar)
        {
            while (true)
            {
                Console.Clear();
                ExibirCabecalho();

                bool haAmigos = telaAmigo.Visualizar(true, false, false);
                if (!haAmigos)
                {
                    direcionar.DirecionarParaMenu(false, true, "Amigo");
                    return null;
                }

                Console.Write("ID do amigo: ");
                string inputAmigo = Console.ReadLine();
                Amigo amigo = repositorioAmigo.SelecionarRegistroPorId(int.Parse(inputAmigo));

                if (amigo == null)
                {
                    Console.WriteLine("Amigo não encontrado!");
                    DigitarEnterEContinuar.Executar();
                    continue;
                }

                if (repositorioEmprestimo.AmigoPossuiEmprestimoAtivo(amigo))
                {
                    Console.WriteLine("Este amigo já possui um empréstimo ativo!");
                    DigitarEnterEContinuar.Executar();
                    return null;
                }

                bool haRevistas = telaRevista.Visualizar(true, false, false);
                if (!haRevistas)
                {
                    direcionar.DirecionarParaMenu(false, true, "Revista");
                    return null;
                }

                Console.Write("ID da revista: ");
                string inputRevista = Console.ReadLine();
                Revista revista = repositorioRevista.SelecionarRegistroPorId(int.Parse(inputRevista));

                if (revista == null)
                {
                    Console.WriteLine("Revista não encontrada!");
                    DigitarEnterEContinuar.Executar();
                    continue;
                }

                if (revista.status.ToString() != "Aberto")
                {
                    Console.WriteLine("Revista não está disponível para empréstimo!");
                    DigitarEnterEContinuar.Executar();
                    return null;
                }

                DateTime dataEmprestimo = DateTime.Today;
                DateTime dataDevolucao = dataEmprestimo.AddDays(revista.caixa?.diasEmprestimo ?? 7);

                return new Emprestimo(amigo, revista, dataEmprestimo, dataDevolucao);
            }
        }


        protected override void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0,-5} | {1,-20} | {2,-20} | {3,-12} | {4,-12} | {5,-10}",
                "ID", "Amigo", "Revista", "Empréstimo", "Devolução", "Status");
        }

        protected override void ImprimirRegistro(Emprestimo e)
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