using System.Linq.Expressions;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.BaseModels;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
    Expression<Func<T, bool>> ToExpression();
}