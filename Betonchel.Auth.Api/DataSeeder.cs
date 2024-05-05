using Betonchel.Domain.DBModels;
using Microsoft.AspNetCore.Identity;

namespace Betonchel.Identity;

public class DataSeeder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public DataSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task SeedDataAsync()
    {
        if (!_userManager.Users.Any())
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            var adminEmail = _configuration["Admin:Email"];
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await _userManager.CreateAsync(admin, _configuration["Admin:Password"]);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, UserRoles.Admin);
                }
            }
        }
    }
}