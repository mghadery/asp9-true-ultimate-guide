using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Section15_16_17.Practice.Web.Filters.Filters;

public class HitResultFilter(ILogger<HitResultFilter> logger) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        logger.LogInformation("HitResultFilter before");
        await next();
        logger.LogInformation("HitResultFilter after");
    }
}
