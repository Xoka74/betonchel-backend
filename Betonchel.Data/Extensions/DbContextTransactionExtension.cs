using Betonchel.Domain.BaseModels;
using Microsoft.EntityFrameworkCore.Storage;

namespace Betonchel.Data.Extensions;

public static class DbContextTransactionExtension
{
    public static void CompleteWithStatus(this IDbContextTransaction transaction, 
        IRepositoryOperationStatus transactionStatus)
    {
        if (transactionStatus is SuccessOperationStatus)
            transaction.Commit();
        else
            transaction.Rollback();
    }
}