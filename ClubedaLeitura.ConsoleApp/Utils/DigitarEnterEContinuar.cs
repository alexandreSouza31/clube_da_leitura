namespace ClubedaLeitura.Utils
{
    public static class DigitarEnterEContinuar
    {
        public static void Executar(bool lerInput = true)
        {
            Console.WriteLine();
            Console.Write("Digite [Enter] para continuar");
            if (lerInput) Console.ReadLine();
        }
    }
}