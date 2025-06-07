namespace ClubedaLeitura.Compartilhado
{
	public static class EntradaHelper
	{
        public static T ObterEntrada<T>(string campo, T valorAtual, bool editar)
        {
            while (true)
            {
                Console.Write(editar ? $"{campo} ({valorAtual}): " : $"{campo}: ");
                string entrada = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(entrada))
                    return valorAtual;

                try
                {
                    if (typeof(T) == typeof(string))
                        return (T)(object)entrada;

                    object convertido = Convert.ChangeType(entrada, typeof(T));
                    return (T)convertido;
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Formato inv�lido. Tente novamente.");
                    Console.ResetColor();
                }
                catch (OverflowException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("N�mero fora do intervalo permitido. Tente novamente.");
                    Console.ResetColor();
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Entrada inv�lida. Tente novamente.");
                    Console.ResetColor();
                }
            }
        }

    }
}