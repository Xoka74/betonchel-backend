using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class RegisterUser
{
    [Required] public string FullName { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }

    public User ToUser()
    {
        return new User()
        {
            FullName = FullName,
            Email = Email
        };
    }
}