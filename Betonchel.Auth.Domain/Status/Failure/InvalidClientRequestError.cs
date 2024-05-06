namespace Betonchel.Auth.Domen.Models;

public class InvalidClientRequestError<TModel> : FailureOperationStatusAuth
{
    public InvalidClientRequestError() : base($"{typeof(TModel).Name}InvalidClientRequest")
    {
    }
}