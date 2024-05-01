using System.Text.Json.Serialization;

namespace Betonchel.Domain.DBModels;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationStatus
{
    Created,
    InProcess,
    SuccessfullyFinished,
    Rejected
}