using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Betonchel.Data.Repositories;

public class UserRepository : IBaseRepository<User, int>
{
    public IQueryable<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public User? GetBy(int id)
    {
        throw new NotImplementedException();
    }
    
    public User? GetBy(string email)
    {
        throw new NotImplementedException();
    }

    public void Create(User model)
    {
        throw new NotImplementedException();
    }

    public void Update(User model)
    {
        throw new NotImplementedException();
    }

    public void DeleteBy(int id)
    {
        throw new NotImplementedException();
    }
    
    public bool DeleteBy(string email)
    {
        throw new NotImplementedException();
    }
}