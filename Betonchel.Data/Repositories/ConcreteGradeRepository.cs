using Betonchel.Domain.BaseModels;
using Betonchel.Domain.Models;
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

    public void Create(ConcreteGrade model)
    {
        dataContext.Add(model);
        dataContext.SaveChanges();
    }

    public void Update(ConcreteGrade model)
    {
        var toUpdate = dataContext.ConcreteGrades.FirstOrDefault(cg => cg.Id == model.Id);

        if (toUpdate == null) return;
        
        toUpdate.Mark = model.Mark;
        toUpdate.Class = model.Class;
        toUpdate.WaterproofTypeId = model.WaterproofTypeId;
        toUpdate.FrostResistanceTypeId = model.FrostResistanceTypeId;
        toUpdate.PricePerCubicMeter = model.PricePerCubicMeter;
        dataContext.Update(toUpdate);
        dataContext.SaveChanges();
    }

    public void DeleteBy(int id)
    {
        var concreteGrade = dataContext.ConcreteGrades.Find(id);
        if (concreteGrade is not null)
            dataContext.ConcreteGrades.Remove(concreteGrade);
        dataContext.SaveChanges();
    }
}