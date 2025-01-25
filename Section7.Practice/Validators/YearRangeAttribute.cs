using System.ComponentModel.DataAnnotations;

namespace Section7.Practice.Validators
{
    public class YearRangeAttribute(int minYear, int maxYear) : ValidationAttribute
    {
        public YearRangeAttribute() : this(1950, 2000) { }
        private const string DefaultErrorMessage = "Year not in the range";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            DateTime.TryParse(value?.ToString(), out DateTime val);
            if (val.Year < minYear || val.Year > maxYear)
            {
                string? error =
                    string.Format(ErrorMessage ?? DefaultErrorMessage, validationContext.DisplayName, minYear, maxYear);
                return new ValidationResult(error);
            }
            return ValidationResult.Success;
        }
    }
}
