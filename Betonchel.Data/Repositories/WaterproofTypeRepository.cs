using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class WaterproofTypeRepository : IFilterableRepository<WaterproofType, int>
{
    private readonly BetonchelContext dataContext;

    public WaterproofTypeRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<WaterproofType> GetAll() => dataContext.WaterproofTypes;

    public WaterproofType? GetBy(int id) => GetAll().SingleOrDefault(wpt => wpt.Id == id);

    public RepositoryOperationStatus Create(WaterproofType model)
    {
        if (!HasWaterproofTypeUniqueName(model)) 
            return RepositoryOperationStatus.UniquenessValueViolation;

        return dataContext.TrySaveEntity(model)
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError; 
    }

    public RepositoryOperationStatus Update(WaterproofType model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate is null)
            return RepositoryOperationStatus.NonExistentEntity;

        if (!HasWaterproofTypeUniqueName(model))
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

    private bool HasWaterproofTypeUniqueName(WaterproofType model) =>
        !GetFiltered(new WaterproofTypeNameFilter(model.Name)).Any();

    public IQueryable<WaterproofType> GetFiltered(Specification<WaterproofType> filter) =>
        GetAll().Where(filter);
}