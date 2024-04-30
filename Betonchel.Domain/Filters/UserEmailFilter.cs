using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.Filters;

public class UserEmailFilter : IFilter<User>
{
    private readonly string email;
    public UserEmailFilter(string email)
    {
        this.email = email;
    }

    public IQueryable<User> Filter(IQueryable<User> source) => source.Where(user => user.Email == email);
}
