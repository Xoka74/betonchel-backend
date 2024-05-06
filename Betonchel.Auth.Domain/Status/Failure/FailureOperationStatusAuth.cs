namespace Betonchel.Auth.Domen.Models;

public class FailureOperationStatusAuth
{
    public bool Success => false;
    public string Error { get; }

    protected FailureOperationStatusAuth(string error)
    {
        Error = error;
    }
}