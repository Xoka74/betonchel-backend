using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.DBModels;

public class ConcretePump : Entity<int>
{
    public float MaximumCapacity { get; set; }
    public float? PipeLength { get; set; }
    public double PricePerHour { get; set; }

    public ICollection<Application> Applications { get; set; }
}