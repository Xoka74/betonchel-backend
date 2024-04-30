using System.Text.Json.Serialization;
using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.DBModels;

public class User : Entity<int>
{
    public string FullName;
    public string Email;
    public UserGrade Grade;
    public string PasswordHash;

    [JsonIgnore]
    public ICollection<Application> Application;
}