namespace Betonchel.Auth.Domen.Models;

public class NotExist<TModel> : FailureOperationStatusAuth
{
    public NotExist() : base($"{typeof(TModel).Name}NotExist")
    {
    }
}