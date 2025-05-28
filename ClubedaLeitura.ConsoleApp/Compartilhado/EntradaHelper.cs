namespace ClubedaLeitura.Compartilhado
{
	public static class EntradaHelper
	{
		public static T ObterEntrada<T>(string campo, T valorAtual, bool editar)
		{
			Console.Write(editar ? $"{campo} ({valorAtual}): " : $"{campo}: ");
			string entrada = Console.ReadLine();

			if (string.IsNullOrWhiteSpace(entrada))
				return valorAtual;

			try
			{
				return (T)Convert.ChangeType(entrada, typeof(T));
			}
			catch
			{
				Console.WriteLine("Entrada inválida. Manterá o valor atual.");
				return valorAtual;
			}
		}
	}
}