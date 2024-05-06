using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class RestrictRelation<TReferer, TReferenced> : FailureOperationStatus
    where TReferer : class
    where TReferenced : class
{
    public RestrictRelation() : base($"{typeof(TReferer).Name}Has{typeof(TReferenced).Name}")
    {
    }
}