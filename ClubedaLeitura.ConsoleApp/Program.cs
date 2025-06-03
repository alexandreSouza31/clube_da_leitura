using ClubedaLeitura.Apresentacao;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloCaixa;
using ClubedaLeitura.ModuloRevista;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepositorioAmigo repositorioAmigo = new RepositorioAmigo();
            TelaAmigo telaAmigo = new TelaAmigo(repositorioAmigo);

            RepositorioCaixa repositorioCaixa=new RepositorioCaixa();
            TelaCaixa telaCaixa=new TelaCaixa(repositorioCaixa);

            RepositorioRevista repositorioRevista=new RepositorioRevista();
            TelaRevista telaRevista = new TelaRevista(repositorioRevista, repositorioCaixa, telaCaixa);

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
                    case '2':
                        telaCaixa.ExecutarMenu();
                        break;
                    case '3':
                        telaRevista.ExecutarMenu();
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
