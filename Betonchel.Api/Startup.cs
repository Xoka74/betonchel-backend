using System.Text;
using Betonchel.Api.SwaggerConfiguration;
using Betonchel.Data;
using Betonchel.Data.Repositories;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Betonchel.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<BetonchelContext>(options =>
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));

        AddRepositories(services);
        
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<BetonchelContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = "https://localhost:7016",
                    ValidIssuer = "https://localhost:7016",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret"))
                };
            });

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

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        app.UseSwagger();

        app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "Betonchel API V1"); });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<DataSeeder>();
        services.AddScoped<IFilterableRepository<ConcreteGrade, int>, ConcreteGradeRepository>();
        // services.AddScoped<IFilterableRepository<User, int>, UserRepository>();
        services.AddScoped<IBaseRepository<ConcretePump, int>, ConcretePumpRepository>();
        services.AddScoped<ApplicationRepository>();
    }
}