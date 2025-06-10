using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloEmprestimo;
using ClubedaLeitura.ModuloRevista;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ModuloMulta
{
    class TelaMulta : TelaBase<Multa>
    {
        private readonly RepositorioEmprestimo repositorioEmprestimo;
        private readonly RepositorioAmigo repositorioAmigo;
        private readonly RepositorioRevista repositorioRevista;
        private readonly RepositorioMulta repositorioMulta;

        private readonly TelaEmprestimo telaEmprestimo;
        private readonly TelaAmigo telaAmigo;
        private readonly TelaRevista telaRevista;

        Direcionar direcionar = new Direcionar();

        public TelaMulta(RepositorioMulta repositorioMulta,
                        RepositorioEmprestimo repositorioEmprestimo,
                        RepositorioAmigo repositorioAmigo,
                        RepositorioRevista repositorioRevista,
                        TelaEmprestimo telaEmprestimo,
                        TelaAmigo telaAmigo,
                        TelaRevista telaRevista)
            : base("Multa", repositorioMulta)
        {
            this.repositorioMulta = repositorioMulta;
            this.repositorioEmprestimo = repositorioEmprestimo;
            this.repositorioAmigo = repositorioAmigo;
            this.repositorioRevista = repositorioRevista;
            this.telaEmprestimo = telaEmprestimo;
            this.telaAmigo = telaAmigo;
            this.telaRevista = telaRevista;
        }

        public void ExecutarMenu()
        {
            foreach (var multa in repositorioMulta.SelecionarRegistros())
            {
                if (multa != null)
                    multa.VerificarAtraso();
            }

            VerificarEMultarAtrasos();

            var menu = new TelaMenuEntidadeBase<Multa>(this);
            bool continuar = true;
            while (continuar)
                continuar = menu.ExecutarMenuEntidade();
        }



        public override Multa CriarInstanciaVazia()
        {
            return new Multa();
        }

        protected override Multa ObterNovosDados(Multa multa, bool edicao)
        {
            ExibirCabecalho("Cadastro de Multa");

            telaEmprestimo.Visualizar(false, false, false);
            Console.Write("Digite o ID do empréstimo relacionado: ");
            int idEmprestimo = Convert.ToInt32(Console.ReadLine());

            var emprestimo = repositorioEmprestimo.SelecionarRegistroPorId(idEmprestimo);
            if (emprestimo == null)
            {
                Console.WriteLine("Empréstimo inválido.");
                return multa;
            }

            multa.emprestimo = emprestimo;
            multa.amigo = emprestimo.amigo;
            multa.revista = emprestimo.revista;
            multa.status = Multa.StatusMulta.Pendente;

            int diasAtraso = (DateTime.Today - emprestimo.dataDevolucao).Days;
            multa.valorAcumulado = diasAtraso > 0 ? diasAtraso * 2 : 0;

            return multa;
        }

        public void VerificarEMultarAtrasos()
        {
            var emprestimos = repositorioEmprestimo.SelecionarRegistros();

            foreach (var emprestimo in emprestimos)
            {
                if (emprestimo == null || emprestimo.amigo == null || emprestimo.revista == null)
                    continue;

                if (emprestimo.status == StatusEmprestimo.Aberto &&
                    DateTime.Today > emprestimo.dataDevolucao)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    var multas = repositorioMulta.SelecionarRegistros();

                    bool haMulta = multas.Any(m => m != null && m.emprestimo != null && m.emprestimo.id == emprestimo.id);

                    if (!haMulta)
                    {
                        Multa novaMulta = new Multa(
                            emprestimo,
                            emprestimo.amigo,
                            emprestimo.revista
                        );

                        repositorioMulta.CadastrarRegistro(novaMulta);

                    }
                    Console.ResetColor();
                }
            }
        }


        public void QuitarMulta()
        {
            repositorioEmprestimo.AtualizarStatusEmprestimos();
            ExibirCabecalho("Quitar");

            var multasPendentes = repositorioMulta.SelecionarMultasPendentes();

            if (multasPendentes.Count == 0)
            {
                direcionar.DirecionarParaMenu(false, false, nomeEntidade);
                return;
            }

            Console.WriteLine("\nMultas Pendentes:");
            ImprimirCabecalhoTabela();

            foreach (var e in multasPendentes)
            {
                ImprimirRegistro(e);
            }

            int id = EntradaHelper.ObterEntrada<int>("Digite o ID da multa a quitar", 0, true);

            var multa = repositorio.SelecionarRegistroPorId(id);

            if (multa == null || multa.status != Multa.StatusMulta.Pendente)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Multa não encontrada ou já quitada!");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, nomeEntidade);
                return;
            }

            multa.Quitar();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Quitação registrada com sucesso!");
            Console.ResetColor();

            direcionar.DirecionarParaMenu(true, false, nomeEntidade);
        }

        public bool VisualizarMultaAmigo()
        {
            repositorioEmprestimo.AtualizarStatusEmprestimos();
            Visualizar(true, false, false);
            int idAmigo = EntradaHelper.ObterEntrada<int>("Digite o ID do Amigo", 0, true);

            var amigo = repositorioAmigo.SelecionarRegistroPorId(idAmigo);

            if (amigo == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Amigo não encontrado!");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, "amigo");
                return false;
            }

            List<Multa> multasDoAmigo = Multa.ObterMultas(amigo, repositorioMulta.SelecionarRegistros())
                .Where(m => m.status == Multa.StatusMulta.Pendente &&
                            m.emprestimo.status != StatusEmprestimo.Concluido)
                .ToList();


            if (multasDoAmigo.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Este amigo não possui multas.");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, "amigo");
                return false;
            }

            Console.Clear();
            ExibirCabecalho();
            ImprimirCabecalhoTabela();

            foreach (var multa in multasDoAmigo)
                ImprimirRegistro(multa);

            DigitarEnterEContinuar.Executar();

            return true;
        }

        protected override void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0,-5} | {1,-20} | {2,-20} | {3,-12} | {4,-12} | {5,-10} | {6,-10}",
                "ID", "Amigo", "Revista", "Empréstimo", "Devolução", "Status", "Valor");
        }

        protected override void ImprimirRegistro(Multa m)
        {
            Emprestimo e = repositorioEmprestimo.SelecionarRegistros()
                .FirstOrDefault(emp => emp == m.emprestimo)!;

            if (e == null) return;

            ConsoleColor cor = Console.ForegroundColor;

            if (m.status == Multa.StatusMulta.Pendente)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("{0,-5} | {1,-20} | {2,-20} | {3,-12:dd/MM/yyyy} | {4,-12:dd/MM/yyyy} | {5,-10} | {6,-10}",
                m.id, m.amigo.nome, m.revista.titulo, e.dataEmprestimo, e.dataDevolucao, m.status, m.valorAcumulado);

            Console.ForegroundColor = cor;
        }
    }
}
