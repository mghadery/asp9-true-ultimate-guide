using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactManager.Web.Filters;

public class PersonsListActionFilter(ILogger<PersonsListActionFilter> logger, int order = 1, string tag = "default") : IActionFilter, IOrderedFilter
{
    public int Order => order;

    public void OnActionExecuted(ActionExecutedContext context)
    {
        logger.LogInformation("PersonsListActionFilter OnActionExecuted {tag}", tag);
        ((Controller)context.Controller).ViewBag.Test = "This is PersonsListActionFilter filter";
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //context.ActionArguments
        //context.HttpContext.Items
        logger.LogInformation("PersonsListActionFilter OnActionExecuting {tag}", tag);
    }
}
