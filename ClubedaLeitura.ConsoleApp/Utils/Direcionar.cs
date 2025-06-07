namespace ClubedaLeitura.Utils
{
	public class Direcionar
	{
		public ResultadoDirecionamento DirecionarParaMenu(bool haItens, bool menuPrincipal, string contexto)
		{
			if (!haItens)
			{
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;

				if(contexto == "Revista") Console.WriteLine($"\nNenhuma {contexto} cadastrada,ou disponível no momento!");
                else Console.WriteLine($"\nNenhum(a) {contexto}(a) cadastrado(a) ainda!");

				Console.ResetColor();
			}

			string mensagem = menuPrincipal
				? "Voltando ao menu principal"
				: $"Voltando ao menu de {contexto}s";

			ExibirTimerRegressivo(5, mensagem);

			return menuPrincipal
				? ResultadoDirecionamento.VoltarMenuPrincipal
				: ResultadoDirecionamento.VoltarMenuContexto;
		}


		private void ExibirTimerRegressivo(int segundos, string mensagem)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;

			for (int i = segundos; i >= 1; i--)
			{
                Console.Write($"\r{mensagem}... {i}   ");
				Thread.Sleep(1000);
			}

			Console.ResetColor();
			Console.WriteLine();
		}
	}
}