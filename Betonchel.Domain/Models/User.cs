namespace Betonchel.Domain.Models;

public class User
{
    public int Id;
    public string Name;
    public string ContactData;
    public UserGrade UserGrade;
    
    public ICollection<Application> Application;
}