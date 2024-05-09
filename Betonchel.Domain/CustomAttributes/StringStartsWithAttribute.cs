using System.ComponentModel.DataAnnotations;

namespace Betonchel.Domain.CustomAttributes;

public class StringStartsWithAttribute : ValidationAttribute
{
    public string Prefix { get; }

    public StringStartsWithAttribute(string prefix)
    {
        Prefix = prefix;
    }

    public override bool IsValid(object? value) => 
        value is null || (value is string strValue && strValue.StartsWith(Prefix));
}