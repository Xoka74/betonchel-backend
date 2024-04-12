using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using IdentityModel.Client;
using System.IdentityModel.Tokens.Jwt;
using Betonchel.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Newtonsoft.Json;

namespace Betonchel.Identity.Controllers;

public class TokenRequestModel
{
    public string access_token { get; set; }
}

public class LoginModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}

public class TokenResponse
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
}

public class RefreshTokenModel
{
    public string RefreshToken { get; set; }
}


[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DbContext _dbContext;

    public AuthController(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        return Ok("120992");
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
    {
        var baseUrl  = "http://localhost:5073";
        var httpClient = new HttpClient();
        var tokenEndpoint = "/connect/token";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", "AnyClientId"),
            new KeyValuePair<string, string>("client_secret", "S3cr3t"),
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", model.RefreshToken),
        });
    
        var response = await httpClient.PostAsync(baseUrl + tokenEndpoint, content);
    
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
        
            var tokens = new 
            {
                tokenResponse.access_token, tokenResponse.refresh_token
            };

            return Ok(tokens);
        }
    
        return StatusCode((int)response.StatusCode);
    }

    
    [HttpPost("getAccessRefreshToken")]
    public async Task<IActionResult> GetAccessRefresh([FromBody] LoginModel model)
    {
        var baseUrl  = "http://localhost:5073";
        var httpClient = new HttpClient();
        var tokenEndpoint = "/connect/token";
        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("client_id", "AnyClientId"),
            new KeyValuePair<string, string>("client_secret", "S3cr3t"),
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", model.Username),
            new KeyValuePair<string, string>("password", model.Password),
        });
        
        var response = await httpClient.PostAsync(baseUrl + tokenEndpoint, content);
        
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
            
            var tokens = new 
            {
                tokenResponse.access_token, tokenResponse.refresh_token
            };

            return Ok(tokens);
        }
        
        return StatusCode((int)response.StatusCode);
    }
    
    [HttpPost("addUser")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        user.DateOfBirth = DateTime.SpecifyKind(user.DateOfBirth, DateTimeKind.Utc);
        _dbContext.AddUser(user);
        return Ok($"{user.FirstName} {user.LastName} added with role: {user.Role}");
    }
    
    [HttpPost("removeUser")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> RemoveUser([FromBody] User user)
    {
        var userToDelete = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        Console.WriteLine(_dbContext.Users);
        if (userToDelete == null)
        {
            return NotFound("User not found");
        }
        _dbContext.RemoveUser(userToDelete);
        return Ok($"{user.FirstName} {user.LastName} deleted");
    }
}