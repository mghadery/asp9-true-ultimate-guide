using Microsoft.AspNetCore.Mvc;
using Serilog;
using SerilogTimings;

namespace section20_logging.Controllers;

public class HomeController(ILogger<HomeController> logger, IDiagnosticContext diagnosticContext):Controller
{
    [Route("/")]
    public async Task<IActionResult> Index()
    {
        int myVal = 123;
        diagnosticContext.Set("mykey", myVal);

        using (Operation.Time("HomeIndexTime"))
        {
            await Task.Delay(1000);
        }

        logger.LogInformation($"{ControllerContext.ActionDescriptor.ControllerName}-{ControllerContext.ActionDescriptor.ActionName}");
        return View();
    }
}
