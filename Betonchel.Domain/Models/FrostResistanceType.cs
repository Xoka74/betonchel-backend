namespace Betonchel.Domain.Models;

public class FrostResistanceType
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ConcreteGrade ConcreteGrade { get; set; }
}