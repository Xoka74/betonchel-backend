namespace Betonchel.Auth.Domen.Models;

public class InvalidRefreshTokenError<TModel> : FailureOperationStatusAuth
{
    public InvalidRefreshTokenError() : base($"{typeof(TModel).Name}InvalidRefreshToken")
    {
    }
}