using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.Filters;

public class UserEmailFilter : Specification<User>
{
    public UserEmailFilter(string email)
    {
        Predicate = user => user.Email == email;
    }
}
