namespace Betonchel.Domain.BaseModels;

public interface IFilter<TModel>
    where TModel : class
{
    public IQueryable<TModel> Filter(IQueryable<TModel> source);
}