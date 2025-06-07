using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloCaixa;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ModuloRevista
{
    class TelaRevista : TelaBase<Revista>
    {
        private RepositorioRevista repositorioRevista;
        private RepositorioCaixa repositorioCaixa;
        public TelaCaixa telaCaixa;

        Direcionar direcionar = new Direcionar();

        public TelaRevista(RepositorioRevista repositorioRevista, RepositorioCaixa repositorioCaixa, TelaCaixa telaCaixa)
            : base("Revista", repositorioRevista ?? new RepositorioRevista())
        {
            this.repositorioRevista = repositorioRevista ?? new RepositorioRevista();
            this.repositorioCaixa = repositorioCaixa ?? new RepositorioCaixa();
            this.telaCaixa = telaCaixa;
        }

        public void ExecutarMenu()
        {
            var menuRevista = new TelaMenuEntidadeBase<Revista>(this);

            bool continuar = true;
            while (continuar)
            {
                continuar = menuRevista.ExecutarMenuEntidade();
            }
        }

        public override Revista CriarInstanciaVazia()
        {
            return new Revista();
        }

        protected override Revista ObterNovosDados(Revista dadosOriginais, bool editar)
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

                string titulo = EntradaHelper.ObterEntrada("Titulo", dadosOriginais.titulo, editar);
                int numeroEdicao = EntradaHelper.ObterEntrada("Número edição", dadosOriginais.numeroEdicao, editar);
                int anoPublicacao = EntradaHelper.ObterEntrada("Ano Publicação", dadosOriginais.anoPublicacao, editar);

                bool haCaixas = telaCaixa.Visualizar(true, false, false);

                if (!haCaixas)
                {
                    direcionar.DirecionarParaMenu(false, true, "Caixa");
                    return null;
                }

                Console.WriteLine();
                Console.Write(editar ? $"ID da caixa ({dadosOriginais.caixa?.id}): " : "ID da caixa: ");
                string inputCaixa = Console.ReadLine()!;

                Caixa caixa = string.IsNullOrWhiteSpace(inputCaixa)
                    ? dadosOriginais.caixa
                    : repositorioCaixa.SelecionarRegistroPorId(int.Parse(inputCaixa));

                if (caixa == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Caixa não encontrada! Pressione Enter para continuar...");
                    Console.ResetColor();
                    Console.ReadLine();
                    continue;
                }

                string[] nomesCampos = { "titulo", "número edição", "ano publicação", "caixa" };
                string[] valoresCampos = { titulo, numeroEdicao.ToString(), anoPublicacao.ToString(), caixa.ToString()! };
                string erros = ValidarCampo.ValidarCampos(nomesCampos, valoresCampos);
                var registros = repositorioRevista.SelecionarRegistros();

                string erroDuplicado = ValidarCampo.ValidarDuplicidadeRevista(titulo, numeroEdicao,anoPublicacao, registros, dadosOriginais.id);

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
                return new Revista(titulo, numeroEdicao, anoPublicacao, caixa);
            }
        }

        public static void AtualizarRevista(Revista original, Revista novosDados)
        {
            original.titulo = novosDados.titulo;
            original.numeroEdicao = novosDados.numeroEdicao;
            original.anoPublicacao = novosDados.anoPublicacao;
            original.caixa = novosDados.caixa;
        }

        protected override void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -25} | {3, -15} | {4, -12} | {5, -10}",
                "id".ToUpper(), "titulo".ToUpper(), "número edição".ToUpper(), "ano publicação".ToUpper(), "caixa".ToUpper(), "status".ToUpper());
        }

        protected override void ImprimirRegistro(Revista r)
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -25} | {3, -15} | {4, -12} | {5, -10}",
                r.id, r.titulo, r.numeroEdicao, r.anoPublicacao, r.caixa?.etiqueta, r.status);
        }
    }
}