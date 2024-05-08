using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

internal class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.CustomerName)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(a => a.UserId)
            .IsRequired();

        builder.Property(a => a.ConcreteGradeId)
            .IsRequired();

        builder.Property(a => a.TotalPrice)
            .HasColumnType("numeric")
            .IsRequired();
        builder.HasCheckConstraint("CK_TotalPrice", "\"TotalPrice\" >= 0");

        builder.Property(a => a.ContactData)
            .HasMaxLength(124)
            .IsRequired();

        builder.Property(a => a.Volume)
            .IsRequired();
        builder.HasCheckConstraint("CK_Volume", "\"Volume\" >= 0");

        builder.Property(a => a.DeliveryAddress)
            .HasMaxLength(124);

        builder.Property(a => a.DeliveryDate)
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(a => a.Status)
            .HasDefaultValue(ApplicationStatus.Created)
            .IsRequired();

        builder.Property(a => a.ApplicationCreationDate)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(a => a.Description)
            .HasMaxLength(512);

        builder.HasOne(a => a.User)
            .WithMany(e => e.Application)
            .HasForeignKey(a => a.UserId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}