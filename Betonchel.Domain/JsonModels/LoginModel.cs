using System.ComponentModel.DataAnnotations;

namespace Betonchel.Domain.JsonModels;

public class LoginModel
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}