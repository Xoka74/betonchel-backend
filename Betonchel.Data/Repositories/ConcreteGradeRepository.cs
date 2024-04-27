using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class ConcreteGradeRepository : IBaseRepository<ConcreteGrade, int>
{
    private readonly BetonchelContext dataContext;
    private readonly IBaseRepository<WaterproofType, int> waterproofTypeRepository;
    private readonly IBaseRepository<FrostResistanceType, int> frostResistanceTypeRepository;

    public ConcreteGradeRepository(
        BetonchelContext dataContext,
        IBaseRepository<WaterproofType, int> waterproofTypeRepository,
        IBaseRepository<FrostResistanceType, int> frostResistanceTypeRepository
    )
    {
        this.dataContext = dataContext;
        this.waterproofTypeRepository = waterproofTypeRepository;
        this.frostResistanceTypeRepository = frostResistanceTypeRepository;
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
        var waterproofType = waterproofTypeRepository.GetAll()
            .FirstOrDefault(wpt => wpt.Name == model.WaterproofType.Name);
        if (waterproofType is not null)
            model.WaterproofType = waterproofType;

        var frostResistanceType = frostResistanceTypeRepository.GetAll()
            .FirstOrDefault(frt => frt.Name == model.FrostResistanceType.Name);
        if (frostResistanceType is not null)
            model.FrostResistanceType = frostResistanceType;

        dataContext.Add(model);
        return TrySaveContext();
    }

    public bool Update(ConcreteGrade model)
    {
        var toUpdate = dataContext.ConcreteGrades.FirstOrDefault(cg => cg.Id == model.Id);

        if (toUpdate == null) return false;

        var waterproofType = waterproofTypeRepository.GetAll()
            .FirstOrDefault(wpt => wpt.Name == model.WaterproofType.Name);
        if (waterproofType is null) return false;

        var frostResistanceType = frostResistanceTypeRepository.GetAll()
            .FirstOrDefault(frt => frt.Name == model.FrostResistanceType.Name);
        if (frostResistanceType is null) return false;

        toUpdate.Mark = model.Mark;
        toUpdate.Class = model.Class;
        toUpdate.WaterproofTypeId = waterproofType.Id;
        toUpdate.FrostResistanceTypeId = frostResistanceType.Id;
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