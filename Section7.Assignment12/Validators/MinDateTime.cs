using System.ComponentModel.DataAnnotations;

namespace Section7.Assignment12.Validators;

public class MinDateTime(string minDateTimeVal) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        DateTime minDateTime = Convert.ToDateTime(minDateTimeVal);
        if (value is null) return null;
        DateTime dateTime = Convert.ToDateTime(value);
        if (dateTime >= minDateTime) return ValidationResult.Success;
        string error = string.Format(ErrorMessage ?? "", validationContext.DisplayName, minDateTime);
        return new ValidationResult(error);
    }
}
