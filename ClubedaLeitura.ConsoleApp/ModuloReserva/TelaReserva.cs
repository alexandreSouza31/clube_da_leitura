using ClubedaLeitura.Compartilhado;
using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloEmprestimo;
using ClubedaLeitura.ModuloRevista;
using ClubedaLeitura.Utils;
using static ClubedaLeitura.ModuloRevista.Revista;

namespace ClubedaLeitura.ModuloReserva
{
    internal class TelaReserva : TelaBase<Reserva>
    {
        private readonly RepositorioRevista repositorioRevista;
        private readonly RepositorioAmigo repositorioAmigo;
        private readonly RepositorioEmprestimo repositorioEmprestimo;

        private readonly TelaRevista telaRevista;
        private readonly TelaAmigo telaAmigo;

        Direcionar direcionar = new Direcionar();

        public TelaReserva(
            RepositorioReserva repositorioReserva,
            RepositorioRevista repositorioRevista,
            RepositorioAmigo repositorioAmigo,
            RepositorioEmprestimo repositorioEmprestimo,
            TelaRevista telaRevista,
            TelaAmigo telaAmigo)
            : base("Reserva", repositorioReserva)
        {
            this.repositorioRevista = repositorioRevista;
            this.repositorioAmigo = repositorioAmigo;
            this.repositorioEmprestimo = repositorioEmprestimo;
            this.telaRevista = telaRevista;
            this.telaAmigo = telaAmigo;
        }

        public void ExecutarMenu()
        {
            var menu = new TelaMenuEntidadeBase<Reserva>(this);

            bool continuar = true;
            while (continuar)
                continuar = menu.ExecutarMenuEntidade();
        }

        public override Reserva CriarInstanciaVazia()
        {        
            return null!;
        }

        protected override Reserva ObterNovosDados(Reserva dadosIniciais, bool editar)
        {
            Console.WriteLine("\n--- Cadastrar Reserva ---");
            bool haAmigos = telaAmigo.Visualizar(true, false, false);
            if (!haAmigos)
            {
                direcionar.DirecionarParaMenu(false, true, "Amigo");
                return null!;
            }

            Console.WriteLine();
            Console.Write("Digite o ID do amigo: ");
            Console.WriteLine();

            int idAmigo = int.Parse(Console.ReadLine()!);
            Amigo amigo = repositorioAmigo.SelecionarRegistroPorId(idAmigo);

            if (amigo == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Amigo não encontrado.");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, "Reserva");
                return null!;
            }

            var emprestimosAmigo = repositorioEmprestimo.SelecionarRegistros();
            foreach (var e in emprestimosAmigo)
            {
                if (e.Amigo.Id == amigo.Id && e.Multa != null && !e.Multa.EstaPaga)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Amigo possui multa pendente. Reserva não permitida!");
                    Console.ResetColor();
                    direcionar.DirecionarParaMenu(true, false, "Reserva");
                    return null!;
                }
            }


            bool haRevistas = telaRevista.Visualizar(true, false, false, r => r.Status == StatusRevista.Disponivel);
            if (!haRevistas)
            {
                direcionar.DirecionarParaMenu(false, true, "Revista");
                return null!;
            }
            Console.WriteLine();
            Console.Write("Digite o ID da revista: ");
            Console.WriteLine();

            int idRevista = int.Parse(Console.ReadLine()!);
            Revista revistaSelecionada = repositorioRevista.SelecionarRegistroPorId(idRevista);

            if (revistaSelecionada == null || revistaSelecionada.Status != StatusRevista.Disponivel)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Revista indisponível para reserva.");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, "Reserva");
                return null!;
            }

            var reservasAtivas = ((RepositorioReserva)repositorio).SelecionarReservasAtivas();
            foreach (var r in reservasAtivas)
            {
                if (r.Revista.Id == revistaSelecionada.Id)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Já existe uma reserva ativa para essa revista.");
                    Console.ResetColor();
                    direcionar.DirecionarParaMenu(true, false, "Reserva");
                    return null!;
                }
            }

            Reserva novaReserva = new Reserva(amigo, revistaSelecionada);

            return novaReserva;
        }

        public override bool PossuiRegistroVinculado(int idRegistro)
        {
            return false;
        }

        public void CancelarReserva()
        {
            Visualizar(true,false,false);
            Console.WriteLine();
            Console.Write("Digite o ID da reserva a cancelar: ");
            Console.WriteLine();

            int id = int.Parse(Console.ReadLine()!);

            Reserva reserva = ((RepositorioReserva)repositorio).SelecionarRegistroPorId(id);

            if (reserva == null || reserva.Status == StatusReserva.Concluida)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Reserva não encontrada ou já concluída.");
                Console.ResetColor();
                direcionar.DirecionarParaMenu(true, false, "Reserva");
                return;
            }

            reserva.Concluir();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Reserva cancelada com sucesso!");
            Console.ResetColor();
            direcionar.DirecionarParaMenu(true, false, "Reserva");
        }

        public void VisualizarReservasAtivas()
        {
            var reservas = ((RepositorioReserva)repositorio).SelecionarReservasAtivas();

            Console.WriteLine("\n--- Reservas Ativas ---");
            foreach (var r in reservas)
            {
                Console.WriteLine($"ID: {r.Id}, Amigo: {r.Amigo.Nome}, Revista: {r.Revista.Titulo}, Data: {r.DataReserva:dd/MM/yyyy}");
            }
        }

        protected override void ImprimirCabecalhoTabela()
        {
            Console.WriteLine("{0, -5} | {1, -20} | {2, -20} | {3, -15} | {4, -10}",
                "id".ToUpper(), "amigo".ToUpper(), "revista".ToUpper(), "data reserva".ToUpper(), "status".ToUpper());
        }

        protected override void ImprimirRegistro(Reserva r)
        {
            if (r != null)
            {
                Console.WriteLine("{0, -5} | {1, -20} | {2, -20} | {3, -15} | {4, -10}",
                r.Id, r.Amigo.Nome, r.Revista.Titulo, r.DataReserva.ToShortDateString(), r.Status);
            }
        }
    }
}
