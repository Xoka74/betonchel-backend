using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class UserApplication
{
    [Required]
    [StringLength(50, ErrorMessage = "TooLong")]
    public string CustomerName { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "OutOfRange")]
    public int ConcreteGradeId { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "OutOfRange")]
    public double TotalPrice { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "OutOfRange")]
    public int? ConcretePumpId { get; set; }

    [Required]
    [StringLength(512, ErrorMessage = "TooLong")]
    public string ContactData { get; set; }

    [Required]
    [Range(0.1, float.MaxValue, ErrorMessage = "OutOfRange")]
    public float Volume { get; set; }

    [StringLength(512, ErrorMessage = "TooLong")]
    public string? DeliveryAddress { get; set; }

    [Required] public DateTime DeliveryDate { get; set; }

    [StringLength(512, ErrorMessage = "TooLong")]
    public string? Description { get; set; }

    [Range(0, 4, ErrorMessage = "NotExisted")]
    public ApplicationStatus? Status { get; set; }

    public Application ToApplication(int userId, int id = 0)
    {
        return new Application
        {
            Id = id,
            UserId = userId,
            CustomerName = CustomerName,
            ConcreteGradeId = ConcreteGradeId,
            TotalPrice = TotalPrice,
            ConcretePumpId = ConcretePumpId,
            ContactData = ContactData,
            Volume = Volume,
            DeliveryAddress = DeliveryAddress,
            DeliveryDate = DeliveryDate,
            Description = Description,
            Status = Status ?? ApplicationStatus.Created
        };
    }
}