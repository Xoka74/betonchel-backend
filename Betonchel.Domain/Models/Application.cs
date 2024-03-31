using System.ComponentModel.DataAnnotations.Schema;

namespace Betonchel.Domain.Models;

public class Application
{
    public int ApplicationId { get; init; }
    public string CustomerName { get; init; }
    public string ManagerName { get; init; }
    public int ConcreteId { get; init; }
    public double TotalPrice { get; init; }
    public int ConcretePumpId { get; init; }
    [Column(TypeName = "json")] 
    public string ContactDate { get; init; }
    [Column(TypeName = "real")] 
    public float Volume { get; init; }
    [Column(TypeName = "json")] 
    public string DeliveryAddress { get; init; }
    [Column(TypeName = "timestamptz")]
    public DateTime DeliveryDate { get; init; }
    [Column(TypeName = "timestamptz")]
    public DateTime ApplicationCreationDate { get; init; }
}