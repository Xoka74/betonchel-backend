using System.Security.Claims;
using Betonchel.Data;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Betonchel.Identity;
public class Startup
{
    private readonly IConfiguration _configuration; 

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration; 
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<BetonchelContext>(options =>
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));
        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryApiScopes(new[] { new ApiScope("api1", userClaims: new[] { JwtClaimTypes.Role }),})
            .AddTestUsers(new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = "idAdmin",
                    Username = "admin",
                    Password = "admin",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Role, "admin"),
                    }
                },
                new TestUser()
                {
                    SubjectId = "idmaneger",
                    Username = "manager",
                    Password = "manager",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Role, "manager"),
                    }
                }
            })
            .AddInMemoryClients(new[]
            {
                new Client
                {
                    Enabled = true,
                    ClientId = "AnyClientId",
                    ClientName = "ClientName",
                    AccessTokenType = AccessTokenType.Jwt,
                    AllowedScopes = { "api1" },
                    UpdateAccessTokenClaimsOnRefresh = true,
                    IncludeJwtId = true,
                    ClientSecrets = {new Secret("S3cr3t".Sha256())},
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 3600,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    AbsoluteRefreshTokenLifetime = 3600,
                    SlidingRefreshTokenLifetime = 7200,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                }
            });
        
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5073"; // Укажите соответствующий URL
                options.RequireHttpsMetadata = false; // Временно для отладки
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false // Настройте соответственно вашим требованиям
                };
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("admin", policy => policy.RequireRole("admin"));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseIdentityServer();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}