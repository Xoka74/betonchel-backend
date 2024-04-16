using Betonchel.Domain.BaseModels;
using Betonchel.Domain.Models;

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
            .Join(
                dataContext.WaterproofTypes,
                cg => cg.WaterproofTypeId,
                wpt => wpt.Id,
                (cg, wpt) => new { ConcreteGrade = cg, WaterproofType = wpt }
            )
            .Join(
                dataContext.FrostResistanceTypes,
                union => union.ConcreteGrade.FrostResistanceTypeId,
                frt => frt.Id,
                (union, frt) => new ConcreteGrade()
                {
                    Id = union.ConcreteGrade.Id,
                    WaterproofTypeId = union.WaterproofType.Id,
                    FrostResistanceTypeId = frt.Id,
                    Class = union.ConcreteGrade.Class,
                    Mark = union.ConcreteGrade.Mark,
                    PricePerCubicMeter = union.ConcreteGrade.PricePerCubicMeter,
                    WaterproofType = union.WaterproofType,
                    FrostResistanceType = frt
                }
            );
    }

    public ConcreteGrade? GetBy(int id)
    {
        return GetAll().FirstOrDefault(cg => cg.Id == id);
    }

    public IBaseRepository<ConcreteGrade, int> Create(ConcreteGrade model)
    {
        dataContext.Add(model);
        return this;
    }

    public IBaseRepository<ConcreteGrade, int> Update(ConcreteGrade model)
    {
        var toUpdate = dataContext.ConcreteGrades.FirstOrDefault(cg => cg.Id == model.Id);

        if (toUpdate != null)
        {
            toUpdate.Mark = model.Mark;
            toUpdate.Class = model.Class;
            toUpdate.WaterproofTypeId = model.WaterproofTypeId;
            toUpdate.FrostResistanceTypeId = model.FrostResistanceTypeId;
            toUpdate.PricePerCubicMeter = model.PricePerCubicMeter;
            dataContext.Update(toUpdate);
        }

        return this;
    }

    public IBaseRepository<ConcreteGrade, int> DeleteBy(int id)
    {
        var concreteGrade = dataContext.ConcreteGrades.Find(id);
        if (concreteGrade is not null)
            dataContext.ConcreteGrades.Remove(concreteGrade);
        return this;
    }

    public IBaseRepository<ConcreteGrade, int> SaveChanges()
    {
        dataContext.SaveChanges();
        return this;
    }

    #region DisposablePattern

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool isDisposed;

    ~ConcreteGradeRepository()
    {
        Dispose(false);
    }

    private void Dispose(bool fromDisposeMethod)
    {
        if (isDisposed) return;
        SaveChanges();
        isDisposed = true;
    }

    #endregion
}