using ClubedaLeitura.Apresentacao;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepositorioAmigo repositorioAmigo = new RepositorioAmigo();
            TelaAmigo telaAmigo = new TelaAmigo(repositorioAmigo);
            Direcionar direcionar = new Direcionar();

            TelaMenuPrincipal telaPrincipal = new TelaMenuPrincipal();

            while (true)
            {
                char telaEscolhida = telaPrincipal.ExibirMenuPrincipal();

                if (telaEscolhida == 'S') break;

                switch (telaEscolhida)
                {
                    case '1':
                        telaAmigo.ExecutarMenu();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opção inválida!");
                        Console.ResetColor();
                        break;
                }
            }
        }
    }
}
