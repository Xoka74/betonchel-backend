using Betonchel.Data.Repositories;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.JsonModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Betonchel.Identity.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserRepository repository;

    public AuthController(UserRepository repository)
    {
        this.repository = repository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        return Ok("120992");
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
    {
        var baseUrl = "http://localhost:5073";
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
                access_token = tokenResponse.AccessToken, refresh_token = tokenResponse.RefreshToken
            };

            return Ok(tokens);
        }

        return StatusCode((int)response.StatusCode);
    }


    [HttpPost("getAccessRefreshToken")]
    public async Task<IActionResult> GetAccessRefresh([FromBody] LoginModel model)
    {
        var baseUrl = "http://localhost:5073";
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
                access_token = tokenResponse.AccessToken, refresh_token = tokenResponse.RefreshToken
            };

            return Ok(tokens);
        }

        return StatusCode((int)response.StatusCode);
    }

    [HttpPost("addUser")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        repository.Create(user);
        return Ok($"{user.FullName} added with role: {user.Grade}");
    }

    [HttpPost("removeUser")]
    [Authorize(Policy = "admin")]
    public async Task<IActionResult> RemoveUser([FromBody] User user)
    {
        var isRemoved = repository.DeleteBy(user.Email);
        if (!isRemoved)
            return NotFound("User not found");
        return Ok($"{user.FullName} deleted");
    }
}