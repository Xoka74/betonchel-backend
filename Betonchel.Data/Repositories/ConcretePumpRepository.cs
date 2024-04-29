using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class ConcretePumpRepository : IBaseRepository<ConcretePump, int>
{
    private readonly BetonchelContext dataContext;

    public ConcretePumpRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<ConcretePump> GetAll() => dataContext.ConcretePumps;

    public ConcretePump? GetBy(int id) => GetAll().SingleOrDefault(pump => pump.Id == id);

    public RepositoryOperationStatus Create(ConcretePump model)
    {
        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        var transactionStatus = dataContext.TrySaveEntity(model)
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public RepositoryOperationStatus Update(ConcretePump model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate is null)
            return RepositoryOperationStatus.NonExistentEntity;

        toUpdate.MaximumCapacity = model.MaximumCapacity;
        toUpdate.PipeLength = model.PipeLength;
        toUpdate.PricePerHour = model.PricePerHour;

        return dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
    }

    public RepositoryOperationStatus DeleteBy(int id)
    {
        var concretePump = dataContext.ConcretePumps.Find(id);

        if (concretePump is null)
            return RepositoryOperationStatus.NonExistentEntity;

        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        dataContext.ConcretePumps.Remove(concretePump);
        var transactionStatus = dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.HasReferences;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }
}