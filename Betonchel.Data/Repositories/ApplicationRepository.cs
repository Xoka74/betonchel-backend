using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class ApplicationRepository : IFilterableRepository<Application, int>
{
    private readonly BetonchelContext dataContext;
    private readonly IFilterableRepository<ConcreteGrade, int> concreteGradeRepository;
    private readonly IFilterableRepository<User, int> userRepository;
    private readonly IBaseRepository<ConcretePump, int> concretePumpRepository;

    public ApplicationRepository(
        BetonchelContext dataContext,
        IFilterableRepository<ConcreteGrade, int> concreteGradeRepository,
        IFilterableRepository<User, int> userRepository,
        IBaseRepository<ConcretePump, int> concretePumpRepository
    )
    {
        this.dataContext = dataContext;
        this.concreteGradeRepository = concreteGradeRepository;
        this.userRepository = userRepository;
        this.concretePumpRepository = concretePumpRepository;
    }

    public IQueryable<Application> GetAll()
    {
        return dataContext.Applications
            .Include(ap => ap.ConcreteGrade)
            .ThenInclude(cg => cg.WaterproofType)
            .Include(ap => ap.ConcreteGrade)
            .ThenInclude(cg => cg.FrostResistanceType)
            .Include(ap => ap.ConcretePump)
            .Include(ap => ap.User);
    }

    public Application? GetBy(int id) => GetAll().FirstOrDefault(a => a.Id == id);

    public RepositoryOperationStatus Create(Application model)
    {
        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        var (user, concreteGrade, concretePump) = GetForeignEntities(model);

        if (user is null || concreteGrade is null || (concretePump is null && model.ConcretePumpId is not null))
            return RepositoryOperationStatus.ForeignKeyViolation;

        model.User = user;
        model.ConcreteGrade = concreteGrade;
        model.ConcretePump = concretePump;

        var transactionStatus = dataContext.TrySaveEntity(model)
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public RepositoryOperationStatus Update(Application model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate is null)
            return RepositoryOperationStatus.NonExistentEntity;

        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);

        var (user, concreteGrade, concretePump) = GetForeignEntities(model);

        if (user is null || concreteGrade is null || (concretePump is null && model.ConcretePumpId is not null))
            return RepositoryOperationStatus.ForeignKeyViolation;

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

        var transactionStatus = dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public RepositoryOperationStatus DeleteBy(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Application> GetFiltered(Func<Application, bool> selector) => GetAll().Where(a => selector(a));

    private (User?, ConcreteGrade?, ConcretePump?) GetForeignEntities(Application model)
    {
        var user = userRepository.GetBy(model.UserId);
        var concreteGrade = concreteGradeRepository.GetBy(model.ConcreteGradeId);
        var concretePump = model.ConcretePumpId is null
            ? null
            : concretePumpRepository.GetBy(model.ConcretePumpId.Value);
        return (user, concreteGrade, concretePump);
    }

    public IQueryable<Application> GetFiltered(Specification<Application> filter) => GetAll().Where(filter);
}