using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.FailureStatuses;

public class UnexpectedError : FailureOperationStatus
{
    public UnexpectedError() : base("UnexpectedError")
    {
    }
}