using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Betonchel.Identity;
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

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
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword
                }
            });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("admin", 
                policy => policy.RequireClaim("admin"));
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