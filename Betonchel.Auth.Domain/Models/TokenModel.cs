using System.ComponentModel.DataAnnotations;

namespace Betonchel.Auth.Domen.Models;

public class TokenModel
{ 
    [Required]
    public string AccessToken { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}