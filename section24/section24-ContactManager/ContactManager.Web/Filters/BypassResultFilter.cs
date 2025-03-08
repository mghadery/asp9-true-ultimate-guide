using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.Web.Filters;

public class BypassResultFilter(ILogger<BypassResultFilter> logger) : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context)
    {
        //this executes! also the next result filters!
        logger.LogInformation("BypassResultFilter executed");
    }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        logger.LogInformation("BypassResultFilter executing");
        context.Result = new BadRequestResult();
    }
}
