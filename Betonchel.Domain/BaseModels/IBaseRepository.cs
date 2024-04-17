namespace Betonchel.Domain.BaseModels;

public interface IBaseRepository<TDbModel, in TDbModelPk>
    where TDbModel : Entity<TDbModelPk>
{
    public IQueryable<TDbModel> GetAll();
    public TDbModel? GetBy(TDbModelPk id);
    public void Create(TDbModel model);
    public void Update(TDbModel model);
    public void DeleteBy(TDbModelPk id);
}