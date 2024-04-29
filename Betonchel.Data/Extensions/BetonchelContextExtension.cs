using Betonchel.Domain.BaseModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Extensions;

public static class BetonchelContextExtension
{
    public static bool TrySaveContext(this BetonchelContext dataContext)
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
    
    public static bool TrySaveEntity<TDbModelPk>(this BetonchelContext dataContext, Entity<TDbModelPk> model)
    {
        try
        {
            dataContext.Add(model);
            dataContext.SaveChanges();
            return true;
        }
        catch (DbUpdateException)
        {
            dataContext.Remove(model);
            return false;
        }
    }
}