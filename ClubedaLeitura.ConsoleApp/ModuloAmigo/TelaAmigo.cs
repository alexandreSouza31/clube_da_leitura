using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloEmprestimo;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ModuloAmigo
{
    public class TelaAmigo : TelaBase<Amigo>
    {
        private RepositorioAmigo repositorioAmigo;
        RepositorioEmprestimo repositorioEmprestimo = new RepositorioEmprestimo();

        public TelaAmigo(RepositorioAmigo? repositorioAmigo = null)
            : base("Amigo", repositorioAmigo ?? new RepositorioAmigo())
        {
            this.repositorioAmigo = repositorioAmigo ?? new RepositorioAmigo();
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

                #region criar BD para fins de teste
                string nome = "Alexandre";
                string email = "alexandre@email.com";
                string telefone = "6399105-5089";
                //string nome = EntradaHelper.ObterEntrada("Nome", dadosOriginais.nome, editar);
                //string email = EntradaHelper.ObterEntrada("Nome Responsável", dadosOriginais.nomeResponsavel, editar);
                //string telefone = EntradaHelper.ObterEntrada("Telefone", dadosOriginais.telefone, editar);
                #endregion

                string[] nomesCampos = { "nome", "nome responsável", "telefone" };
                string[] valoresCampos = { nome, email, telefone };
                string erros = ValidarCampo.ValidarCampos(nomesCampos, valoresCampos);
                var registros = repositorioAmigo.SelecionarRegistros();

                string erroDuplicado = ValidarCampo.ValidarDuplicidadeAmigo(nome, telefone, registros, dadosOriginais.id);

                if (!string.IsNullOrEmpty(erroDuplicado))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nErros encontrados:");
                    Console.WriteLine(erroDuplicado);
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar();
                    Console.Clear();
                    continue;
                }

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
            Visualizar(true, false, false);
            Console.WriteLine();
            Console.Write("Digite o ID do Amigo: ");
            string strIdAmigo = Console.ReadLine()!;
            int idAmigo = Convert.ToInt32(strIdAmigo);

            Amigo amigo = repositorioAmigo.SelecionarRegistroPorId(idAmigo);

            if (amigo == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Amigo não encontrado!");
                Console.ResetColor();
                DigitarEnterEContinuar.Executar();
                return false;
            }

            List<Emprestimo> emprestimosDoAmigo = amigo.ObterEmprestimos(repositorioEmprestimo.SelecionarRegistros());

            if (emprestimosDoAmigo.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Este amigo não possui empréstimos.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Empréstimos do amigo {amigo.nome}:");
                Console.ResetColor();

                foreach (var emprestimo in emprestimosDoAmigo)
                {
                    Console.WriteLine($"ID: {emprestimo.id}, Revista: {emprestimo.revista.titulo}, " +
                                      $"Data Empréstimo: {emprestimo.dataEmprestimo.ToShortDateString()}, " +
                                      $"Data Devolução: {emprestimo.dataDevolucao.ToShortDateString()}, " +
                                      $"Status: {emprestimo.status}");
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