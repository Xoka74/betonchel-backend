using Betonchel.Api.Utils;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Mvc;

namespace Betonchel.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IFilterableRepository<User, int> repository;
    private readonly string registerUrl;

    public AuthController(IFilterableRepository<User, int> repository, RegisterUrl registerUrl)
    {
        this.repository = repository;
        this.registerUrl = registerUrl.Value;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser model, [FromQuery]string accessToken)
    {
        var isCreated = await Authentication.Register(registerUrl, model, accessToken);
        if (!isCreated) return BadRequest();
        var status = await repository.Create(model.ToUser());
        return Ok(status);
    }
}