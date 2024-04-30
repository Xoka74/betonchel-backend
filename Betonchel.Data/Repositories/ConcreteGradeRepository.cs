using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.RepositoryStatuses.FailureStatuses;
using Betonchel.Domain.RepositoryStatuses.SuccessStatuses;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class ConcreteGradeRepository : IFilterableRepository<ConcreteGrade, int>
{
    private readonly BetonchelContext dataContext;

    public ConcreteGradeRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<ConcreteGrade> GetAll() => dataContext.ConcreteGrades;

    public ConcreteGrade? GetBy(int id) => GetAll().SingleOrDefault(cg => cg.Id == id);

    public async Task<IRepositoryOperationStatus> Create(ConcreteGrade model)
    {
        await using var transaction = await dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        IRepositoryOperationStatus transactionStatus = await dataContext.TrySaveEntity(model)
            ? new Success()
            : new UnexpectedError();

        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public async Task<IRepositoryOperationStatus> Update(ConcreteGrade model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate == null) return new NotExist<ConcreteGrade>();

        await using var transaction = await dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        toUpdate.Mark = model.Mark;
        toUpdate.Class = model.Class;
        toUpdate.WaterproofType = model.WaterproofType;
        toUpdate.FrostResistanceType = model.FrostResistanceType;
        toUpdate.PricePerCubicMeter = model.PricePerCubicMeter;

        IRepositoryOperationStatus transactionStatus = await dataContext.TrySaveContext()
            ? new Success()
            : new UnexpectedError();
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public async Task<IRepositoryOperationStatus> DeleteBy(int id)
    {
        var concreteGrade = await dataContext.ConcreteGrades.FindAsync(id);

        if (concreteGrade is null) return new NotExist<ConcreteGrade>();

        await using var transaction = await dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        dataContext.ConcreteGrades.Remove(concreteGrade);
        IRepositoryOperationStatus transactionStatus = await dataContext.TrySaveContext()
            ? new Success()
            : new RestrictRelation<Application, ConcreteGrade>();
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public IQueryable<ConcreteGrade> GetAll(params IFilter<ConcreteGrade>[] filters) =>
        filters.Aggregate(GetAll(), (current, filter) => filter.Filter(current));
}