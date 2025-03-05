using Microsoft.AspNetCore.Mvc.Filters;

namespace Section15_16_17.Practice.Web.Filters.Filters;

public class CreateFilterFactoryAttribute : Attribute, IFilterFactory, IOrderedFilter
{
    private string _tag;
    public CreateFilterFactoryAttribute(int order, string tag)
    {
        Order = order;
        _tag = tag;
    }
    public bool IsReusable => false;

    public int Order { get; private set; }

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetService<ILogger<CreateFilter>>();
        var f = new CreateFilter(logger) { Tag = _tag };
        return f;
    }
}
public class CreateFilter : IAsyncActionFilter
{
    ILogger<CreateFilter> _logger;
    public CreateFilter(ILogger<CreateFilter> logger)
    {
        _logger = logger;
    }
    public string? Tag { get; set; }


    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("{Tag}", Tag);
        await next();
    }
}
