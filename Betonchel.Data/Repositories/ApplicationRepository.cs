using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.RepositoryStatuses.FailureStatuses;
using Betonchel.Domain.RepositoryStatuses.SuccessStatuses;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class ApplicationRepository : IFilterableRepository<Application, int>
{
    private readonly BetonchelContext _dataContext;

    public ApplicationRepository(BetonchelContext dataContext)
    {
        this._dataContext = dataContext;
    }

    public IQueryable<Application> GetAll()
    {
        return _dataContext.Applications
            .Include(ap => ap.ConcreteGrade)
            .Include(ap => ap.ConcretePump)
            .Include(ap => ap.User);
    }

    public IQueryable<Application> GetAll(params IFilter<Application>[] filters) =>
        filters.Aggregate(GetAll(), (current, filter) => filter.Filter(current));

    public Application? GetBy(int id) => GetAll().FirstOrDefault(a => a.Id == id);

    public async Task<IRepositoryOperationStatus> Create(Application model)
    {
        await using var transaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        var (user, concreteGrade, concretePump) = await GetForeignEntities(model);

        if (user is null)
            return new MissingReference<Application, User>();
        if (concreteGrade is null)
            return new MissingReference<Application, ConcreteGrade>();
        if (concretePump is null && model.ConcretePumpId is not null)
            return new MissingReference<Application, ConcretePump>();

        model.User = user;
        model.ConcreteGrade = concreteGrade;
        model.ConcretePump = concretePump;

        IRepositoryOperationStatus transactionStatus = await _dataContext.TrySaveEntity(model)
            ? new Success()
            : new UnexpectedError();
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public async Task<IRepositoryOperationStatus> Update(Application model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate is null) return new NotExist<Application>();

        await using var transaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        var (user, concreteGrade, concretePump) = await GetForeignEntities(model);

        if (user is null)
            return new MissingReference<Application, User>();
        if (concreteGrade is null)
            return new MissingReference<Application, ConcreteGrade>();
        if (concretePump is null && model.ConcretePumpId is not null)
            return new MissingReference<Application, ConcretePump>();

        toUpdate.CustomerName = model.CustomerName;
        toUpdate.UserId = model.UserId;
        toUpdate.ConcreteGradeId = model.ConcreteGradeId;
        toUpdate.TotalPrice = model.TotalPrice;
        toUpdate.ConcretePumpId = model.ConcretePumpId;
        toUpdate.ContactData = model.ContactData;
        toUpdate.Volume = model.Volume;
        toUpdate.DeliveryAddress = model.DeliveryAddress;
        toUpdate.DeliveryDate = model.DeliveryDate;
        toUpdate.Description = model.Description;
        toUpdate.Status = model.Status;

        IRepositoryOperationStatus transactionStatus = await _dataContext.TrySaveContext()
            ? new Success()
            : new UnexpectedError();
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public async Task<IRepositoryOperationStatus> DeleteBy(int id)
    {
        var application = await _dataContext.Applications.FindAsync(id);

        if (application is null) return new NotExist<Application>();

        await using var transaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        _dataContext.Applications.Remove(application);
        IRepositoryOperationStatus transactionStatus = await _dataContext.TrySaveContext()
            ? new Success()
            : new UnexpectedError();

        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    private async Task<(User?, ConcreteGrade?, ConcretePump?)> GetForeignEntities(Application model)
    {
        var user = await _dataContext.Users.SingleOrDefaultAsync(cg => cg.Id == model.UserId);
        var concreteGrade = await _dataContext.ConcreteGrades
            .SingleOrDefaultAsync(cg => cg.Id == model.ConcreteGradeId);
        var concretePump = model.ConcretePumpId is not null
            ? await _dataContext.ConcretePumps.SingleOrDefaultAsync(pump => pump.Id == model.ConcretePumpId)
            : null;
        return (user, concreteGrade, concretePump);
    }
}