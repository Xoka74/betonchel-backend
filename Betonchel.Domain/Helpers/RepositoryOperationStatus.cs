namespace Betonchel.Domain.Helpers;

public enum RepositoryOperationStatus
{
    ForeignKeyViolation,
    NonExistentEntity,
    HasReferences,
    UniquenessValueViolation, 
    UnexpectedError,
    Success
}