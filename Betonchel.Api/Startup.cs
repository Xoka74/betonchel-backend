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

        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<ConcreteGrade, int>, ConcreteGradeRepository>();
        services.AddScoped<IBaseRepository<WaterproofType, int>, WaterproofTypeRepository>();
        services.AddScoped<IBaseRepository<FrostResistanceType, int>, FrostResistanceTypeRepository>();
        services.AddScoped<IBaseRepository<User, int>, UserRepository>();
    }
}