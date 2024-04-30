using Betonchel.Data.Configurations;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data;

public class BetonchelContext : DbContext
{
    public DbSet<Application> Applications { get; set; }
    public DbSet<ConcretePump> ConcretePumps { get; set; }
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

        base.OnModelCreating(modelBuilder);
    }
}