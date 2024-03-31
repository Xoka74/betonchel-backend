using Microsoft.EntityFrameworkCore;
using Betonchel.Domain.Models;

namespace Betonchel.Domain;

public class DomainContext : DbContext
{
    public DbSet<Application> Applications { get; set; }
    public DbSet<ConcretePump> ConcretePumps { get; set; }
    public DbSet<FrostResistanceType> FrostResistanceTypes { get; set; }
    public DbSet<WaterproofType> WaterproofTypes { get; set; }
    public DbSet<ConcreteGrade> ConcreteGrades { get; set; }

    public DomainContext(DbContextOptions<DomainContext> options) : base(options)
    {
    }
}