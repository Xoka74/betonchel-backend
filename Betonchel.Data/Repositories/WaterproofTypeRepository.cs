using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class WaterproofTypeRepository : IBaseRepository<WaterproofType, int>
{
    private readonly BetonchelContext dataContext;

    public WaterproofTypeRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<WaterproofType> GetAll() => dataContext.WaterproofTypes;

    public WaterproofType? GetBy(int id) => GetAll().FirstOrDefault(wpt => wpt.Id == id);

    public bool Create(WaterproofType model)
    {
        dataContext.Add(model);
        return TrySaveContext();
    }

    public bool Update(WaterproofType model)
    {
        var toUpdate = dataContext.WaterproofTypes.FirstOrDefault(wpt => wpt.Id == model.Id);

        if (toUpdate is null)
            return false;

        toUpdate.Name = model.Name;
        return TrySaveContext();
    }

    public bool DeleteBy(int id)
    {
        var waterproofType = dataContext.WaterproofTypes.Find(id);

        if (waterproofType is null)
            return false;

        dataContext.WaterproofTypes.Remove(waterproofType);
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