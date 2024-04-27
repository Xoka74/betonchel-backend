using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class FrostResistanceTypeRepository : IBaseRepository<FrostResistanceType, int>
{
    private readonly BetonchelContext dataContext;

    public FrostResistanceTypeRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<FrostResistanceType> GetAll() => dataContext.FrostResistanceTypes;

    public FrostResistanceType? GetBy(int id) => GetAll().FirstOrDefault(frt => frt.Id == id);

    public bool Create(FrostResistanceType model)
    {
        dataContext.Add(model);
        return TrySaveContext();
    }

    public bool Update(FrostResistanceType model)
    {
        var toUpdate = dataContext.FrostResistanceTypes
            .FirstOrDefault(frt => frt.Id == model.Id);

        if (toUpdate is null)
            return false;

        toUpdate.Name = model.Name;
        return TrySaveContext();
    }

    public bool DeleteBy(int id)
    {
        var frostResistanceType = dataContext.FrostResistanceTypes.Find(id);

        if (frostResistanceType is null)
            return false;

        dataContext.FrostResistanceTypes.Remove(frostResistanceType);
        return TrySaveContext();
    }

    private bool TrySaveContext()
    {
        try
        {
            dataContext.SaveChanges();
            return true;
        }
        catch (DbUpdateException e)
        {
            return false;
        }
    }
}