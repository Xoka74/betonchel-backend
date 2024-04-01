namespace Betonchel.Domain.Models;

public class Employee
{
    public int Id;
    public string Name;
    public string ContactData;
    public EmployeeGrade EmployeeGrade;
    
    public ICollection<Application> Application;
}