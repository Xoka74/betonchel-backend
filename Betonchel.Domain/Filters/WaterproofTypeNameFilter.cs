using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.Filters;

public class WaterproofTypeNameFilter : Specification<WaterproofType>
{
    public WaterproofTypeNameFilter(string name)
    {
        Predicate = wpt => wpt.Name == name;
    }
}