using Betonchel.Api.SwaggerConfiguration;
using Betonchel.Api.Utils;
using Betonchel.Data;
using Betonchel.Data.Repositories;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


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
        AddUrls(services, configuration);

        services.AddControllers();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Betonchel API", Version = "v1" });
            c.SchemaFilter<StartsWithSchemaFilter>();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.UseSwagger();

        app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "Betonchel API V1"); });
    }

    private static void AddUrls(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<CheckUrl>(_ => new CheckUrl(configuration["AuthServer:CheckUrl"]));
        services.AddScoped<RegisterUrl>(_ => new RegisterUrl(configuration["AuthServer:RegisterUrl"]));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IFilterableRepository<ConcreteGrade, int>, ConcreteGradeRepository>();
        services.AddScoped<IFilterableRepository<User, int>, UserRepository>();
        services.AddScoped<IBaseRepository<ConcretePump, int>, ConcretePumpRepository>();
        services.AddScoped<ApplicationRepository>();
    }
}