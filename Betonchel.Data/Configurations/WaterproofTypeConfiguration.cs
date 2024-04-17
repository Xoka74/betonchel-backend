using Betonchel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

public class WaterproofTypeConfiguration : IEntityTypeConfiguration<WaterproofType>
{
    public void Configure(EntityTypeBuilder<WaterproofType> builder)
    {
        builder.HasKey(wt => wt.Id);

        builder.Property(wt => wt.Name)
            .HasColumnType("varchar(10)")
            .IsRequired();
    }
}