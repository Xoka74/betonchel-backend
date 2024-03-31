using System.ComponentModel.DataAnnotations.Schema;

namespace Betonchel.Domain.Models;

public class FrostResistanceType
{
    public int FrostResistanceTypeId { get; init; }
    [Column(TypeName = "varchar(5)")]
    public string Name { get; init; }
}