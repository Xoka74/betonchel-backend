using Betonchel.Domain.BaseModels;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.Filters;

public class FrostResistanceTypeNameFilter : Specification<FrostResistanceType>
{
    public FrostResistanceTypeNameFilter(string name)
    {
        Predicate = frt => frt.Name == name;
    }
}