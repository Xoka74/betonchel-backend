using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

internal class ConcreteGradeConfiguration : IEntityTypeConfiguration<ConcreteGrade>
{
    public void Configure(EntityTypeBuilder<ConcreteGrade> builder)
    {
        builder.HasKey(cg => cg.Id);

        builder.Property(cg => cg.Name)
            .IsRequired();

        builder.Property(cg => cg.Mark)
            .HasColumnType("varchar(10)")
            .IsRequired();

        builder.Property(cg => cg.Class)
            .HasColumnType("varchar(10)")
            .IsRequired();

        builder.HasIndex(cg => new { cg.Mark, cg.Class });

        builder.Property(cg => cg.WaterproofType)
            .HasMaxLength(10);

        builder.Property(cg => cg.FrostResistanceType)
            .HasMaxLength(10);

        builder.Property(cg => cg.PricePerCubicMeter)
            .IsRequired();

        builder.HasCheckConstraint("CK_PricePerCubicMeter", "\"PricePerCubicMeter\" >= 0");

        builder.HasMany(cg => cg.Applications)
            .WithOne(a => a.ConcreteGrade)
            .HasForeignKey(a => a.ConcreteGradeId)
            .HasPrincipalKey(cg => cg.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}