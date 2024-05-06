namespace Betonchel.Auth.Domen.Models;

public class NotUnique<TModel> : FailureOperationStatusAuth
{
    public NotUnique() : base($"{typeof(TModel).Name}NotUnique")
    {
    }
}