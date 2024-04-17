using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.Models;

public class User : Entity<int>
{
    public string FullName;
    public string Email;
    public UserGrade Grade;
    public string PasswordHash;

    public ICollection<Application> Application;
}