namespace ClubedaLeitura.ModuloCaixa
{
    public static class SeletorDeCor
    {
        private static readonly Dictionary<int, (string Nome, string Hex)> PaletaCoresIndexada;

        static SeletorDeCor()
        {
            PaletaCoresIndexada = new Dictionary<int, (string, string)>();
            int index = 1;
            foreach (var cor in PaletaCores.Cores)
            {
                PaletaCoresIndexada.Add(index++, (cor.Key, cor.Value));
            }
            PaletaCoresIndexada.Add(0, ("Outra (digitar hexadecimal)", null!));
        }

        public static string ObterCor(string corAtual, bool editar)
        {
            Console.WriteLine("\nSelecione uma cor para a caixa:");

            foreach (var item in PaletaCoresIndexada)
            {
                string texto = item.Value.Hex != null
                    ? $"\t{item.Key} - {item.Value.Nome} ({item.Value.Hex})"
                    : $"\t{item.Key} - {item.Value.Nome}";

                Console.WriteLine(texto);
            }
            Console.WriteLine();

            Console.Write(editar ? $"Escolha (cor atual: {corAtual}): " : "Escolha: ");
            string escolha = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(escolha))
                return corAtual;

            if (int.TryParse(escolha, out int indice) && PaletaCoresIndexada.ContainsKey(indice))
            {
                if (indice == 0)
                {
                    Console.Write("Digite a cor hexadecimal (ex: #FF00FF): ");
                    string hex = Console.ReadLine()!;
                    if (!string.IsNullOrWhiteSpace(hex) && hex.StartsWith("#") && hex.Length == 7)
                        return hex.ToUpper();
                    else if(editar)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Hexadecimal inválido. Usará a cor atual.");
                        Console.ResetColor();
                        return corAtual;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Hexadecimal inválido!");
                        Console.ResetColor();
                        return corAtual;
                    }
                }
                else
                {
                    return PaletaCoresIndexada[indice].Hex!;
                }
            }

            if (editar)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Opção inválida. Usará a cor atual.");
                Console.ResetColor();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Opção inválida!");
            Console.ResetColor();
            return corAtual;
        }
    }
}