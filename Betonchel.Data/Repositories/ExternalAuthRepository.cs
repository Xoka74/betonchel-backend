using System.Net;
using System.Text;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.JsonModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Betonchel.Data.Repositories;

public class ExternalAuthRepository : IBaseRepositoryWithAuth<RegisterUser, string?>
{
    private readonly string _registerUrl;

    public ExternalAuthRepository(IConfiguration configuration)
    {
        _registerUrl = configuration["AuthServer:RegisterUrl"];
    }

    public async Task<string?> Store(RegisterUser user, string? accessToken)
    {
        var json = JsonConvert.SerializeObject(new { user.Email, user.Password });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var response = await SendRequest(_registerUrl, HttpMethod.Post, accessToken, content);
        return !response.IsSuccessStatusCode
            ? await response.Content.ReadAsStringAsync()
            : null;
    }

    private static async Task<HttpResponseMessage> SendRequest(string url, HttpMethod method, string? accessToken,
        HttpContent? content = null)
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(method, url);
            request.Headers.Add("Authorization", accessToken);
            request.Content = content;
            return await client.SendAsync(request);
        }
        catch (Exception)
        {
            return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        }
    }
}