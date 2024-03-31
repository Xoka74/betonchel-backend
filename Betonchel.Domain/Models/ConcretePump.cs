namespace Betonchel.Domain.Models;

public class ConcretePump
{
    public int ConcretePumpId { get; init; }
    public float MaximumCapacity { get; init; }
    public float? PipeLength { get; init; }
    public double PricePerHour { get; set; }
}