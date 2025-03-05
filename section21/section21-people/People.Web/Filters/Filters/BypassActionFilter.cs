using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Section15_16_17.Practice.Web.Filters.Filters;

public class BypassActionFilter(ILogger<BypassActionFilter> logger) : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //won't be executed!
        logger.LogInformation("BypassActionFilter executed");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.Result = new ContentResult() { Content = "This is BypassActionFilter filter", StatusCode = StatusCodes.Status401Unauthorized };
        logger.LogInformation("BypassActionFilter executing");
    }
}
