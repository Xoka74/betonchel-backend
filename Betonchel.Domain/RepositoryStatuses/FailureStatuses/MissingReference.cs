using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class MissingReference<TReferer, TReferenced> : FailureOperationStatus
    where TReferer : class
    where TReferenced : class
{
    public MissingReference() : base($"{typeof(TReferer).Name}HasNo{typeof(TReferenced).Name}")
    {
    }
}