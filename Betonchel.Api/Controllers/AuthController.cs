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
    public async Task<IActionResult> Register([FromBody] RegisterUser model)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        var message = await Authentication.TryRegister(registerUrl, model, accessToken);
        if (message is not null) return BadRequest(message);
        var status = await repository.Create(model.ToUser());
        return Ok(status);
    }
}