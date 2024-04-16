namespace Betonchel.Domain.BaseModels;

public interface IBaseRepository<TDbModel, in TDbModelPk> : IDisposable
    where TDbModel : Entity<TDbModelPk>
{
    public IQueryable<TDbModel> GetAll();
    public TDbModel? GetBy(TDbModelPk id);
    public IBaseRepository<TDbModel, TDbModelPk> Create(TDbModel model);
    public IBaseRepository<TDbModel, TDbModelPk> Update(TDbModel model);
    public IBaseRepository<TDbModel, TDbModelPk> DeleteBy(TDbModelPk id);
    public IBaseRepository<TDbModel, TDbModelPk> SaveChanges();
}