using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloRevista;
using ClubedaLeitura.Utils;
using static ClubedaLeitura.ModuloRevista.Revista;

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

                Console.WriteLine();
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Este amigo já possui um empréstimo ativo!");
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar();
                    return null;
                }

                bool haRevistas = telaRevista.Visualizar(true, false, false, r => r.Status == StatusRevista.Disponivel);
                if (!haRevistas)
                {
                    direcionar.DirecionarParaMenu(false, true, "Revista");
                    return null;
                }

                Console.WriteLine();
                Console.Write("ID da revista: ");
                string inputRevista = Console.ReadLine();
                Revista revista = repositorioRevista.SelecionarRegistroPorId(int.Parse(inputRevista));

                if (revista == null)
                {
                    Console.WriteLine("Revista não encontrada!");
                    DigitarEnterEContinuar.Executar();
                    continue;
                }

                if (revista.Status != StatusRevista.Disponivel)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Existe um empréstimo em andamento para essa revista!");
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar();
                    return null;
                }

                DateTime dataEmprestimo = DateTime.Today;
                DateTime dataDevolucao = dataEmprestimo.AddDays(revista.Caixa?.DiasEmprestimo ?? 7);
                #region forçando atraso para testes
                //DateTime dataDevolucao = new DateTime(2025, 5, 31);
                #endregion forçando atraso

                revista.Status = StatusRevista.Emprestada;
                repositorioEmprestimo.AtualizarStatusEmprestimos();
                return new Emprestimo(amigo, revista, dataEmprestimo, dataDevolucao);
            }
        }

        public void CadastrarDevolucao()
        {
            ExibirCabecalho("Cadastrar Devolução");

            var emprestimosAbertos = repositorioEmprestimo.SelecionarEmprestimosAbertos();

            if (emprestimosAbertos.Count == 0)
            {
                direcionar.DirecionarParaMenu(false, false, nomeEntidade);
                return;
            }

            Console.WriteLine("\nEmpréstimos Abertos:");
            repositorioEmprestimo.AtualizarStatusEmprestimos();

            ImprimirCabecalhoTabela();
            foreach (var e in emprestimosAbertos)
                ImprimirRegistro(e);

            Console.Write("\nDigite o ID do empréstimo a devolver: ");
            int id = int.Parse(Console.ReadLine()!);

            var emprestimo = repositorioEmprestimo.SelecionarRegistroPorId(id);

            if (emprestimo == null || (emprestimo.Status != StatusEmprestimo.Aberto && emprestimo.Status != StatusEmprestimo.Atrasado))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Empréstimo não encontrado ou já concluído!");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, nomeEntidade);
                return;
            }

            bool multaQuitada = QuitarMulta(emprestimo);
            if (!multaQuitada) return;

            emprestimo.Concluir();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Devolução registrada com sucesso!");
            Console.ResetColor();

            direcionar.DirecionarParaMenu(true, false, nomeEntidade);
        }

        private bool QuitarMulta(Emprestimo emprestimo)
        {
            repositorioEmprestimo.GerarMultaSeAtrasado(emprestimo);

            if (emprestimo.Multa != null && !emprestimo.Multa.EstaPaga)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n{emprestimo.Amigo.Nome} possui uma multa de R$ {emprestimo.Multa.Valor:F2}.");
                Console.Write("Deseja quitar agora? (s/n): ");
                Console.ResetColor();

                string? resposta = Console.ReadLine();

                if (resposta?.ToLower() != "s")
                {                  
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Multa pendente!");
                    Console.ResetColor();
                    direcionar.DirecionarParaMenu(true, false, "Empréstimo");
                    return false;
                }

                emprestimo.Multa.Pagar();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("Multa quitada com sucesso!");
                Console.WriteLine();

                Console.ResetColor();
                return true;
            }
            return true;
        }


        protected override void ImprimirCabecalhoTabela()
        {
            repositorioEmprestimo.ImprimirCabecalhoTabela();
        }

        protected override void ImprimirRegistro(Emprestimo e)
        {
            repositorioEmprestimo.AtualizarStatusEmprestimos();
            repositorioEmprestimo.ImprimirRegistro(e);
        }
    }
}