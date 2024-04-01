namespace Betonchel.Domain.Models;

public class ConcreteGrade
{
    public int Id { get; set; }
    public string Mark { get; set; }
    public string Class { get; set; }
    public int WaterproofTypeId { get; set; }
    public int FrostResistanceTypeId { get; set; }
    public double PricePerCubicMeter { get; set; }

    public ICollection<Application> Applications { get; set; }
    public WaterproofType WaterproofType { get; set; }
    public FrostResistanceType FrostResistanceType { get; set; }
}