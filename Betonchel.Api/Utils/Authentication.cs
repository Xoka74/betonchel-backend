using System.Net.Http.Headers;
using System.Text;

namespace Betonchel.Api.Utils;

public static class Authentication
{
    public static async Task<bool> CheckByAccessToken(string accessToken)
    {
        try
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var content = new StringContent("", Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://localhost:5001/api/Authenticate/check", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }
}