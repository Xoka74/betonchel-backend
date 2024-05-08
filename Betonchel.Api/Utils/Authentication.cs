using System.Text;
using Newtonsoft.Json;
using Betonchel.Domain.JsonModels;

namespace Betonchel.Api.Utils;

public class Authentication
{
    private readonly string checkUrl;
    private readonly string registerUrl;
    
    public Authentication(string checkUrl, string registerUrl)
    {
        this.registerUrl = registerUrl;
        this.checkUrl = checkUrl;
    }
    
    public async Task<bool> CheckByAccessToken(string? accessToken) =>
        await SendRequest(checkUrl, accessToken) is null;


    public async Task<string?> TryRegister(RegisterUser user, string? accessToken)
    {
        var json = JsonConvert.SerializeObject(new { Email = user.Email, Password = user.Password });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await SendRequest(registerUrl, accessToken, content);
    }

    private static async Task<string?> SendRequest(string url, string? accessToken,
        HttpContent? content = null)
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("Authorization", accessToken);
            request.Content = content;
            using var response = await client.SendAsync(request);
            return !response.IsSuccessStatusCode
                ? await response.Content.ReadAsStringAsync()
                : null;
        }
        catch (Exception)
        {
            return "Unexpected error";
        }
    }
}