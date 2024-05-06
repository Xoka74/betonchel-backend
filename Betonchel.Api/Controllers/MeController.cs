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
    private readonly IFilterableRepository<User, int> repository;
    private readonly string checkUrl;
    private static readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public MeController(IFilterableRepository<User, int> repository, CheckUrl checkUrl)
    {
        this.repository = repository;
        this.checkUrl = checkUrl.Value;
    }

    [HttpGet]
    public async Task<IActionResult> GetMe()
    {
        var accessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
        if (accessToken is null || !await Authentication.CheckByAccessToken(accessToken, checkUrl))
            return Unauthorized();


        var decodedToken = _jwtSecurityTokenHandler.ReadToken(accessToken) as JwtSecurityToken;
        
        var email = decodedToken?.Claims.First(claim => claim.Type == "email");
        
        if (email == null)
        {
            return Unauthorized();
        }
        
        var user = await repository.GetAll().FirstOrDefaultAsync(x => x.Email == email.Value);

        return user != null ? Ok(user) : NotFound();
    }
}