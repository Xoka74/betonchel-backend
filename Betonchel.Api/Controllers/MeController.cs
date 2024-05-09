using System.Security.Claims;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Api.Controllers;

[Route("api/me")]
[ApiController]
public class MeController : ControllerBase
{
    private readonly IFilterableRepository<User, int> repository;

    public MeController(IFilterableRepository<User, int> repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetMe()
    {
        var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);

        if (email == null)
        {
            return Unauthorized();
        }

        var user = await repository.GetAll().FirstOrDefaultAsync(x => x.Email == email);

        return user != null ? Ok(user) : NotFound();
    }
}