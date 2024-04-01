﻿using Betonchel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

internal class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.CustomerName)
            .HasMaxLength(50)
            .HasColumnType("varchar")
            .IsRequired();

        builder.Property(a => a.EmployeeId)
            .IsRequired();

        builder.Property(a => a.ConcreteGradeId)
            .IsRequired();

        builder.Property(a => a.TotalPrice)
            .HasColumnType("numeric")
            .IsRequired();
        builder.HasCheckConstraint("TotalPrice", "TotalPrice >= 0");

        builder.Property(a => a.ConcretePumpId)
            .IsRequired();

        builder.Property(a => a.ContactData)
            .HasColumnType("json")
            .IsRequired();

        builder.Property(a => a.Volume)
            .IsRequired();
        builder.HasCheckConstraint("Volume", "Volume >= 0");

        builder.Property(a => a.DeliveryAddress)
            .HasColumnType("json");

        builder.Property(a => a.DeliveryDate)
            .HasColumnType("timestamptz")
            .IsRequired();
        builder.HasCheckConstraint("DeliveryDate", "DeliveryDate > now()");

        builder.Property(a => a.ApplicationCreationDate)
            .HasColumnType("timestamptz")
            .HasDefaultValueSql("now()")
            .IsRequired();

        builder.Property(a => a.Description)
            .HasColumnType("varchar")
            .HasMaxLength(512);

        builder.HasOne(a => a.User)
            .WithMany(e => e.Application)
            .HasForeignKey(a => a.EmployeeId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}