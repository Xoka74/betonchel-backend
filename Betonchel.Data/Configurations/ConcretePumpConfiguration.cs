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
        builder.HasCheckConstraint("CK_MaximumCapacity", "\"MaximumCapacity\" >= 0");

        builder.HasCheckConstraint("CK_PipeLength", "\"PipeLength\" >= 0");

        builder.Property(cp => cp.PricePerHour)
            .IsRequired();
        builder.HasCheckConstraint("CK_PricePerHour", "\"PricePerHour\" >= 0");


        builder.HasMany(cp => cp.Applications)
            .WithOne(a => a.ConcretePump)
            .HasForeignKey(a => a.ConcretePumpId)
            .HasPrincipalKey(cp => cp.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}