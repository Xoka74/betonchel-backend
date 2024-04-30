using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.Filters;

public class ApplicationDateFilter : IFilter<Application>
{
    private readonly DateTime? date;

    public ApplicationDateFilter(DateTime? date)
    {
        this.date = date;
    }

    public IQueryable<Application> Filter(IQueryable<Application> source) =>
        date is null ? source : source.Where(app => app.DeliveryDate == date);
}