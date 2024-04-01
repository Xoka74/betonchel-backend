using Betonchel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

internal class ConcreteGradeConfiguration : IEntityTypeConfiguration<ConcreteGrade>
{
    public void Configure(EntityTypeBuilder<ConcreteGrade> builder)
    {
        builder.HasKey(cg => cg.Id);

        builder.Property(cg => cg.Mark)
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(cg => cg.Class)
            .HasColumnType("varchar")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(cg => cg.WaterproofTypeId)
            .IsRequired();

        builder.Property(cg => cg.FrostResistanceTypeId)
            .IsRequired();

        builder.Property(cg => cg.PricePerCubicMeter)
            .IsRequired();

        builder.HasCheckConstraint("PricePerCubicMeter", "PricePerCubicMeter >= 0");
        
        builder.HasMany(cg => cg.Applications)
            .WithOne(a => a.ConcreteGrade)
            .HasForeignKey(a => a.ConcreteGradeId)
            .HasPrincipalKey(cg => cg.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cg => cg.WaterproofType)
            .WithMany(wt => wt.ConcreteGrades)
            .HasForeignKey(cg => cg.WaterproofTypeId)
            .HasPrincipalKey(wt => wt.Id)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(cg => cg.FrostResistanceType)
            .WithMany(frt => frt.ConcreteGrades)
            .HasForeignKey(cg => cg.FrostResistanceTypeId)
            .HasPrincipalKey(frt => frt.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}