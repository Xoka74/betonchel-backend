using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class UserApplication : IValidatableObject
{
    [Required] public string CustomerName { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public int ConcreteGradeId { get; set; }
    [Required] public double TotalPrice { get; set; }
    public int? ConcretePumpId { get; set; }
    [Required] public string ContactData { get; set; }
    [Required] public float Volume { get; set; }
    public string? DeliveryAddress { get; set; }
    [Required] public DateTime DeliveryDate { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CustomerName.Length > 50)
            yield return new ValidationResult(
                "CustomerNameTooLong",
                new[] { nameof(CustomerName) }
            );

        var numericValidations = new List<ValidationResult?>()
        {
            ValidateIfPositive(UserId, nameof(UserId)),
            ValidateIfPositive(ConcreteGradeId, nameof(ConcreteGradeId)),
            ValidateIfPositive(TotalPrice, nameof(TotalPrice)),
            ValidateIfPositive(Volume, nameof(Volume))
        };

        if (ConcretePumpId is not null)
            numericValidations.Add(ValidateIfPositive(ConcretePumpId.Value, nameof(ConcretePumpId)));

        foreach (var validation in numericValidations.Where(validation => validation != null))
            yield return validation;

        if (Description is not null && Description.Length > 512)
            yield return new ValidationResult(
                "DescriptionTooLong",
                new[] { nameof(Description) }
            );

        if (Status != null && !Enum.TryParse(typeof(ApplicationStatus), Status, out var status))
        {
            yield return new ValidationResult(
                "IncorrectApplicationStatus",
                new[] { nameof(ApplicationStatus) }
            );
        }
    }

    private static ValidationResult? ValidateIfPositive(double id, string fieldName)
    {
        if (id <= 0)
            return new ValidationResult(
                $"{fieldName}NotPositive",
                new[] { fieldName }
            );

        return null;
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
            Status = Status is null ? ApplicationStatus.Created : Enum.Parse<ApplicationStatus>(Status)
        };
    }
}