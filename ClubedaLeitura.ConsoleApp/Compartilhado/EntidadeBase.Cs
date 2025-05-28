namespace ClubedaLeitura.Compartilhado;

public abstract class EntidadeBase
{
    public abstract void AtualizarRegistro(EntidadeBase registroAtualizado);
    public abstract string Validar();

}