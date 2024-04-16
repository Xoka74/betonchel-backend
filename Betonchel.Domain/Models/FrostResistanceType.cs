using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.Models;

public class FrostResistanceType : Entity<int>
{
    public string Name { get; set; }

    public ICollection<ConcreteGrade> ConcreteGrades { get; set; }
}