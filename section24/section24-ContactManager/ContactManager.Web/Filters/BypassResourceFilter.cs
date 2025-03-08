using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ContactManager.Web.Filters;

public class BypassResourceFilter(ILogger<BypassResourceFilter> logger) : IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        //won't be executed!
        logger.LogInformation("ByPassResFilter executed");
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        logger.LogInformation("ByPassResFilter executing");
        context.Result = new ContentResult() { Content = "This is ByPassResFilter" };
    }
}
