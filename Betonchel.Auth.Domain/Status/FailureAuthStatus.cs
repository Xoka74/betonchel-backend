namespace Betonchel.Auth.Domen.Status;

public class FailureAuthStatus : IAuthStatus
{
    public bool Success => false;
    public string Error { get; }
    public string Massage { get; }

    protected FailureAuthStatus(string error, string message)
    {
        Error = error;
        Massage = message;
    }
}