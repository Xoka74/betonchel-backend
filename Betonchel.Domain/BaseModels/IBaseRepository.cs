using Betonchel.Domain.Helpers;

namespace Betonchel.Domain.BaseModels;

public interface IBaseRepository<TDbModel, in TDbModelPk> 
    where TDbModel : Entity<TDbModelPk>
{
    public IQueryable<TDbModel> GetAll();
    public TDbModel? GetBy(TDbModelPk id);
    public RepositoryOperationStatus Create(TDbModel model);
    public RepositoryOperationStatus Update(TDbModel model);
    public RepositoryOperationStatus DeleteBy(TDbModelPk id);
}