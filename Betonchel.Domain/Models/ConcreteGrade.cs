namespace Betonchel.Domain.Models;

public class ConcreteGrade
{
    public int ConcreteGradeId { get; init; }
    public string Make { get; init; }
    public string Class { get; set; }
    public int WaterproofId { get; init; }
    public int FrostResistanceId { get; set; }
    public double PricePerCubicMeter { get; init; }
}