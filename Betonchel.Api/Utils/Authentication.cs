using System.Net;
using System.Text;
using Newtonsoft.Json;
using Betonchel.Domain.JsonModels;

namespace Betonchel.Api.Utils;

public class Authentication
{
    private readonly string _resolveUrl;
    private readonly string _registerUrl;

    public Authentication(string resolveUrl, string registerUrl)
    {
        _resolveUrl = resolveUrl;
        _registerUrl = registerUrl;
    }

    public async Task<bool> CheckByAccessToken(string? accessToken)
    {
        using var response = await SendRequest(_resolveUrl, HttpMethod.Get, accessToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<string?> ResolveBy(string? accessToken)
    {
        using var response = await SendRequest(_resolveUrl, HttpMethod.Get, accessToken);
        return response.IsSuccessStatusCode
            ? JsonConvert.DeserializeAnonymousType(
                await response.Content.ReadAsStringAsync(),
                new { Email = "" }
            )?.Email
            : null;
    }


    public async Task<string?> TryRegister(RegisterUser user, string? accessToken)
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