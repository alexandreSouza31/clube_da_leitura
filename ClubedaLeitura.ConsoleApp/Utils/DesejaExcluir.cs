namespace ClubedaLeitura.Utils
{
   public class DesejaExcluir
    {
        public string DesejaMesmoExcluir(string item)
        {
            string opcaoExcluir = "";

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Tem certeza que deseja excluir {item}? (S/N): ");
                Console.ResetColor();
                opcaoExcluir = Console.ReadLine()!.Trim().ToUpper();
            } while (opcaoExcluir != "S" && opcaoExcluir != "N");

            return opcaoExcluir;
        }
    }
}