using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Betonchel.Auth.Domen.Models;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Betonchel.Identity.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticateController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthenticateController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            return StatusCode(StatusCodes.Status500InternalServerError, new InvalidCredentials<User>());

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = CreateToken(authClaims);
        var refreshToken = GenerateRefreshToken();

        int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays);

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);

        await _userManager.UpdateAsync(user);

        return Ok(new SuccessLoginModelStatus
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        });
    }

    [HttpPost]
    [Route("register-manager")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new NotUnique<ApplicationUser>());

        ApplicationUser user = new()
        {
            UserName = model.Email,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new InvalidCredentials<User>());

        if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));

        if (await _roleManager.RoleExistsAsync(UserRoles.Manager))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Manager);
        }

        return Ok(new SuccessOperationStatusAuth());
    }

    [HttpPost]
    [Route("register-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
    {
        var userExists = await _userManager.FindByEmailAsync(model.Email);
        if (userExists != null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new NotUnique<ApplicationUser>());

        ApplicationUser user = new()
        {
            UserName = model.Email,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new InvalidCredentials<User>());

        if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

        if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
        {
            await _userManager.AddToRoleAsync(user, UserRoles.Admin);
        }

        return Ok(new SuccessOperationStatusAuth());
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        if (tokenModel is null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new InvalidClientRequestError<TokenModel>());
        }

        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new InvalidTokenError<TokenModel>());
        }

        var username = principal.Identity.Name;

        var user = await _userManager.FindByNameAsync(username);

        if (user == null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new NotExist<User>());

        if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            return StatusCode(StatusCodes.Status500InternalServerError,
                new InvalidRefreshTokenError<User>());

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return Ok(new SuccessLoginModelStatus
        {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken,
            Expiration = newAccessToken.ValidTo
        });
    }

    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return BadRequest("Invalid user name");

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = default;
        await _userManager.UpdateAsync(user);

        return Ok(new SuccessOperationStatusAuth());
    }

    [HttpPost]
    [Route("revoke-all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RevokeAll()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = default;
            await _userManager.UpdateAsync(user);
        }

        return Ok(new SuccessOperationStatusAuth());
    }

    [HttpPost]
    [Route("check")]
    [Authorize]
    public async Task<IActionResult> Check()
    {
        return Ok(new SuccessOperationStatusAuth());
    }


    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}