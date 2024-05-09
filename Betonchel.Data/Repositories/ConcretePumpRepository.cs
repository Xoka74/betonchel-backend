using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.RepositoryStatuses.FailureStatuses;
using Betonchel.Domain.RepositoryStatuses.SuccessStatuses;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class ConcretePumpRepository : IBaseRepository<ConcretePump, int>
{
    private readonly BetonchelContext _dataContext;

    public ConcretePumpRepository(BetonchelContext dataContext)
    {
        this._dataContext = dataContext;
    }

    public IQueryable<ConcretePump> GetAll() => _dataContext.ConcretePumps;

    public ConcretePump? GetBy(int id) => GetAll().SingleOrDefault(pump => pump.Id == id);

    public async Task<IRepositoryOperationStatus> Create(ConcretePump model)
    {
        await using var transaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        IRepositoryOperationStatus transactionStatus = await _dataContext.TrySaveEntity(model)
            ? new Success()
            : new UnexpectedError();
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public async Task<IRepositoryOperationStatus> Update(ConcretePump model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate is null) return new NotExist<ConcretePump>();

        toUpdate.MaximumCapacity = model.MaximumCapacity;
        toUpdate.PipeLength = model.PipeLength;
        toUpdate.PricePerHour = model.PricePerHour;

        return await _dataContext.TrySaveContext()
            ? new Success()
            : new UnexpectedError();
    }

    public async Task<IRepositoryOperationStatus> DeleteBy(int id)
    {
        var concretePump = await _dataContext.ConcretePumps.FindAsync(id);

        if (concretePump is null) return new NotExist<ConcretePump>();

        await using var transaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        _dataContext.ConcretePumps.Remove(concretePump);
        IRepositoryOperationStatus transactionStatus = await _dataContext.TrySaveContext()
            ? new Success()
            : new RestrictRelation<Application, ConcretePump>();
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }
}