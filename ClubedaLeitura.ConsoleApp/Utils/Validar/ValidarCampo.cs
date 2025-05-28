using ClubedaLeitura.ModuloAmigo;
using System.Net.Mail;
namespace ClubedaLeitura.Utils
{
    public class ValidarCampo
    {
        public static string ValidarCampos(string[] nomesCampos, string[] valoresCampos)
        {
            string erros = "";

            for (int i = 0; i < nomesCampos.Length; i++)
            {
                string nomeCampo = nomesCampos[i];
                string valorCampo = valoresCampos[i];

                if (string.IsNullOrWhiteSpace(valorCampo))
                {
                    erros += $"O campo '{nomeCampo}' é obrigatório!\n";
                    continue;
                }

                if (nomeCampo.ToLower() == "email")
                {
                    if (!MailAddress.TryCreate(valorCampo, out _))
                    {
                        erros += $"O campo '{nomeCampo}' deve conter um e-mail válido (ex: nome@provedor.com).\n";
                    }
                }

                if (nomeCampo.ToLower() == "nome" || nomeCampo.ToLower() == "nome responsável")
                {
                    int numeroMinimoCaracteres = 3;
                    int numeroMaximoCaracteres = 100;

                    if (valorCampo.ToLower().Length < numeroMinimoCaracteres)
                    {
                        erros += $"'{nomeCampo}' deve ter mais de {numeroMinimoCaracteres} caracteres!\n";
                    }
                    else if(valorCampo.ToLower().Length > numeroMaximoCaracteres)
                    {
                        erros += $"'{nomeCampo}' deve ter menos de {numeroMaximoCaracteres} caracteres!\n";
                    }
                }

                if(nomeCampo.ToLower()=="telefone")
                {
                    erros += $"'{nomeCampo}' deve ter o formato '(63)3215 - 0000 ou(63) 99100 - 0000)'!\n";                    
                }
                //Não pode haver amigos com o mesmo nome e telefone.
            }
            return erros;
        }
    }
}