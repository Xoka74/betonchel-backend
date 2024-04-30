using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class UserConcreteGrade : IValidatableObject
{
    [Required] 
    public string Mark { get; set; }
    [Required] 
    public string Class { get; set; }
    [Required] 
    public string WaterproofType { get; set; }
    [Required]
    public string FrostResistanceType { get; set; }
    [Required] 
    public double PricePerCubicMeter { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PricePerCubicMeter < 0)
            yield return new ValidationResult(
                $"Price for cubic meter of concrete should be positive, but given {PricePerCubicMeter}",
                new[] { nameof(PricePerCubicMeter) }
            );

        if (!Mark.StartsWith("m", StringComparison.InvariantCultureIgnoreCase) || Mark.Length > 10)
            yield return new ValidationResult(
                $"Mark of concrete should start with 'M' and have maximum length 10, but given {Mark}",
                new[] { nameof(Mark) }
            );

        if (!Class.StartsWith("b", StringComparison.InvariantCultureIgnoreCase) || Class.Length > 10)
            yield return new ValidationResult(
                $"Class of concrete should start with 'B' and have maximum length 10, but given {Class}",
                new[] { nameof(Class) }
            );

        if (!FrostResistanceType.StartsWith("f", StringComparison.InvariantCultureIgnoreCase) ||
            FrostResistanceType.Length > 10)
            yield return new ValidationResult(
                $"Frost resistance type of concrete should start with 'F' and have maximum length 10, but given {FrostResistanceType}",
                new[] { nameof(FrostResistanceType) }
            );

        if (!WaterproofType.StartsWith("w", StringComparison.InvariantCultureIgnoreCase) ||
            WaterproofType.Length > 10)
            yield return new ValidationResult(
                $"Waterproof type of concrete should start with 'W' and have maximum length 10, but given {WaterproofType}",
                new[] { nameof(WaterproofType) }
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