using System.Text.Json.Serialization;
using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.DBModels;

public class User : Entity<int>
{
    public string FullName { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public ICollection<Application> Application;
}