using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;

namespace Betonchel.Identity.Controllers;

public class TokenRequestModel
{
    public string access_token { get; set; }
}

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        return Ok("120992");
    } 
    
    
    [HttpPost("add")] 
    [Authorize(Policy="admin")]
    public IActionResult Add()
    {
        return Ok("Yes, BOSS");
    }
    
    [HttpPost("add1")]
    public IActionResult Add1([FromBody] TokenRequestModel request)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = request.access_token;
        var token = tokenHandler.ReadJwtToken(accessToken);
        var role = token.Claims.FirstOrDefault(claim => claim.Type == "role")?.Value;
        if (role != "admin")
        {
            return BadRequest("No access right");
        }
        
        
        return Ok("Админ, может добавлять менеджеров");
    }
}