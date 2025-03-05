using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Section15_16_17.Practice.Web.Filters.Filters;

public class BypassAuthFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        context.Result = new ContentResult() { Content = "This is BypassAuthFilter filter", StatusCode = StatusCodes.Status401Unauthorized };
        await Task.CompletedTask;
    }
}
