using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class NotUnique<TModel> : IFailureOperationStatus
    where TModel : class
{
    public string Tokenize() => $"{typeof(TModel).Name}NotUnique";
}