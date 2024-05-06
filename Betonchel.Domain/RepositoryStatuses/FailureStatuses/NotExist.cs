using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class NotExist<TModel> : FailureOperationStatus
{
    public NotExist() : base($"{typeof(TModel).Name}NotExist")
    {
    }
}