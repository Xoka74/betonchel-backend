using Betonchel.Domain.DBModels;
using Microsoft.AspNetCore.Identity;

namespace Betonchel.Api;

public class DataSeeder
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DataSeeder(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task SeedDataAsync()
    {
        if (!_userManager.Users.Any())
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            var adminEmail = "admin@mail.ru";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var admin = new User
                {
                    
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Админ",
                    LastName = "Админович"
                };

                var result = await _userManager.CreateAsync(admin, "Admin#24");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, UserRoles.Admin);
                }
            }
        }
    }
}