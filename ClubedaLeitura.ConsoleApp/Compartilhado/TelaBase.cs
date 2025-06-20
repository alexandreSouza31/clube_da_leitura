using ClubedaLeitura.Configuracoes;
using ClubedaLeitura.ModuloCaixa;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.Compartilhado
{
    public abstract class TelaBase<T> where T : IEntidade
    {
        internal string nomeEntidade;
        protected RepositorioBase<T> repositorio;

        protected TelaBase(string nomeEntidade, RepositorioBase<T> repositorio)
        {
            this.nomeEntidade = nomeEntidade;
            this.repositorio = repositorio;
        }

        Direcionar direcionar = new Direcionar();

        protected void ExibirCabecalho(string contexto = "")
        {
            Console.Clear();
            Console.WriteLine($"----- {Configuracao.NomeProjeto}/Menu {nomeEntidade}/{contexto} {nomeEntidade} -----");
            Console.WriteLine();
        }

        public char ApresentarMenu()
        {
            ExibirCabecalho();
            Console.WriteLine();

            if(nomeEntidade == "Reserva")
            {
                Console.WriteLine($"1 - Cadastrar {nomeEntidade}");
                Console.WriteLine($"2 - Visualizar {nomeEntidade}");
                Console.WriteLine($"3 - Cancelar {nomeEntidade}");
            }
            else if(nomeEntidade == "Empr�stimo")
            {
                Console.WriteLine($"1 - Cadastrar {nomeEntidade}");
                Console.WriteLine($"2 - Cadastrar Devolu��o");
                Console.WriteLine($"3 - Visualizar {nomeEntidade}");
            }
            else
            {
                Console.WriteLine($"1 - Cadastrar {nomeEntidade}");
                Console.WriteLine($"2 - Visualizar {nomeEntidade}");
                Console.WriteLine($"3 - Editar {nomeEntidade}");
                Console.WriteLine($"4 - Excluir {nomeEntidade}");

                if (nomeEntidade == "Amigo") Console.WriteLine($"5 - Visualizar Empr�stimo {nomeEntidade}");
            }
            
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma op��o: ");
            string entrada = EntradaHelper.ObterEntrada("Op��o", " ", false);
            char opcaoEscolhida = entrada.ToUpper()[0];

            return opcaoEscolhida;
        }

        public bool Cadastrar()
        {
            ExibirCabecalho("cadastrar");

            T dadosIniciais = CriarInstanciaVazia();

            var novosDados = ObterNovosDados(dadosIniciais, false);

            if (novosDados == null) return false;

            else
            {
                repositorio.CadastrarRegistro(novosDados);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{nomeEntidade} cadastrado(a) com sucesso! ID: {novosDados.Id}");
                Console.ResetColor();

                direcionar.DirecionarParaMenu(true, false, nomeEntidade);
                return true;
            }
        }

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar, bool msgAoCadastrar = true, Func<T, bool> filtro = null!)
        {
            ExibirCabecalho("visualizar");

            Console.Clear();
            if (exibirCabecalho)
                ExibirCabecalho();
            Console.WriteLine($"----- {nomeEntidade}s Registrados(as) -----");

            bool haRegistros = repositorio.VerificarExistenciaRegistros();

            if (haRegistros == false)
            {
                if (msgAoCadastrar) 
                {
                    Console.WriteLine($"Ainda n�o h� {nomeEntidade.ToLower()}s!");
                    direcionar.DirecionarParaMenu(false, false, nomeEntidade);
                    return false;
                }
            }

            T[] registros = repositorio.SelecionarRegistros();
            int encontrados = 0;

            foreach (T reg in registros)
            {
                if (reg == null || (filtro != null && !filtro(reg))) continue;

                if (encontrados == 0)
                {
                    Console.WriteLine();
                    ImprimirCabecalhoTabela();
                }

                ImprimirRegistro(reg);

                encontrados++;
            }

            if (digitarEnterEContinuar)
                DigitarEnterEContinuar.Executar();

            return encontrados > 0;
        }

        public bool Editar()
        {
            ExibirCabecalho("editar");

            var haCadastro = repositorio.VerificarExistenciaRegistros();
            if (!haCadastro)
            {
                direcionar.DirecionarParaMenu(false, false, nomeEntidade);
                return false;
            }

            Visualizar(true, false, false);

            while (true)
            {
                Console.Write($"\nDigite o Id do {nomeEntidade} para editar: ");
                if (!int.TryParse(Console.ReadLine(), out int idRegistro))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ID inv�lido. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                if (!repositorio.TentarObterRegistro(idRegistro, out var registroExistente))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{nomeEntidade} n�o encontrado. Tente novamente!");
                    Console.ResetColor();
                    continue;
                }

                var novosDados = ObterNovosDados(registroExistente, true);
                novosDados.Id = registroExistente.Id;

                repositorio.EditarRegistro(idRegistro, novosDados);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{nomeEntidade} editado(a) com sucesso! id: {novosDados.Id}");
                Console.ResetColor();

                direcionar.DirecionarParaMenu(true, false, nomeEntidade);
                return true;
            }
        }


            public bool Excluir()
            {
                ExibirCabecalho("excluir");

                var haCadastro = repositorio.VerificarExistenciaRegistros();

                if (!haCadastro)
                {
                    direcionar.DirecionarParaMenu(false, false, nomeEntidade);
                    return false;
                }
                else
                {
                    Visualizar(true, false, false);
                    while (true)
                    {
                        Console.Write($"\nDigite o Id do {nomeEntidade} para excluir: ");
                        if (!int.TryParse(Console.ReadLine(), out int idRegistro))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ID inv�lido. Tente novamente.");
                            Console.ResetColor();
                            continue;
                        }

                        if (!repositorio.TentarObterRegistro(idRegistro, out var registro))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{nomeEntidade} n�o encontrado. Tente novamente.");
                            Console.ResetColor();
                            continue;
                        }

                        if (PossuiRegistroVinculado(idRegistro)) return false;

                        DesejaExcluir desejaExcluir = new DesejaExcluir();
                        var vaiExcluir = desejaExcluir.DesejaMesmoExcluir($"esse {nomeEntidade}");

                        if (vaiExcluir != "S") return false;

                        repositorio.ExcluirRegistro(idRegistro);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{nomeEntidade} exclu�do(a) com sucesso! id: {registro.Id}");
                        Console.ResetColor();
                        bool aindaHaRegistros = repositorio.VerificarExistenciaRegistros();
                        direcionar.DirecionarParaMenu(aindaHaRegistros, false, nomeEntidade);

                    return true;
                    }
                }
            }

        public abstract T CriarInstanciaVazia();

        protected abstract T ObterNovosDados(T dadosIniciais, bool editar);

        protected abstract void ImprimirCabecalhoTabela();

        protected abstract void ImprimirRegistro(T entidade);

        public virtual bool PossuiRegistroVinculado(int idRegistro)
        {
            return false;
        }
    }
}