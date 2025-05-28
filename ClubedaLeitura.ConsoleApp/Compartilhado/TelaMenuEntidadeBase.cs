using ClubedaLeitura.Utils;

namespace ClubedaLeitura.Compartilhado
{
    class TelaMenuEntidadeBase<T> where T : IEntidade
    {
        protected string nomeEntidade;
        protected TelaBase<T> telaBase;

        public TelaMenuEntidadeBase(TelaBase<T> telaBase)
        {
            this.telaBase = telaBase;
            this.nomeEntidade = telaBase.nomeEntidade;
        }

        public bool ExecutarMenuEntidade()
        {
            char opcaoEscolhida = telaBase.ApresentarMenu();

            if (opcaoEscolhida == 'S' || opcaoEscolhida == 's')
                return false;

            switch (opcaoEscolhida)
            {
                case '1':
                    telaBase.Cadastrar();
                    break;
                case '2':
                    telaBase.Visualizar(true, true, false);
                    break;
                case '3':
                    telaBase.Editar();
                    break;
                case '4':
                    telaBase.Excluir();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Digite uma opção válida!");
                    Console.ResetColor();
                    DigitarEnterEContinuar.Executar(true);
                    break;
            }
            return true;
        }
    }
}