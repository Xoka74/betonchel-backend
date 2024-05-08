using System.Text.Json.Serialization;

namespace Betonchel.Domain.DBModels;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConcreteGradeAttribute
{
    Common,
    Fine
}