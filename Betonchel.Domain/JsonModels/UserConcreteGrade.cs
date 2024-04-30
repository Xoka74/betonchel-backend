using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class UserConcreteGrade : IValidatableObject
{
    [Required] public string Mark { get; set; }
    [Required] public string Class { get; set; }
    [Required] public string WaterproofType { get; set; }
    [Required] public string FrostResistanceType { get; set; }
    [Required] public double PricePerCubicMeter { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PricePerCubicMeter < 0)
            yield return new ValidationResult(
                "PricePerCubicMeterIsNegative",
                new[] { nameof(PricePerCubicMeter) }
            );

        foreach (var validation in ValidateName(Mark, "m", 10, nameof(Mark)))
            yield return validation;

        foreach (var validation in ValidateName(Class, "b", 10, nameof(Class)))
            yield return validation;

        foreach (var validation in ValidateName(FrostResistanceType, "f", 10, nameof(FrostResistanceType)))
            yield return validation;

        foreach (var validation in ValidateName(WaterproofType, "w", 10, nameof(WaterproofType)))
            yield return validation;
    }

    private IEnumerable<ValidationResult> ValidateName(
        string name,
        string nameBeginning,
        int maxLength,
        string fieldName
    )
    {
        if (!name.StartsWith(nameBeginning, StringComparison.InvariantCultureIgnoreCase))
            yield return new ValidationResult(
                $"{fieldName}ShouldStartWith{nameBeginning}",
                new[] { fieldName }
            );

        if (name.Length > maxLength)
            yield return new ValidationResult(
                $"{fieldName}TooLong",
                new[] { fieldName }
            );
    }


    public ConcreteGrade ToConcreteGrade(int id = 0)
    {
        return new ConcreteGrade
        {
            Id = id,
            Mark = Mark,
            Class = Class,
            FrostResistanceType = FrostResistanceType,
            WaterproofType = WaterproofType,
            PricePerCubicMeter = PricePerCubicMeter
        };
    }
}