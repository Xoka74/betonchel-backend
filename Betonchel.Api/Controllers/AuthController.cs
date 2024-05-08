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
    private readonly IFilterableRepository<User, int> _repository;
    private readonly Authentication _authentication;

    public AuthController(IFilterableRepository<User, int> repository, Authentication authentication)
    {
        _repository = repository;
        _authentication = authentication;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser model)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        var message = await _authentication.TryRegister(model, accessToken);
        if (message is not null) return BadRequest(message);
        var status = await _repository.Create(model.ToUser());
        return Ok(status);
    }
}