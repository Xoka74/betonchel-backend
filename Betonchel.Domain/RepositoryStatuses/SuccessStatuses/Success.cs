using Betonchel.Domain.BaseModels;

namespace Betonchel.Domain.RepositoryStatuses.SuccessStatuses;

public class Success : ISuccessOperationStatus
{
    public string Tokenize() => "success";
}