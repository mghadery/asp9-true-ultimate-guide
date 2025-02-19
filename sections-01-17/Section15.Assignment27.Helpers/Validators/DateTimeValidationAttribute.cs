using System.ComponentModel.DataAnnotations;

namespace Section15.Assignment27.Helpers.Validators;

public class DateTimeValidationAttribute(string minDateTimeStr) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var minDateTime = DateTime.Parse(minDateTimeStr);
        if (value is null) return ValidationResult.Success;
        var dateTime = (DateTime)value;
        var parName = validationContext.DisplayName;
        var errorMessage = string.Format(ErrorMessage ?? "", parName, minDateTimeStr);
        return dateTime >= minDateTime ? ValidationResult.Success :
            new ValidationResult(errorMessage);
    }
}
