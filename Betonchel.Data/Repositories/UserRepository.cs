using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

// TODO add checking for applications which relate with user
public class UserRepository : IBaseRepository<User, int>
{
    private readonly BetonchelContext dataContext;

    public UserRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<User> GetAll() => dataContext.Users;

    public User? GetBy(int id) => GetAll().FirstOrDefault(user => user.Id == id);

    public User? GetBy(string email) => GetAll().FirstOrDefault(user => user.Email == email);

    public bool Create(User model)
    {
        dataContext.Add(model);
        return TrySaveContext();
    }

    public bool Update(User model)
    {
        var toUpdate = dataContext.Users.FirstOrDefault(user => user.Id == model.Id);

        if (toUpdate == null) return false;

        toUpdate.FullName = model.FullName;
        toUpdate.Grade = model.Grade;
        toUpdate.PasswordHash = model.PasswordHash;
        toUpdate.Email = model.Email;
        return TrySaveContext();
    }

    public bool DeleteBy(int id)
    {
        var user = dataContext.Users.Find(id);

        if (user is null) return false;

        dataContext.Users.Remove(user);
        return TrySaveContext();
    }

    public bool DeleteBy(string email)
    {
        var user = GetBy(email);

        if (user is null) return false;

        dataContext.Users.Remove(user);
        return TrySaveContext();
    }

    private bool TrySaveContext()
    {
        try
        {
            dataContext.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
}