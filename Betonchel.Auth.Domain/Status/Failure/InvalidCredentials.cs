namespace Betonchel.Auth.Domen.Status.Failure;

public class InvalidCredentials : FailureAuthStatus
{
    public InvalidCredentials() : base("InvalidCredentials", "Указан неверный логин или пароль")
    {
    }
}