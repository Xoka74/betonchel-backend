namespace Betonchel.Auth.Domen.Status.Success;

public class LoginSuccess : SuccessAuthStatus
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}