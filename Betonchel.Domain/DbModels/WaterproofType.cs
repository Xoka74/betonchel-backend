using System.Text.Json.Serialization;
using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.DBModels;

public class WaterproofType : Entity<int>
{
    public string Name { get; set; }

    [JsonIgnore]
    public ICollection<ConcreteGrade> ConcreteGrades { get; set; }
}