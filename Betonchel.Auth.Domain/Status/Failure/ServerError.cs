namespace Betonchel.Auth.Domen.Models;

public class ServerError<TModel> : FailureOperationStatusAuth
{
    public ServerError() : base($"{typeof(TModel).Name}ServerError")
    {
    }
}