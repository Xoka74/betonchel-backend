using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.Filters;

public class ApplicationStatusFilter : Specification<Application>
{
    public ApplicationStatusFilter(ApplicationStatus? status)
    {
        Predicate = status is null
        ? application => true
        : application => application.Status == status;
    }
}