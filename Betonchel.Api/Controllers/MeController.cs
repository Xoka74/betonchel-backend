using System.IdentityModel.Tokens.Jwt;
using Betonchel.Api.Utils;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Api.Controllers;

[Route("api/me")]
[ApiController]
public class MeController : ControllerBase
{
    private readonly IFilterableRepository<User, int> _repository;
    private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();
    private readonly Authentication _authentication;

    public MeController(IFilterableRepository<User, int> repository, Authentication authentication)
    {
        _repository = repository;
        _authentication = authentication;
    }

    [HttpGet]
    public async Task<IActionResult> GetMe()
    {
        var accessToken = Request.Headers["Authorization"].ToString();
        if (accessToken is null || !await _authentication.CheckByAccessToken(accessToken))
            return Unauthorized();
        
        var decodedToken = _jwtSecurityTokenHandler.ReadToken(accessToken.Replace("Bearer ", "")) as JwtSecurityToken;

        var email = decodedToken?.Claims.First(claim => claim.Type == "email");

        if (email == null)
        {
            return Unauthorized();
        }

        var user = await _repository.GetAll().FirstOrDefaultAsync(x => x.Email == email.Value);

        return user != null ? Ok(user) : NotFound();
    }
}