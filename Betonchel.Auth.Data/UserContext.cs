using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Auth.Data;

public class UserContext: IdentityDbContext<ApplicationUser>
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
}