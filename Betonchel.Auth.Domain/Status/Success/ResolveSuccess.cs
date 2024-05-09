namespace Betonchel.Auth.Domen.Status.Success;

public class ResolveSuccess : Success
{
    public string? Email { get; }

    public ResolveSuccess(string? email)
    {
        Email = email;
    }
}