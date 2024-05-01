using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class UserConcreteGrade
{
    [Required]
    [StringLength(10, ErrorMessage = "TooLong")]
    [StringStartsWith("M", ErrorMessage = "ShouldStartWithM")]
    public string Mark { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "TooLong")]
    [StringStartsWith("B", ErrorMessage = "ShouldStartWithB")]
    public string Class { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "TooLong")]
    [StringStartsWith("W", ErrorMessage = "ShouldStartWithW")]
    public string WaterproofType { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "TooLong")]
    [StringStartsWith("F", ErrorMessage = "ShouldStartWithF")]
    public string FrostResistanceType { get; set; }


    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "OutOfRange")]
    public double PricePerCubicMeter { get; set; }

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