using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.Filters;

public class ApplicationStatusFilter : IFilter<Application>
{
    private readonly ApplicationStatus? status;

    public ApplicationStatusFilter(ApplicationStatus? status)
    {
        this.status = status;
    }

    public IQueryable<Application> Filter(IQueryable<Application> source) => 
        status is null ? source : source.Where(app => app.Status == status.Value);
}