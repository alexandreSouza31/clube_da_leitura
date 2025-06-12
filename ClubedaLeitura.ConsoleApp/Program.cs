using ClubedaLeitura.Apresentacao;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloCaixa;
using ClubedaLeitura.ModuloEmprestimo;
using ClubedaLeitura.ModuloReserva;
using ClubedaLeitura.ModuloRevista;
using ClubedaLeitura.Utils;

namespace ClubedaLeitura.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepositorioAmigo repositorioAmigo = new RepositorioAmigo();
            RepositorioEmprestimo repositorioEmprestimo = new RepositorioEmprestimo();
            TelaAmigo telaAmigo = new TelaAmigo(repositorioAmigo,repositorioEmprestimo);
            RepositorioRevista repositorioRevista = new RepositorioRevista();
            RepositorioReserva repositorioReserva = new RepositorioReserva();

            RepositorioCaixa repositorioCaixa=new RepositorioCaixa();
            TelaCaixa telaCaixa=new TelaCaixa(repositorioCaixa,repositorioRevista);

            TelaRevista telaRevista = new TelaRevista(repositorioRevista, repositorioCaixa, telaCaixa);

            TelaEmprestimo telaEmprestimo = new TelaEmprestimo(repositorioEmprestimo,repositorioAmigo,repositorioRevista, repositorioReserva,telaAmigo,telaRevista);

            TelaReserva telaReserva = new TelaReserva(repositorioReserva, repositorioRevista, repositorioAmigo, repositorioEmprestimo, telaRevista,telaAmigo);

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
                    case '4':
                        telaEmprestimo.ExecutarMenu();
                        break;
                    case '5':
                        telaReserva.ExecutarMenu();
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
