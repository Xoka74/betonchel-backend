using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class MissingReference<TReferer, TReferenced> : IFailureOperationStatus
    where TReferer : class
    where TReferenced : class
{
    public string Tokenize() => $"{typeof(TReferer).Name}HasNoReferenceTo{typeof(TReferenced).Name}";
}