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
            Console.WriteLine("S - Sair");
            Console.Write("\nDigite uma opção: ");
            char telaEscolhida = Convert.ToChar(Console.ReadLine()!.ToUpper());

            return telaEscolhida;
        }
    }
}