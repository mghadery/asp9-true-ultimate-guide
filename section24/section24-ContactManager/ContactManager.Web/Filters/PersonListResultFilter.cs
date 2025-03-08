using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.Web.Filters;

public class PersonListResultFilter : ResultFilterAttribute
{
    string _headerKey;
    string _headerVal;
    bool _bypass;
    public PersonListResultFilter(int order, string headerKey, string headerVal, bool bypass)
    {
        _headerKey = headerKey;
        _headerVal = headerVal;
        _bypass = bypass;
        Order = order;
    }
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        context.HttpContext.Response.Headers[_headerKey] = _headerVal;
        if (!_bypass)
            await next();
        // context.HttpContext.Response.Headers[_headerKey + "1"] = _headerVal + "1";

    }
}
