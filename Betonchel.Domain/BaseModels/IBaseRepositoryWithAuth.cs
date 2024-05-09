namespace Betonchel.Domain.BaseModels;

public interface IBaseRepositoryWithAuth<in TModel, TValidationResult>
    where TModel : class
{
    public Task<TValidationResult> Store(TModel model, string accessToken);
}