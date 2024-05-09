using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Betonchel.Api.Controllers;

[Route("api/auth")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IFilterableRepository<User, int> _userRepository;
    private readonly IBaseRepositoryWithAuth<RegisterUser, string?> _authRepository;

    public AuthController(IFilterableRepository<User, int> userRepository,
        IBaseRepositoryWithAuth<RegisterUser, string?> authRepository)
    {
        _userRepository = userRepository;
        _authRepository = authRepository;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUser model)
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null) return Unauthorized();
        var message = await _authRepository.Store(model, accessToken);
        if (message is not null) return BadRequest(message);
        var status = await _userRepository.Create(model.ToUser());
        return Ok(status);
    }
}