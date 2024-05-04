using System.Text.Json.Serialization;
using Betonchel.Domain.BaseModels;
using Microsoft.AspNetCore.Identity;

namespace Betonchel.Domain.DBModels;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    [JsonIgnore]
    public ICollection<Application> Application;
}