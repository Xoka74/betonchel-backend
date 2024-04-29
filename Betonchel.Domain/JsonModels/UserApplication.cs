using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class UserApplication : IValidatableObject
{
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public int ConcreteGradeId { get; set; }
    [Required]
    public double TotalPrice { get; set; }
    public int? ConcretePumpId { get; set; }
    [Required]
    public string ContactData { get; set; }
    [Required]
    public float Volume { get; set; }
    public string? DeliveryAddress { get; set; }
    [Required]
    public DateTime DeliveryDate { get; set; }
    public string? Description { get; set; }
    public ApplicationStatus? Status { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CustomerName.Length > 50)
            yield return new ValidationResult(
                $"Customer name should have less then 50 symbols, but given {CustomerName.Length}",
                new[] { nameof(CustomerName) }
            );

        if (UserId < 1)
            yield return new ValidationResult(
                $"User id should be positive, but given {UserId}",
                new[] { nameof(UserId) }
            );

        if (ConcreteGradeId < 1)
            yield return new ValidationResult(
                $"Concrete grade id should be positive, but given {ConcreteGradeId}",
                new[] { nameof(ConcreteGradeId) }
            );

        if (TotalPrice < 0)
            yield return new ValidationResult(
                $"Total price of application should be non-negative, but given {TotalPrice}",
                new[] { nameof(TotalPrice) }
            );

        if (Volume < 0)
            yield return new ValidationResult(
                $"Concrete volume should be non-negative, but given {TotalPrice}",
                new[] { nameof(Volume) }
            );

        if (Description is not null && Description.Length > 512)
            yield return new ValidationResult(
                $"Application description should have less then 512 symbols, but given {Description.Length}",
                new[] { nameof(Description) }
            );
    }

    public Application ToApplication(int id = 0)
    {
        return new Application
        {
            Id = id,
            CustomerName = CustomerName,
            UserId = UserId,
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