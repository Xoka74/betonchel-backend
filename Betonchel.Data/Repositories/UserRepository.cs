﻿using System.Data;
using Betonchel.Data.Extensions;
using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;
using Betonchel.Domain.Filters;
using Betonchel.Domain.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Betonchel.Data.Repositories;

public class UserRepository : IFilterableRepository<User, int>
{
    private readonly BetonchelContext dataContext;

    public UserRepository(BetonchelContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public IQueryable<User> GetAll() => dataContext.Users;

    public User? GetBy(int id) => GetAll().SingleOrDefault(user => user.Id == id);

    public RepositoryOperationStatus Create(User model)
    {
        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);
     
        if (!HasUserUniqueEmail(model)) 
            return RepositoryOperationStatus.UniquenessValueViolation;

        var transactionStatus = dataContext.TrySaveEntity(model)
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    public RepositoryOperationStatus Update(User model)
    {
        var toUpdate = GetBy(model.Id);

        if (toUpdate is null)
            return RepositoryOperationStatus.NonExistentEntity;

        if (!HasUserUniqueEmail(model))
            return RepositoryOperationStatus.UniquenessValueViolation;

        toUpdate.FullName = model.FullName;
        toUpdate.Grade = model.Grade;
        toUpdate.PasswordHash = model.PasswordHash;
        toUpdate.Email = model.Email;

        return dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.UnexpectedError;
    }

    public RepositoryOperationStatus DeleteBy(int id)
    {
        var user = dataContext.Users.Find(id);

        if (user is null)
            return RepositoryOperationStatus.NonExistentEntity;
        
        using var transaction = dataContext.Database.BeginTransaction(IsolationLevel.RepeatableRead);
        
        dataContext.Users.Remove(user);
        var transactionStatus = dataContext.TrySaveContext()
            ? RepositoryOperationStatus.Success
            : RepositoryOperationStatus.HasReferences;
        transaction.CompleteWithStatus(transactionStatus);
        return transactionStatus;
    }

    private bool HasUserUniqueEmail(User model) => !GetFiltered(new UserEmailFilter(model.Email)).Any();

    public IQueryable<User> GetFiltered(Specification<User> filter) => GetAll().Where(filter);
}