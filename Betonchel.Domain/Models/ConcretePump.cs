using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Domain.Models;

public class ConcretePump
{
    public int Id { get; set; }
    public float MaximumCapacity { get; set; }
    public float? PipeLength { get; set; }
    public double PricePerHour { get; set; }

    public ICollection<Application> Applications { get; set; }
}