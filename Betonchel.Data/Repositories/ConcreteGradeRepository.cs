using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class ConcreteGradeRepository : IFilterableRepository<ConcreteGrade, int>
{
    private readonly BetonchelContext dataContext;
    private readonly IFilterableRepository<WaterproofType, int> waterproofTypeRepository;
    private readonly IFilterableRepository<FrostResistanceType, int> frostResistanceTypeRepository;

    public ConcreteGradeRepository(
        BetonchelContext dataContext,
        IFilterableRepository<WaterproofType, int> waterproofTypeRepository,
        IFilterableRepository<FrostResistanceType, int> frostResistanceTypeRepository
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

    public ConcreteGrade? GetBy(int id) => GetAll().SingleOrDefault(cg => cg.Id == id);

    public RepositoryOperationStatus Create(ConcreteGrade model)
    {
        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        var waterproofType = waterproofTypeRepository
            .GetFiltered(new WaterproofTypeNameFilter(model.WaterproofType.Name))
            .FirstOrDefault();

        if (waterproofType is not null)
            model.WaterproofType = waterproofType;

        var frostResistanceType = frostResistanceTypeRepository
            .GetFiltered(new FrostResistanceTypeNameFilter(model.FrostResistanceType.Name))
            .FirstOrDefault();

        if (frostResistanceType is not null)
            model.FrostResistanceType = frostResistanceType;

        var transactionStatus = dataContext.TrySaveEntity(model)
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public RepositoryOperationStatus Update(ConcreteGrade model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate == null) return RepositoryOperationStatus.NonExistentEntity;
        
        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        var waterproofType = waterproofTypeRepository.GetAll()
            .FirstOrDefault(wpt => wpt.Name == model.WaterproofType.Name);
        if (waterproofType is null)
            return RepositoryOperationStatus.ForeignKeyViolation;

        var frostResistanceType = frostResistanceTypeRepository.GetAll()
            .FirstOrDefault(frt => frt.Name == model.FrostResistanceType.Name);
        if (frostResistanceType is null)
            return RepositoryOperationStatus.ForeignKeyViolation;

        toUpdate.Mark = model.Mark;
        toUpdate.Class = model.Class;
        toUpdate.WaterproofTypeId = waterproofType.Id;
        toUpdate.FrostResistanceTypeId = frostResistanceType.Id;
        toUpdate.PricePerCubicMeter = model.PricePerCubicMeter;

        var transactionStatus = dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public RepositoryOperationStatus DeleteBy(int id)
    {
        var concreteGrade = dataContext.ConcreteGrades.Find(id);

        if (concreteGrade is null)
            return RepositoryOperationStatus.NonExistentEntity;
        
        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        dataContext.ConcreteGrades.Remove(concreteGrade);
        var transactionStatus = dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.HasReferences;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public IQueryable<ConcreteGrade> GetFiltered(Specification<ConcreteGrade> filter) => GetAll().Where(filter);
}