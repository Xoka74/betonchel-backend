using System.ComponentModel.DataAnnotations.Schema;

namespace Betonchel.Domain.Models;

public class WaterproofType
{
    public int WaterproofTypeId { get; init; } 
    [Column(TypeName = "varchar(3)")] 
    public string Name { get; init; }
}