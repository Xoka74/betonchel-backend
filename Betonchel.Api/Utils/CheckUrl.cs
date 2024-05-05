namespace Betonchel.Api.Utils;

public class CheckUrl : IUrl
{
    public CheckUrl(string value)
    {
        Value = value;
    }

    public string Value { get; }
}