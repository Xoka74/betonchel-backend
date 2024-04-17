using Betonchel.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Betonchel.Domain.Models;

namespace Betonchel.Data;

public class BetonchelContext : DbContext
{
    public DbSet<Application> Applications { get; set; }
    public DbSet<ConcretePump> ConcretePumps { get; set; }
    public DbSet<FrostResistanceType> FrostResistanceTypes { get; set; }
    public DbSet<WaterproofType> WaterproofTypes { get; set; }
    public DbSet<ConcreteGrade> ConcreteGrades { get; set; }
    public DbSet<User> Users { get; set; }

    public BetonchelContext(DbContextOptions<BetonchelContext> options) : base(options)
    {
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.LogTo(Console.WriteLine);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
        modelBuilder.ApplyConfiguration(new ConcreteGradeConfiguration());
        modelBuilder.ApplyConfiguration(new ConcretePumpConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new FrostResistanceTypeConfiguration());
        modelBuilder.ApplyConfiguration(new WaterproofTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}