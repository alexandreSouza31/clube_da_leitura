using ClubedaLeitura.Configuracoes;
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
            Console.WriteLine($"1 - Cadastrar {nomeEntidade}");
            Console.WriteLine($"2 - Visualizar {nomeEntidade}");
            Console.WriteLine($"3 - Editar {nomeEntidade}");
            Console.WriteLine($"4 - Excluir {nomeEntidade}");

            if (nomeEntidade == "Amigo") Console.WriteLine($"5 - Visualizar Empréstimo {nomeEntidade}");
            
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opção: ");
            char opcaoEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper()[0]);

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
                Console.WriteLine($"\n{nomeEntidade} cadastrado com sucesso! ID: {novosDados.id}");
                Console.ResetColor();

                direcionar.DirecionarParaMenu(true, false, nomeEntidade);
                return true;
            }
        }

        public bool Visualizar(bool exibirCabecalho, bool digitarEnterEContinuar, bool msgAoCadastrar = true)
        {
            ExibirCabecalho("visualizar");

            Console.Clear();
            if (exibirCabecalho)
                ExibirCabecalho();
            Console.WriteLine($"----- {nomeEntidade}s Registrados -----");

            bool haRegistros = repositorio.VerificarExistenciaRegistros();

            if (!haRegistros)
            {
                if (msgAoCadastrar)
                {
                    Console.WriteLine($"Ainda não há {nomeEntidade.ToLower()}s!");
                    direcionar.DirecionarParaMenu(false, false, nomeEntidade);
                    return false;
                }
            }

            T[] registros = repositorio.SelecionarRegistros();
            int encontrados = 0;

            foreach (T reg in registros)
            {
                if (reg == null) continue;

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
                    Console.WriteLine("ID inválido. Tente novamente.");
                    Console.ResetColor();
                    continue;
                }

                if (!repositorio.TentarObterRegistro(idRegistro, out var registroExistente))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{nomeEntidade} não encontrado. Tente novamente!");
                    Console.ResetColor();
                    continue;
                }

                var novosDados = ObterNovosDados(registroExistente, true);
                novosDados.id = registroExistente.id;

                repositorio.EditarRegistro(idRegistro, novosDados);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{nomeEntidade} editado com sucesso! id: {novosDados.id}");
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
                        Console.WriteLine("ID inválido. Tente novamente.");
                        Console.ResetColor();
                        continue;
                    }

                    if (!repositorio.TentarObterRegistro(idRegistro, out var registro))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{nomeEntidade} não encontrado. Tente novamente.");
                        Console.ResetColor();
                        continue;
                    }

                    DesejaExcluir desejaExcluir = new DesejaExcluir();
                    var vaiExcluir = desejaExcluir.DesejaMesmoExcluir($"esse {nomeEntidade}");

                    if (vaiExcluir != "S") return false;

                    repositorio.ExcluirRegistro(idRegistro);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{nomeEntidade} excluído com sucesso! id: {registro.id}");
                    Console.ResetColor();
                    direcionar.DirecionarParaMenu(false, false, nomeEntidade);
                    return true;
                }
            }
        }

        public abstract T CriarInstanciaVazia();

        protected abstract T ObterNovosDados(T dadosIniciais, bool editar);

        protected abstract void ImprimirCabecalhoTabela();

        protected abstract void ImprimirRegistro(T entidade);
    }
}