namespace Betonchel.Auth.Domen.Models;

public class SuccessLoginModelStatus
{
    public bool Success { get; } = true;
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}