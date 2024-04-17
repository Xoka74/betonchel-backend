using Betonchel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

internal class FrostResistanceTypeConfiguration : IEntityTypeConfiguration<FrostResistanceType>
{
    public void Configure(EntityTypeBuilder<FrostResistanceType> builder)
    {
        builder.HasKey(frt => frt.Id);

        builder.Property(wt => wt.Name)
            .HasColumnType("varchar(10)")
            .IsRequired();
    }
}