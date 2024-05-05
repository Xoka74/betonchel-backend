namespace Betonchel.Api.Utils;

public class RegisterUrl : IUrl
{
    public RegisterUrl(string value)
    {
        Value = value;
    }

    public string Value { get; }
}