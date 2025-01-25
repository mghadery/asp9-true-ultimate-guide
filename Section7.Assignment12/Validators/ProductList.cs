using Section7.Assignment12.Models;
using System.ComponentModel.DataAnnotations;

namespace Section7.Assignment12.Validators;

public class ProductList(int minSize) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return null;
        List<Product>? products = value as List<Product>;
        if (products == null) return null;

        if (products.Count >= minSize) return ValidationResult.Success;
        string error = string.Format(ErrorMessage ?? "", validationContext.DisplayName, minSize);
        return new ValidationResult(error);
    }
}
