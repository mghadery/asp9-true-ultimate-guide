using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Section15_16_17.Practice.Web.Filters.Filters;

public class HitActionFilter(ILogger<HitActionFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        logger.LogInformation("HitActionFilter before");
        await next();
        logger.LogInformation("HitActionFilter after");
    }
}
