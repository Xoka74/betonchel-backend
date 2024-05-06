namespace Betonchel.Auth.Domen.Status.Failure;

public class InvalidAccessToken : FailureAuthStatus
{
    public InvalidAccessToken() : base("InvalidAccessToken", "Неверный access token")
    {
    }
}