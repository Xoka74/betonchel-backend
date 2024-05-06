namespace Betonchel.Domain.BaseModels;

public class FailureOperationStatus : IRepositoryOperationStatus
{
    public bool Success => false;
    public string Error { get; }

    protected FailureOperationStatus(string error)
    {
        Error = error;
    }
}