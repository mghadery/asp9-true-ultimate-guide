using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Stocks.ServiceContracts.DTOs;
using Stocks.ServiceContracts.Interfaces;

namespace Stocks.Web.Filters;

public class ValidateRequestFilter(ILogger<ValidateRequestFilter> logger) : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        var controllerName = context.ActionDescriptor.RouteValues["controller"];
        var actionName = context.ActionDescriptor.RouteValues["action"];
        var filterName = this.GetType().Name;
        logger.LogInformation("{controllerName}-{actionName}-{filterName}-{method}", controllerName, actionName, filterName, "OnActionExecuted");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controllerName = context.ActionDescriptor.RouteValues["controller"];
        var actionName = context.ActionDescriptor.RouteValues["action"];
        var filterName = this.GetType().Name;
        var request = context.ActionArguments["request"] as OrderRequest;

        var ModelState = ((Controller)context.Controller).ModelState;
        ModelState.Remove("DateAndTimeOfOrder");
        if (!ModelState.IsValid)
        {
            context.Result = new RedirectToActionResult("index", "trade", new { stockSymbol = request?.StockSymbol });
            logger.LogInformation("{controllerName}-{actionName}-{filterName}-{method}-{result}", controllerName, actionName, filterName, "OnActionExecuting", "failure");
        }
        else
        {
            request.DateAndTimeOfOrder = DateTime.Now;
            logger.LogInformation("{controllerName}-{actionName}-{filterName}-{method}-{result}", controllerName, actionName, filterName, "OnActionExecuting", "success");
        }
    }
}
