using ClubedaLeitura.ModuloEmprestimo;

namespace ClubedaLeitura.Compartilhado
{
    public abstract class RepositorioBase<T> where T : IEntidade
    {
        protected T[] registros = new T[100];
        private int contadorRegistros = 0;

        public void CadastrarRegistro(T novoRegistro)
        {
            if (contadorRegistros < registros.Length)
            {
                registros[contadorRegistros] = novoRegistro;
                contadorRegistros++;
            }
        }

        public bool ExcluirRegistro(int id)
        {
            for (int i = 0; i < registros.Length; i++)
            {
                if (registros[i] != null && registros[i].Id == id)
                {
                    registros[i] = default!;
                    return true;
                }
            }
            return false;
        }

        public bool EditarRegistro(int id, T novosDados)
        {
            for (int i = 0; i < registros.Length; i++)
            {
                if (registros[i] != null && registros[i].Id == id)
                {
                    registros[i] = novosDados;
                    return true;
                }
            }
            return false;
        }

        public T[] SelecionarRegistros()
        {
            return registros.Where(r => r != null).ToArray();
        }

        public T SelecionarRegistroPorId(int id)
        {
            for (int i = 0; i < contadorRegistros; i++)
            {
                if (registros[i] != null && registros[i].Id == id)
                    return registros[i];
            }
            return default!;
        }

        public bool VerificarExistenciaRegistros()
        {
            for (int i = 0; i < contadorRegistros; i++)
            {
                if (registros[i] != null)
                    return true;
            }
            return false;
        }

        public bool TentarObterRegistro(int id, out T registro)
        {
            registro = SelecionarRegistroPorId(id);
            return registro != null && !registro.Equals(default(T));
        }

        public List<T> ObterRegistro<TProp>(T[] registros, TProp entidadeRelacionada, Func<T, TProp?> seletorRelacionamento)
             where TProp : class, IEntidade
        {
            return registros
                .Where(e =>
                    e != null &&
                    seletorRelacionamento(e) != null &&
                    seletorRelacionamento(e)!.Id == entidadeRelacionada.Id)
                .ToList();
        }
    }
}