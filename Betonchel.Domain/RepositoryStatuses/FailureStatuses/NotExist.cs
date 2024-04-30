using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class NotExist<TModel> : IFailureOperationStatus
{
    public string Tokenize() => $"{typeof(TModel).Name}NotExist";
}