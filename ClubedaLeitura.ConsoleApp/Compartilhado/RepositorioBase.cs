namespace ClubedaLeitura.Compartilhado
{
    public abstract class RepositorioBase<T> where T : IEntidade
    {
        private T[] registros = new T[100];
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
                if (registros[i] != null && registros[i].id == id)
                {
                    registros[i] = default;
                    return true;
                }
            }
            return false;
        }

        public bool EditarRegistro(int id, T novosDados)
        {
            for (int i = 0; i < registros.Length; i++)
            {
                if (registros[i] != null && registros[i].id == id)
                {
                    registros[i] = novosDados;
                    return true;
                }
            }
            return false;
        }

        public T[] SelecionarRegistros()
        {
            return registros;
        }

        public T SelecionarRegistroPorId(int id)
        {
            for (int i = 0; i < contadorRegistros; i++)
            {
                if (registros[i] != null && registros[i].id == id)
                    return registros[i];
            }
            return default;
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
    }
}