using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Betonchel.Domain.JsonModels;

namespace Betonchel.Api.Utils;

public static class Authentication
{
    private static readonly HttpClient Client;

    static Authentication()
    {
        Client = new HttpClient();
    }

    public static async Task<bool> CheckByAccessToken(string accessToken, string checkUrl)
    {
        try
        {
            var content = new StringContent("", Encoding.UTF8, "application/json");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await Client.PostAsync(checkUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static async Task<bool> Register(string registerUrl, RegisterUser user, string accessToken)
    {
        try
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = new StringContent(
                JsonConvert.SerializeObject(new { Email = user.Email, Password = user.Password }),
                Encoding.UTF8,
                "application/json"
            );
            var response = await Client.PostAsync(registerUrl, content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}