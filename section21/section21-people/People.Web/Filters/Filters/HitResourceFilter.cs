using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Section15_16_17.Practice.Web.Filters.Filters;

public class HitResourceFilter(ILogger<HitResourceFilter> logger) : IResourceFilter
{
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
        //won't be executed!
        logger.LogInformation("HitResourceFilter executed");
    }

    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        logger.LogInformation("HitResourceFilter executing");
    }
}
