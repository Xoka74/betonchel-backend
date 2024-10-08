﻿using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.RepositoryStatuses.FailureStatuses;
using Betonchel.Domain.RepositoryStatuses.SuccessStatuses;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class UserRepository : IFilterableRepository<User, int>
{
    private readonly BetonchelContext _dataContext;

    public UserRepository(BetonchelContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IQueryable<User> GetAll() => _dataContext.Users;

    public User? GetBy(int id) => GetAll().SingleOrDefault(user => user.Id == id);

    public async Task<IRepositoryOperationStatus> Create(User model)
    {
        await using var transaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        if (!HasUserUniqueEmail(model))
            return new NotUnique<User>();

        IRepositoryOperationStatus transactionStatus = await _dataContext.TrySaveEntity(model)
            ? new Success()
            : new UnexpectedError();

        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public async Task<IRepositoryOperationStatus> Update(User model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate is null)
            return new NotExist<User>();

        if (!HasUserUniqueEmail(model))
            return new NotUnique<User>();

        toUpdate.FullName = model.FullName;
        toUpdate.Email = model.Email;

        return await _dataContext.TrySaveContext()
            ? new Success()
            : new UnexpectedError();
    }

    public async Task<IRepositoryOperationStatus> DeleteBy(int id)
    {
        var user = await _dataContext.Users.FindAsync(id);

        if (user is null) return new NotExist<User>();

        await using var transaction = await _dataContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        _dataContext.Users.Remove(user);
        IRepositoryOperationStatus transactionStatus = await _dataContext.TrySaveContext()
            ? new Success()
            : new RestrictRelation<Application, User>();
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    private bool HasUserUniqueEmail(User model) => !GetAll(new UserEmailFilter(model.Email)).Any();

    public IQueryable<User> GetAll(params IFilter<User>[] filters) =>
        filters.Aggregate(GetAll(), (current, filter) => filter.Filter(current));
}