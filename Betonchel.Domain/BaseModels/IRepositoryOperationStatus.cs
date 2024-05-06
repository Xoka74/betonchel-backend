using Betonchel.Domain.RepositoryStatuses.SuccessStatuses;

namespace Betonchel.Domain.BaseModels;

public interface IRepositoryOperationStatus
{
    public bool Success { get; }
}