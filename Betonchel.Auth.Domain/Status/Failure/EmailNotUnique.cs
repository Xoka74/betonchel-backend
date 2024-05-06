namespace Betonchel.Auth.Domen.Status.Failure;

public class EmailNotUnique : FailureAuthStatus
{
    public EmailNotUnique() : base("EmailNotUnique", "Пользователь с таким email уже существует")
    {
    }
}