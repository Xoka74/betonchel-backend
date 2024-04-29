using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class FrostResistanceTypeRepository : IFilterableRepository<FrostResistanceType, int>
{
    private readonly BetonchelContext dataContext;

    public FrostResistanceTypeRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<FrostResistanceType> GetAll() => dataContext.FrostResistanceTypes;

    public FrostResistanceType? GetBy(int id) => GetAll().SingleOrDefault(wpt => wpt.Id == id);

    public RepositoryOperationStatus Create(FrostResistanceType model)
    {
        if (!HasFrostResistanceTypeUniqueName(model))
            return RepositoryOperationStatus.UniquenessValueViolation;
        
        return dataContext.TrySaveEntity(model)
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError; 
    }

    public RepositoryOperationStatus Update(FrostResistanceType model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate is null)
            return RepositoryOperationStatus.NonExistentEntity;

        if (!HasFrostResistanceTypeUniqueName(model))
            return RepositoryOperationStatus.UniquenessValueViolation;

        toUpdate.Name = model.Name;

        return dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
    }

    public RepositoryOperationStatus DeleteBy(int id)
    {
        var waterproofType = dataContext.WaterproofTypes.Find(id);

        if (waterproofType is null)
            return RepositoryOperationStatus.NonExistentEntity;
     
        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        dataContext.WaterproofTypes.Remove(waterproofType);
        var transactionStatus = dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.HasReferences;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    private bool HasFrostResistanceTypeUniqueName(FrostResistanceType model) =>
        !GetFiltered(new FrostResistanceTypeNameFilter(model.Name)).Any();

    public IQueryable<FrostResistanceType> GetFiltered(Specification<FrostResistanceType> filter) =>
        GetAll().Where(filter);
}