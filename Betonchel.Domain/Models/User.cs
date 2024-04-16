using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.Models;

public class User : Entity<int>
{
    public string Name;
    public string ContactData;
    public UserGrade Grade;
    
    public ICollection<Application> Application;
}