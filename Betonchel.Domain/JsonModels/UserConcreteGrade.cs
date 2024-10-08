﻿using System.ComponentModel.DataAnnotations;
using Betonchel.Domain.CustomAttributes;
using Betonchel.Domain.DBModels;

namespace Betonchel.Domain.JsonModels;

public class UserConcreteGrade
{
    [Required]
    [Range(0, 1, ErrorMessage = "NotExist")]
    public ConcreteGradeAttribute Name { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "TooLong")]
    [StringStartsWith("M", ErrorMessage = "ShouldStartWithM")]
    public string Mark { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "TooLong")]
    [StringStartsWith("B", ErrorMessage = "ShouldStartWithB")]
    public string Class { get; set; }

    [StringLength(10, ErrorMessage = "TooLong")]
    [StringStartsWith("W", ErrorMessage = "ShouldStartWithW")]
    public string? WaterproofType { get; set; }

    [StringLength(10, ErrorMessage = "TooLong")]
    [StringStartsWith("F", ErrorMessage = "ShouldStartWithF")]
    public string? FrostResistanceType { get; set; }


    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "OutOfRange")]
    public double PricePerCubicMeter { get; set; }

    public ConcreteGrade ToConcreteGrade(int id = 0)
    {
        return new ConcreteGrade
        {
            Id = id,
            Name = Name,
            Mark = Mark,
            Class = Class,
            FrostResistanceType = FrostResistanceType,
            WaterproofType = WaterproofType,
            PricePerCubicMeter = PricePerCubicMeter
        };
    }
}