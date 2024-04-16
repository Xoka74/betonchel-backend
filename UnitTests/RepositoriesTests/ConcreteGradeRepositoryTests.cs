using Betonchel.Data;
using Betonchel.Data.Repositories;
using Betonchel.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UnitTests.RepositoriesTests;

[TestFixture]
public class ConcreteGradeRepositoryTests
{
    private readonly BetonchelContext dataContext;

    public ConcreteGradeRepositoryTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(
                @"C:\Users\egore\OneDrive\Рабочий стол\Учёба\betonchel-backend\UnitTests\RepositoriesTests\appsettings.json",
                optional: false, reloadOnChange: true)
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<BetonchelContext>();
        optionsBuilder.UseNpgsql(connectionString);

        dataContext = new BetonchelContext(optionsBuilder.Options);
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var repo = new ConcreteGradeRepository(dataContext);
        // foreach (var concreteGrade in repo.GetAll())
        // {
        //     Console.WriteLine(concreteGrade);
        // }
        // var t1 = repo.GetBy(12);
        // var cg = repo.GetBy(1);
        // cg.Mark = "213";
        var model = new ConcreteGrade()
        {
            Mark = "М-100", Class = "В7,5", WaterproofTypeId = 1, FrostResistanceTypeId = 1, PricePerCubicMeter = 12
        };
        repo.Create(model)
            .SaveChanges();
    }
}