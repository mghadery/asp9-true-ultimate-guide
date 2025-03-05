using Microsoft.AspNetCore.Mvc.Filters;

namespace Section15_16_17.Practice.Web.Filters.Filters;

public class BypassAsyncResultFilter(ILogger<BypassAsyncResultFilter> logger) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        logger.LogInformation("BypassAsyncResultFilter execution");
        await Task.CompletedTask;
        //it bypasses everything before resource filter executed method
    }
}
