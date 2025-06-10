using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.Configuracoes;

namespace ClubedaLeitura.Apresentacao
{
    public class TelaMenuPrincipal
    {
        public char ExibirMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine($"----- {Configuracao.NomeProjeto} -----");
            Console.WriteLine();

            Console.WriteLine("1 - Amigos");
            Console.WriteLine("2 - Caixas");
            Console.WriteLine("3 - Revistas");
            Console.WriteLine("4 - Empréstimos");
            Console.WriteLine("5 - Multas");
            Console.WriteLine("S - Sair");
            char telaEscolhida = char.ToUpper(EntradaHelper.ObterEntrada<char>("Digite uma opção", '\0', false));

            return telaEscolhida;
        }
    }
}