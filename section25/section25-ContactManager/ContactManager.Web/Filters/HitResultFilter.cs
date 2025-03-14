using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.Web.Filters;

public class HitResultFilter(ILogger<HitResultFilter> logger) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        logger.LogInformation("HitResultFilter before");
        await next();
        logger.LogInformation("HitResultFilter after");
    }
}
