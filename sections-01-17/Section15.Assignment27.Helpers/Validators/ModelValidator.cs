using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Section15.Assignment27.Helpers.Validators;

public class ModelValidator
{
    public static void IsValid(object? obj)
    {
        if (obj == null) throw new ArgumentNullException();
        ValidationContext validationContext = new ValidationContext(obj);
        List<ValidationResult> validationResults = new();
        var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);
        if (isValid) return;
        string error = string.Join("\n", validationResults.Select(x => x.ErrorMessage));
        throw new ArgumentException(error);
    }
}
