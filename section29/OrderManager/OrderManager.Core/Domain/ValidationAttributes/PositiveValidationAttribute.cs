using System.ComponentModel.DataAnnotations;

namespace OrderManager.Core.Domain.ValidationAttributes;

public class PositiveValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is  null)
            return new ValidationResult(ErrorMessage?? "Value cannot be null");
        var ok = ValidationResult.Success;
        var error = new ValidationResult(ErrorMessage?? "Value must be positive");
        return value switch
        {
            int intVal when intVal > 0 => ok,
            float floatVal when floatVal > 0 => ok,
            double doubleVal when doubleVal > 0 => ok,
            decimal decimalVal when decimalVal > 0 => ok,
            _ => error
        };
    }
}
