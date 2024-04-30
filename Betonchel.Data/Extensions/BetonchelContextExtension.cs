using Betonchel.Domain.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Extensions;

public static class BetonchelContextExtension
{
    public static async Task<bool> TrySaveContext(this BetonchelContext dataContext)
    {
        try
        {
            await dataContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    
    public static async Task<bool> TrySaveEntity<TDbModelPk>(this BetonchelContext dataContext, Entity<TDbModelPk> model)
    {
        try
        {
            await dataContext.AddAsync(model);
            await dataContext.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            dataContext.Remove(model);
            return false;
        }
    }
}