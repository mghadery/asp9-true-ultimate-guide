using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Stocks.Web.Controllers;

public class HomeController : Controller
{
    [Route("/Error")]
    public IActionResult Error()
    {
        var exp = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
        ViewBag.Error = exp.GetBaseException().Message;
        return View();
    }
}
