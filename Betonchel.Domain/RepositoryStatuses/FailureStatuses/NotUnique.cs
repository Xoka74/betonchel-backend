using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class NotUnique<TModel> : FailureOperationStatus
    where TModel : class
{
    public NotUnique() : base($"{typeof(TModel).Name}NotUnique")
    {
    }
}