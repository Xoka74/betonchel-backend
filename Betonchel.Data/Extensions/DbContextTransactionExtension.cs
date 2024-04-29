using Betonchel.Domain.Helpers;
using Microsoft.EntityFrameworkCore.Storage;

namespace Betonchel.Data.Extensions;

public static class DbContextTransactionExtension
{
    public static void CompleteWithStatus(this IDbContextTransaction transaction,
        RepositoryOperationStatus transactionStatus)
    {
        if (transactionStatus != RepositoryOperationStatus.Success)
            transaction.Rollback();
        else
            transaction.Commit();
    }
}