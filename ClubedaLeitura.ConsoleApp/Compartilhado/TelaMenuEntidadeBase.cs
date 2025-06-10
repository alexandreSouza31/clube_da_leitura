using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloEmprestimo;
using ClubedaLeitura.ModuloMulta;
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

        public bool ExecutarMenuEntidade(bool mostrarEmprestimoAmigo=false)
        {
            char opcaoEscolhida = telaBase.ApresentarMenu();

            if (opcaoEscolhida == 'S' || opcaoEscolhida == 's') return false;

            if (nomeEntidade == "Empréstimo")
            {
                switch (opcaoEscolhida)
                {
                    case '1':
                        telaBase.Cadastrar();
                        break;
                    case '2':
                        if (telaBase is TelaEmprestimo telaEmprestimo) telaEmprestimo.CadastrarDevolucao();
                        break;
                    case '3':
                        telaBase.Visualizar(true, true, false);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Digite uma opção válida!");
                        Console.ResetColor();
                        DigitarEnterEContinuar.Executar(true);
                        break;
                }
            }
            else if (nomeEntidade == "Multa")
            {
                switch (opcaoEscolhida)
                {
                    case '1':
                        if (telaBase is TelaMulta telaMulta) telaMulta.QuitarMulta();
                        break;
                    case '2':
                        telaBase.Visualizar(true, true, false);
                        break;
                    case '3':
                        if (telaBase is TelaMulta telaMultaAmigo) telaMultaAmigo.VisualizarMultaAmigo();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Digite uma opção válida!");
                        Console.ResetColor();
                        DigitarEnterEContinuar.Executar(true);
                        break;
                }
            }
            else
            {
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
                    case '5':
                        if (telaBase is TelaAmigo telaAmigo) telaAmigo.VisualizarEmprestimoAmigo();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Digite uma opção válida!");
                        Console.ResetColor();
                        DigitarEnterEContinuar.Executar(true);
                        break;
                }
            }
            return true;
        }
    }
}