using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloMulta;
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
        private readonly RepositorioMulta repositorioMulta;
        private readonly TelaAmigo telaAmigo;
        private readonly TelaRevista telaRevista;
        private readonly Direcionar direcionar = new();

        public TelaEmprestimo(RepositorioEmprestimo repositorioEmprestimo,
                              RepositorioAmigo repositorioAmigo,
                              RepositorioRevista repositorioRevista,
                              RepositorioMulta repositorioMulta,
                              TelaAmigo telaAmigo,
                              TelaRevista telaRevista)
            : base("Empréstimo", repositorioEmprestimo)
        {
            this.repositorioEmprestimo = repositorioEmprestimo;
            this.repositorioAmigo = repositorioAmigo;
            this.repositorioRevista = repositorioRevista;
            this.telaAmigo = telaAmigo;
            this.telaRevista = telaRevista;
            this.repositorioMulta = repositorioMulta;
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

                bool haRevistas = telaRevista.Visualizar(true, false, false, r => r.status == StatusRevista.Disponivel);
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

                if (revista.status != StatusRevista.Disponivel)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Existe um empréstimo em andamento para essa revista!");
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar();
                    return null;
                }

                DateTime dataEmprestimo = DateTime.Today;
                DateTime dataDevolucao = dataEmprestimo.AddDays(revista.caixa?.diasEmprestimo ?? 7);
                //DateTime dataDevolucao = new DateTime(2025, 5, 5); forçando atraso

                revista.status = StatusRevista.Emprestada;
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
            ImprimirCabecalhoTabela();
            foreach (var e in emprestimosAbertos)
                ImprimirRegistro(e);

            Console.Write("\nDigite o ID do empréstimo a devolver: ");
            int id = int.Parse(Console.ReadLine()!);

            var emprestimo = repositorioEmprestimo.SelecionarRegistroPorId(id);

            if (emprestimo == null || (emprestimo.status != StatusEmprestimo.Aberto && emprestimo.status != StatusEmprestimo.Atrasado))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Empréstimo não encontrado ou já concluído!");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, nomeEntidade);
                return;
            }

            if (emprestimo.status == StatusEmprestimo.Atrasado)
            {
                var multasExistentes = repositorioMulta.ObterRegistro(repositorioMulta.SelecionarRegistros(), emprestimo.amigo, m => m.amigo);
                bool jaTemMulta = multasExistentes.Any(m => m.emprestimo.id == emprestimo.id);

                if (!jaTemMulta)
                {
                    Multa novaMulta = new Multa(emprestimo, emprestimo.amigo, emprestimo.revista);
                    repositorioMulta.CadastrarRegistro(novaMulta);
                }
            }

            var todasMultas = repositorioMulta.SelecionarRegistros();
            var multasDoAmigo = repositorioMulta.ObterRegistro(todasMultas, emprestimo.amigo, m => m.amigo);

            bool temMultaPendente = multasDoAmigo != null && multasDoAmigo.Any(m => m.status == Multa.StatusMulta.Pendente);

            if (temMultaPendente)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Este amigo possui multas pendentes! Quite-as antes de devolver o empréstimo.");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, nomeEntidade);
                return;
            }

            emprestimo.Concluir();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Devolução registrada com sucesso!");
            Console.ResetColor();

            direcionar.DirecionarParaMenu(true, false, nomeEntidade);
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