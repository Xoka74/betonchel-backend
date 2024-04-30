namespace Betonchel.Domain.BaseModels;

public interface IBaseRepository<TDbModel, in TDbModelPk>
    where TDbModel : Entity<TDbModelPk>
{
    public IQueryable<TDbModel> GetAll();
    public TDbModel? GetBy(TDbModelPk id);
    public Task<IRepositoryOperationStatus> Create(TDbModel model);
    public Task<IRepositoryOperationStatus> Update(TDbModel model);
    public Task<IRepositoryOperationStatus> DeleteBy(TDbModelPk id);
}