namespace Betonchel.Domain.BaseModels;

public interface IBaseRepository<TDbModel, in TDbModelPk> 
    where TDbModel : Entity<TDbModelPk>
{
    public IQueryable<TDbModel> GetAll();
    public TDbModel? GetBy(TDbModelPk id);
    public bool Create(TDbModel model);
    public bool Update(TDbModel model);
    public bool DeleteBy(TDbModelPk id);
}