namespace Betonchel.Auth.Domen.Models;

public class InvalidTokenError<TModel> : FailureOperationStatusAuth
{
    public InvalidTokenError() : base($"{typeof(TModel).Name}InvalidToken")
    {
    }
}