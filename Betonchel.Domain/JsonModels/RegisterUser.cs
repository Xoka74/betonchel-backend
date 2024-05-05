using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class RegisterUser
{
    [Required]
    public string FullName;
    [Required]
    public string Email;
    [Required]
    public string Password;

    public User ToUser()
    {
        return new User()
        {
            FullName = FullName,
            Email = Email
        };
    }
}