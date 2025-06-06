using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloRevista;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ModuloCaixa
{
    class TelaCaixa : TelaBase<Caixa>
    {
        private RepositorioCaixa repositorioCaixa;
        private RepositorioRevista repositorioRevista;

        Direcionar direcionar=new Direcionar();

        public TelaCaixa(RepositorioCaixa repositorioCaixa, RepositorioRevista repositorioRevista)
            : base("Caixa", repositorioCaixa)
        {
            this.repositorioCaixa = repositorioCaixa;
            this.repositorioRevista = repositorioRevista;
        }

        public void ExecutarMenu()
        {
            var menuCaixa = new TelaMenuEntidadeBase<Caixa>(this);

            bool continuar = true;
            while (continuar)
            {
                continuar = menuCaixa.ExecutarMenuEntidade();
            }
        }

        public override Caixa CriarInstanciaVazia()
        {
            return new Caixa();
        }

            protected override Caixa ObterNovosDados(Caixa dadosOriginais, bool editar)
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
                //string etiqueta = "dsds45d54";
                //    string cor = "verde";
                //    int diasEmprestimo = 9;
                string etiqueta = EntradaHelper.ObterEntrada("Etiqueta", dadosOriginais.etiqueta, editar);
                string cor = EntradaHelper.ObterEntrada("Cor", dadosOriginais.cor, editar);
                int diasEmprestimo = EntradaHelper.ObterEntrada("Dias de Empréstimo", dadosOriginais.diasEmprestimo, editar);
                #endregion

                if (diasEmprestimo == 0)
                    {
                        diasEmprestimo = 7;
                    }

                    string[] nomesCampos = { "etiqueta", "cor", "dias empréstimo" };
                    string[] valoresCampos = { etiqueta, cor, diasEmprestimo.ToString() };
                    string erros = ValidarCampo.ValidarCampos(nomesCampos, valoresCampos);
                    var registros = repositorioCaixa.SelecionarRegistros();

                    string erroDuplicado = ValidarCampo.ValidarDuplicidadeCaixa(etiqueta, registros, dadosOriginais.id);

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
                    return new Caixa(etiqueta, cor, diasEmprestimo);
                }
            }

        public static void AtualizarCaixa(Caixa original, Caixa novosDados)
        {
            original.etiqueta = novosDados.etiqueta;
            original.cor = novosDados.cor;
            original.diasEmprestimo = novosDados.diasEmprestimo;
        }

        public override bool PossuiRegistroVinculado(int idRegistro)
        {
            var caixa = repositorioCaixa.SelecionarRegistroPorId(idRegistro);

            if (caixa == null)
                return false;

            bool possuiRevistas = repositorioRevista
                .SelecionarRegistros()
                .Where(r => r != null && r.caixa != null)
                .Any(r => r.caixa.id == caixa.id);

            if (possuiRevistas)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Não é possível excluir: a caixa possui revista(s) vinculada(s)!");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, "Caixa");
                return true;
            }
            return false;
        }

        protected override void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -25} | {3, -15}",
                "id".ToUpper(), "etiqueta".ToUpper(), "cor".ToUpper(), "dias empréstimo".ToUpper());
        }

        protected override void ImprimirRegistro(Caixa c)
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -25} | {3, -15}",
                c.id, c.etiqueta, c.cor, c.diasEmprestimo);
        }
    }
}