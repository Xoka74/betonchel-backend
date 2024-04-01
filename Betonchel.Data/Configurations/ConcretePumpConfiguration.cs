using Betonchel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

internal class ConcretePumpConfiguration : IEntityTypeConfiguration<ConcretePump>
{
    public void Configure(EntityTypeBuilder<ConcretePump> builder)
    {
        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.MaximumCapacity)
            .IsRequired();
        builder.HasCheckConstraint("MaximumCapacity", "MaximumCapacity >= 0");
        
        builder.HasCheckConstraint("PipeLength", "PipeLength >= 0");

        builder.Property(cp => cp.PricePerHour)
            .IsRequired();
        builder.HasCheckConstraint("PricePerHour", "PricePerHour >= 0");
    }
}