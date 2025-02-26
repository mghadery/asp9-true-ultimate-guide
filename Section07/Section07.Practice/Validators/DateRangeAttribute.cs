using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Section07.Practice.Validators
{
    public class DateRangeAttribute(string OtherPropertyName):ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null) return null;

            //getting othet property value
            PropertyInfo? otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherPropertyName);

            object? otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance);
            if (otherValue is null) return null;

            //conversion
            var fromDate = Convert.ToDateTime(otherValue);
            var toDate = Convert.ToDateTime(value);

            if (fromDate > toDate)
                return new ValidationResult($"{OtherPropertyName} cannot be greater than {validationContext.MemberName}"
                    , new [] { OtherPropertyName, validationContext.MemberName ?? "" });
            return ValidationResult.Success;
        }
    }
}
