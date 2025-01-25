using Section7.Assignment12.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Section7.Assignment12.Validators;

public class ListPriceCheck(string listPropertyName) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return null;
        double invoicePrice = Convert.ToDouble(value);

        PropertyInfo? propertyInfo = validationContext.ObjectType.GetProperty(listPropertyName);
        object? otherValue = propertyInfo?.GetValue(validationContext.ObjectInstance);
        if (otherValue is null) return null;

        var productList = otherValue as List<Product>;
        if (productList is null) return null;

        var listPrice = productList.Sum(x => x.Price * x.Quantity);
        if (listPrice == invoicePrice) return ValidationResult.Success;

        string error = string.Format(ErrorMessage ?? "", validationContext.DisplayName, listPropertyName);
        return new ValidationResult(error, new[] { validationContext.MemberName??"", listPropertyName });
    }
}
