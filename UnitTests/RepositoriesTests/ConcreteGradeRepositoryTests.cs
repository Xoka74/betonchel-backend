using Betonchel.Data;
using Betonchel.Data.Repositories;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.RepositoryStatuses.SuccessStatuses;
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
        // var repo = new ConcreteGradeRepository(dataContext);
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

    [Test]
    public void Test2()
    {
        var appRepo = new ApplicationRepository(dataContext);
        var model = new Application
        {
            Id = 12,
            CustomerName = "Till Lindeman",
            UserId = 2,
            ConcreteGradeId = 6,
            TotalPrice = 19300,
            ConcretePumpId = 1,
            ApplicationCreationDate = DateTime.Parse("10/10/2024"),
            ContactData = "{\"phone\":  \"+11111\"}",
            Volume = 123123131231,
            DeliveryAddress = "{\"ulitsa\": \"Pushkina\"}",
            DeliveryDate = DateTime.Parse("01/10/2024"),
            Description = "cosino norm",
            Status = ApplicationStatus.Rejected
        };
        var task = appRepo.Update(model);
        task.Wait();
        if (!task.IsCompletedSuccessfully) return;
        
        var status = task.Result; 
        Assert.That(status is SuccessOperationStatus, Is.True);
    }

    [TestCase]
    public void Test3()
    {
        var appRepo = new ApplicationRepository(dataContext);
        var filter = new ApplicationDateFilter(DateTime.Parse("28/07/2024"));
        foreach (var cg in appRepo.GetAll(filter))
        {
            Console.WriteLine(cg);
        }
    }
}