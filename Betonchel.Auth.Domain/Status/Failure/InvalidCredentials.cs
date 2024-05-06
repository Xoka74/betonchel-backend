namespace Betonchel.Auth.Domen.Models;

public class InvalidCredentials<TModel> : FailureOperationStatusAuth
{
    public InvalidCredentials() : base($"{typeof(TModel).Name}InvalidCredentials")
    {
    }
}