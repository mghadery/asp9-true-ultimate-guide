
using MinimalAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace MinimalAPI.Filters;

public class ProductValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var product = context.Arguments.OfType<Product>().FirstOrDefault();
        var vc = new ValidationContext(product);
        List<ValidationResult> errors = new List<ValidationResult>();
        if (!Validator.TryValidateObject(product, vc, errors, true))
            return Results.BadRequest(errors);

        var result = await next(context);
        return result;
    }
}
