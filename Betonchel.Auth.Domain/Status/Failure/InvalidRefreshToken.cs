namespace Betonchel.Auth.Domen.Status.Failure;

public class InvalidRefreshToken : FailureAuthStatus
{
    public InvalidRefreshToken() : base("InvalidRefreshToken", "Неверный refresh token")
    {
    }
}