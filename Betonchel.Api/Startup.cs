using Betonchel.Data;
using Betonchel.Data.Repositories;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Api;

public class Startup
{
    private readonly IConfiguration configuration;

    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<BetonchelContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        AddRepositories(services);

        services.AddHttpClient(
            "UserCheck",
            client => client.BaseAddress = new Uri(configuration["AuthenticationServer:BaseUrl"])
        );

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IFilterableRepository<ConcreteGrade, int>, ConcreteGradeRepository>();
        services.AddScoped<IFilterableRepository<WaterproofType, int>, WaterproofTypeRepository>();
        services.AddScoped<IFilterableRepository<FrostResistanceType, int>, FrostResistanceTypeRepository>();
        services.AddScoped<IFilterableRepository<User, int>, UserRepository>();
        services.AddScoped<IBaseRepository<ConcretePump, int>, ConcretePumpRepository>();
        services.AddScoped<IFilterableRepository<Application, int>, ApplicationRepository>();
    }
}