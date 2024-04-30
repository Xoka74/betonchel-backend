using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class RestrictRelation<TReferer, TReferenced> : IFailureOperationStatus
    where TReferer : class
    where TReferenced : class
{
    public string Tokenize() => $"{typeof(TReferer).Name}HasRestrictReferenceTo{typeof(TReferenced).Name}";
}