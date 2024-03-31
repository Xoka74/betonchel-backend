using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Betonchel.Api.Models;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Betonchel.Api;


[Route("[controller]")]
[ApiController]
public class UsersController: ControllerBase
{
    private static readonly List<User> _users = new List<User>
    {
        new User { Id = 1, 
            FirstName = "John", 
            LastName = "Doe", 
            Email = "john@example.com",
            DateOfBirth = new DateTime(1990, 1, 1), 
            PasswordHash = "password_hash" }
    };
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        
        // принимает в себя логин и пароль пользователя, возвращает access и refresh токены
        var user = _users.FirstOrDefault(u => u.Email == request.Username);
        if (user == null || user.PasswordHash != request.Password)
        {
            return Unauthorized("Invalid email or password");
        }
        
        var accessToken = "access_token";
        var refreshToken = "refresh_token";

        return Ok(new { access_token = accessToken, refresh_token = refreshToken });
    }

    [HttpPost("update-tokens")]
    public async Task<IActionResult> UpdateTokens([FromBody] RefreshTokenRequest request)
    {
        // принимает в себя refresh токен, возвращает новую сгенерированную пару refresh и access токенов
        var newAccessToken = "new_access_token";
        var newRefreshToken = "new_refresh_token";
        return Ok(new { access_token = newAccessToken, refresh_token = newRefreshToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // выход пользователя, поресерчить насчет работы
        return Ok("12");
    }
    
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}