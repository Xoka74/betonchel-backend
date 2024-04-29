using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.Filters;

public class ApplicationDateFilter : Specification<Application>
{
    public ApplicationDateFilter(DateTime? date)
    {
        Predicate = date is null
        ? application => true
        : application => application.DeliveryDate == date.Value;
    }
}