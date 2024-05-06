using System.Text;
using Newtonsoft.Json;
using Betonchel.Domain.JsonModels;

namespace Betonchel.Api.Utils;

public static class Authentication
{
    public static async Task<bool> CheckByAccessToken(string accessToken, string checkUrl)
    {
        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, checkUrl);
            
            request.Headers.Add("Authorization", accessToken);

            using var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }


    
    public static async Task<bool> Register(string registerUrl, RegisterUser user, string? accessToken)
    {
        try
        {
            using var client = new HttpClient();
            var json = JsonConvert.SerializeObject(new  { Email = user.Email, Password = user.Password });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, registerUrl);
    
            request.Headers.Add("Authorization", accessToken);
            request.Content = content;
        
            using var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

}