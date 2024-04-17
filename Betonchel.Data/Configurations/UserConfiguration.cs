using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Betonchel.Data.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.FullName)
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder.Property(e => e.Email)
            .HasColumnType("varchar(50)")
            .IsRequired();
        builder.HasCheckConstraint("CK_Email", "\"Email\" LIKE '%@%'");
        builder.HasIndex(user => user.Email).IsUnique();

        builder.Property(e => e.Grade)
            .IsRequired();

        builder.Property(e => e.PasswordHash)
            .HasColumnType("varchar(70)")
            .IsRequired();
    }
}