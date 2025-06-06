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
                if (typeof(T) == typeof(string))
                    return (T)(object)entrada;

                return (T)Convert.ChangeType(entrada, typeof(T));
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Entrada inválida. Manterá o valor atual.");
                Console.ResetColor();
                return valorAtual;
            }
        }
    }
}