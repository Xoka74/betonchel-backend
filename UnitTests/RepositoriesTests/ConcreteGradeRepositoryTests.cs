using Betonchel.Data;
using Betonchel.Data.Repositories;
using Betonchel.Domain.DBModels;
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
        //     Console.WriteLine(
        //         $"{concreteGrade} {concreteGrade.WaterproofType.Name} {concreteGrade.FrostResistanceType.Name}"
        //     );
        // }
        // var t1 = repo.GetBy(12);
        // var cg = repo.GetBy(2);
        // Console.WriteLine(
        //     $"{cg} {cg.WaterproofType.Name} {cg.FrostResistanceType.Name}"
        // );
        // cg.Mark = "213";
        var model = new ConcreteGrade()
        {
            Id=1, Mark = "М-100", Class = "В7,5", WaterproofTypeId = 2, FrostResistanceTypeId = 1, PricePerCubicMeter = 12
        };
        repo.Create(model);
        var cg = repo.GetBy(1);
        Console.WriteLine(
            $"{cg} {cg.WaterproofType.Name} {cg.FrostResistanceType.Name}"
        );
        //     .SaveChanges();
    }
    [Test]
    public void UserRepositoryTest()
    {
        var repo = new UserRepository(dataContext);
        foreach (var user in repo.GetAll())
        {
            Console.WriteLine(
                $"{user} {user.FullName} {user.Email}"
            );
        }
        var t1 = repo.GetBy(12);
        Console.WriteLine(t1);
        var u = repo.GetBy(3);
        Console.WriteLine(
            $"{u} {u.FullName} {u.Email}"
        );
        // cg.Mark = "213";
        // var model = new ConcreteGrade()
        // {
        //     Id=1, Mark = "М-100", Class = "В7,5", WaterproofTypeId = 2, FrostResistanceTypeId = 1, PricePerCubicMeter = 12
        // };
        // repo.Create(model);
        // var cg = repo.GetBy(1);
        // Console.WriteLine(
        //     $"{cg} {cg.WaterproofType.Name} {cg.FrostResistanceType.Name}"
        // );
        //     .SaveChanges();
    }
    
}