using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class UnexpectedError : IFailureOperationStatus
{
    public string Tokenize() => "unexpectedError";
}