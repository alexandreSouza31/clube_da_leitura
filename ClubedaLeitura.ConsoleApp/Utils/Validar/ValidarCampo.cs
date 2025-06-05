using ClubedaLeitura.ModuloAmigo;
using ClubedaLeitura.ModuloCaixa;
using ClubedaLeitura.ModuloRevista;
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

                if (nomeCampo.Trim().ToLower() == "nome" || nomeCampo.Trim().ToLower() == "nome responsável")
                {
                    int numeroMinimoCaracteres = 3;
                    int numeroMaximoCaracteres = 100;

                    if (valorCampo.Trim().ToLower().Length < numeroMinimoCaracteres)
                    {
                        erros += $"'{nomeCampo}' deve ter mais de {numeroMinimoCaracteres} caracteres!\n";
                    }
                    else if (valorCampo.Trim().ToLower().Length > numeroMaximoCaracteres)
                    {
                        erros += $"'{nomeCampo}' deve ter menos de {numeroMaximoCaracteres} caracteres!\n";
                    }
                }

                if (nomeCampo.ToLower() == "telefone")
                {
                    valorCampo = RemoverNaoNumericos(valorCampo);

                    if (valorCampo.Length < 10 || valorCampo.Length > 11)
                    {
                        erros += $"'{nomeCampo}' deve ter o formato '(63) 3215-0000' ou '(63) 99100-0000'!\n";
                    }
                }

            }
            return erros;
        }

        public static string ValidarDuplicidadeAmigo(string nome, string telefone, Amigo[] amigosCadastrados, int idAtual)
        {
            string nomeComparado = nome.Trim().ToLower();
            string telefoneComparado = RemoverNaoNumericos(telefone);

            foreach (var amigo in amigosCadastrados)
            {
                if (amigo == null)
                    continue;

                string nomeAmigo = amigo.nome.Trim().ToLower();
                string telefoneAmigo = RemoverNaoNumericos(amigo.telefone);

                if (nomeAmigo == nomeComparado && telefoneAmigo == telefoneComparado && amigo.id != idAtual)
                {
                    return $"Já existe um amigo cadastrado com o nome '{nome}' e telefone '{telefone}'!";
                }
                if (nomeAmigo == nomeComparado && amigo.id != idAtual)
                {
                    return $"Já existe um amigo cadastrado com o nome '{nome}'!";
                }
                if (telefoneAmigo == telefoneComparado && amigo.id != idAtual)
                {
                    return $"Já existe um amigo cadastrado com o telefone '{telefone}'!";
                }
            }
            return "";
        }

        public static string ValidarDuplicidadeCaixa(string etiqueta, Caixa[] caixasCadastradas, int idAtual)
        {
            string etiquetaComparada = etiqueta.Trim().ToLower();

            if(etiquetaComparada.Length > 50)
            {
                return $"Tamanho da etiqueta não deve ultrapassar 50 caracteres!";
            }

            foreach (var caixa in caixasCadastradas)
            {
                if (caixa == null)
                    continue;

                string nomeEtiqueta = caixa.etiqueta.Trim().ToLower();

                if (nomeEtiqueta == etiquetaComparada && caixa.id != idAtual)
                {
                    return $"Já existe uma caixa cadastrada com a etiqueta '{etiqueta}'!";
                }
            }
            return "";
        }

        public static string ValidarDuplicidadeRevista(string titulo,int numeroEdicao, Revista[] revistasCadastradas, int idAtual)
        {
            string tituloComparado = titulo.Trim().ToLower();
            int numeroEdicaoComparado = numeroEdicao;

            foreach (var revista in revistasCadastradas)
            {
                if (revista == null)
                    continue;

                string tituloRevista = revista.titulo.Trim().ToLower();
                int numeroEdicaoRevista = revista.numeroEdicao;

                if (tituloRevista == tituloComparado && numeroEdicaoRevista == numeroEdicaoComparado && revista.id != idAtual)
                {
                    return $"Já existe uma revista cadastrada com o título '{titulo}' e edição '{numeroEdicao}'!";
                }
                if (tituloRevista == tituloComparado && revista.id != idAtual)
                {
                    return $"Já existe uma revista cadastrada com o título '{titulo}'!";
                }
                if (numeroEdicaoRevista == numeroEdicaoComparado && revista.id != idAtual)
                {
                    return $"Já existe uma revista cadastrada com edição '{numeroEdicao}'!";
                }
            }
            return "";
        }
        private static string RemoverNaoNumericos(string texto)
        {
            return new string(texto.Where(char.IsDigit).ToArray());
        }
    }
}
