using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloCaixa;
using ClubedaLeitura.ModuloEmprestimo;
using ClubedaLeitura.ModuloRevista;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ModuloAmigo
{
    public class TelaAmigo : TelaBase<Amigo>
    {
        private RepositorioAmigo repositorioAmigo;
        private RepositorioEmprestimo repositorioEmprestimo;
        private RepositorioCaixa repositorioCaixa;
        private RepositorioRevista repositorioRevista;

        Direcionar direcionar = new Direcionar();

        public TelaAmigo(RepositorioAmigo repositorioAmigo, RepositorioEmprestimo repositorioEmprestimo)
            : base("Amigo", repositorioAmigo)
        {
            this.repositorioAmigo = repositorioAmigo;
            this.repositorioEmprestimo = repositorioEmprestimo;
            this.repositorioCaixa = repositorioCaixa;
            this.repositorioRevista = repositorioRevista;
        }

        public void ExecutarMenu()
        {
            var menuAmigo = new TelaMenuEntidadeBase<Amigo>(this);

            bool continuar = true;
            while (continuar)
            {
                continuar = menuAmigo.ExecutarMenuEntidade(true);
            }
        }

        public override Amigo CriarInstanciaVazia()
        {
            return new Amigo();
        }

        protected override Amigo ObterNovosDados(Amigo dadosOriginais, bool editar)
        {
            while (true)
            {
                Visualizar(false, false, false);
                ExibirCabecalho();

                if (editar)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("************* Caso não queira alterar um campo, pressione Enter para mantê-lo.");
                    Console.ResetColor();
                }

                string nome = EntradaHelper.ObterEntrada("Nome", dadosOriginais.nome, editar);
                string email = EntradaHelper.ObterEntrada("Nome Responsável", dadosOriginais.nomeResponsavel, editar);
                string telefone = EntradaHelper.ObterEntrada("Telefone", dadosOriginais.telefone, editar);

                string[] nomesCampos = { "nome", "nome responsável", "telefone" };
                string[] valoresCampos = { nome, email, telefone };
                string erros = ValidarCampo.ValidarCampos(nomesCampos, valoresCampos);
                var registros = repositorioAmigo.SelecionarRegistros();

                string erroDuplicado = ValidarCampo.ValidarDuplicidadeAmigo(nome, telefone, registros, dadosOriginais.id);
                erros += erroDuplicado;

                if (!string.IsNullOrEmpty(erros))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nErros encontrados:");
                    Console.WriteLine(erros);
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar();
                    Console.Clear();
                    continue;
                }
                return new Amigo(nome, email, telefone);
            }
        }

        public bool VisualizarEmprestimoAmigo()
        {
            repositorioEmprestimo.AtualizarStatusEmprestimos();

            Visualizar(true, false, false);
            Console.WriteLine();

            int idAmigo = EntradaHelper.ObterEntrada<int>("Digite o ID do Amigo", 0, true);

            Amigo amigo = repositorioAmigo.SelecionarRegistroPorId(idAmigo);

            if (amigo == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Amigo não encontrado!");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true,false,"amigo");
                return false;
            }

            List<Emprestimo> emprestimosDoAmigo = amigo.ObterEmprestimos(repositorioEmprestimo.SelecionarRegistros());

            if (emprestimosDoAmigo.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Este amigo não possui empréstimos.");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, "amigo");
                return false;
            }
            else
            {

                Console.Clear();
                ExibirCabecalho();
                repositorioEmprestimo.ImprimirCabecalhoTabela();
                foreach (var emprestimo in emprestimosDoAmigo)
                {
                    repositorioEmprestimo.ImprimirRegistro(emprestimo);
                }
            }

            DigitarEnterEContinuar.Executar();

            return true;
        }

        public static void AtualizarAmigo(Amigo original, Amigo novosDados)
        {
            original.nome = novosDados.nome;
            original.nomeResponsavel = novosDados.nomeResponsavel;
            original.telefone = novosDados.telefone;
        }

        public override bool PossuiRegistroVinculado(int idRegistro)
        {
            var amigo = repositorioAmigo.SelecionarRegistroPorId(idRegistro);
            if (amigo == null)
                return false;

            List<Emprestimo> emprestimos = amigo.ObterEmprestimos(repositorioEmprestimo.SelecionarRegistros());

            if (emprestimos.Any(e=>e.status != StatusEmprestimo.Concluido))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Não é possível excluir: este amigo possui empréstimo(s) vinculado(s)(as)!");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, "Amigo");
                return true;
            }
            return false;
        }

        protected override void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -25} | {3, -15}",
                "id".ToUpper(), "nome".ToUpper(), "nome responsável".ToUpper(), "telefone".ToUpper());
        }

        protected override void ImprimirRegistro(Amigo a)
        {
            if (a != null)
            {
                Console.WriteLine("{0, -5} | {1, -20} | {2, -25} | {3, -15}",
                a.id, a.nome, a.nomeResponsavel, a.telefone);
            }
        }
    }
}