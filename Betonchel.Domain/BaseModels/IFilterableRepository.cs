namespace Betonchel.Domain.BaseModels;

public interface IFilterableRepository<TDbModel, in TDbModelPk> : IBaseRepository<TDbModel, TDbModelPk>
    where TDbModel : Entity<TDbModelPk>
{
    public IQueryable<TDbModel> GetAll(params IFilter<TDbModel>[] filters);
}