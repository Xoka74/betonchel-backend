using System.ComponentModel.DataAnnotations;

public class StringStartsWithAttribute : ValidationAttribute
{
    public string Prefix { get; }

    public StringStartsWithAttribute(string prefix)
    {
        Prefix = prefix;
    }

    public override bool IsValid(object? value) => 
        value is string strValue && strValue.StartsWith(Prefix);
}