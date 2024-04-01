namespace Betonchel.Domain.Models;

public class Application
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public int EmployeeId { get; set; }
    public int ConcreteGradeId { get; set; }
    public double TotalPrice { get; set; }
    public int ConcretePumpId { get; set; }
    public string ContactData { get; set; }
    public float Volume { get; set; }
    public string? DeliveryAddress { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime ApplicationCreationDate { get; set; }
    public string? Description { get; set; }

    public ConcreteGrade ConcreteGrade { get; set; }
    public ConcretePump? ConcretePump { get; set; }
    public User User { get; set; }
}