using Betonchel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

internal class ConcreteGradeConfiguration : IEntityTypeConfiguration<ConcreteGrade>
{
    public void Configure(EntityTypeBuilder<ConcreteGrade> builder)
    {
        builder.HasKey(cg => cg.Id);

        builder.Property(cg => cg.Make)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(cg => cg.Class)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(cg => cg.WaterproofTypeId)
            .IsRequired();

        builder.Property(cg => cg.FrostResistanceTypeId)
            .IsRequired();

        builder.Property(cg => cg.PricePerCubicMeter)
            .IsRequired();

        builder.HasCheckConstraint("PricePerCubicMeter", "PricePerCubicMeter >= 0");

        builder
            .HasOne(cg => cg.WaterproofType)
            .WithOne(wt => wt.ConcreteGrade)
            .HasForeignKey<ConcreteGrade>(cg => cg.WaterproofTypeId)
            .HasPrincipalKey<WaterproofType>(wt => wt.Id)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(cg => cg.FrostResistanceType)
            .WithOne(frt => frt.ConcreteGrade)
            .HasForeignKey<ConcreteGrade>(cg => cg.FrostResistanceTypeId)
            .HasPrincipalKey<FrostResistanceType>(frt => frt.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}