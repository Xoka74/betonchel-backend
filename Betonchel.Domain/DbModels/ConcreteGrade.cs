using System.Text.Json.Serialization;
using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.DBModels;

public class ConcreteGrade : Entity<int>
{
    public ConcreteGradeAttribute Name { get; set; }
    public string Mark { get; set; }
    public string Class { get; set; }
    public string? WaterproofType { get; set; }
    public string? FrostResistanceType { get; set; }
    public double PricePerCubicMeter { get; set; }

    [JsonIgnore]
    public ICollection<Application> Applications { get; set; }
}