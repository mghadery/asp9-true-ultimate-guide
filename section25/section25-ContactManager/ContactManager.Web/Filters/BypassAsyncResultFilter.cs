using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.Web.Filters;

public class BypassAsyncResultFilter(ILogger<BypassAsyncResultFilter> logger) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        logger.LogInformation("BypassAsyncResultFilter execution");
        await Task.CompletedTask;
        //it bypasses everything before resource filter executed method
    }
}
