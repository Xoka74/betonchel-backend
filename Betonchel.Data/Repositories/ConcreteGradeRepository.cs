using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public sealed class ConcreteGradeRepository : IBaseRepository<ConcreteGrade, int>
{
    private readonly BetonchelContext dataContext;

    public ConcreteGradeRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<ConcreteGrade> GetAll()
    {
        return dataContext.ConcreteGrades
            .Include(cg => cg.WaterproofType)
            .Include(cg => cg.FrostResistanceType);
    }

    public ConcreteGrade? GetBy(int id)
    {
        return GetAll().FirstOrDefault(cg => cg.Id == id);
    }

    public bool Create(ConcreteGrade model)
    {
        dataContext.Add(model);
        return TrySaveContext();
    }

    public bool Update(ConcreteGrade model)
    {
        var toUpdate = dataContext.ConcreteGrades.FirstOrDefault(cg => cg.Id == model.Id);

        if (toUpdate == null) 
            return false;

        toUpdate.Mark = model.Mark;
        toUpdate.Class = model.Class;
        toUpdate.WaterproofTypeId = model.WaterproofTypeId;
        toUpdate.FrostResistanceTypeId = model.FrostResistanceTypeId;
        toUpdate.PricePerCubicMeter = model.PricePerCubicMeter;
        dataContext.Update(toUpdate);
        return TrySaveContext();
    }

    public bool DeleteBy(int id)
    {
        var concreteGrade = dataContext.ConcreteGrades.Find(id);
        
        if (concreteGrade is null) 
            return false;
        
        dataContext.ConcreteGrades.Remove(concreteGrade);
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