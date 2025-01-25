using Microsoft.AspNetCore.Mvc;
using Section7.Assignment12.Models;

namespace Section7.Assignment12.Controllers;

public class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return Ok("Welcome");
    }

    [HttpPost("/order")]
    public IActionResult order([Bind(nameof(Order.Products), nameof(Order.InvoicePrice), nameof(Order.OrderDate))][FromForm]Order order)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(x=>x.Errors).Select(x=>x.ErrorMessage).ToList();
            var error = string.Join("\n", errors);
            return new ContentResult() { Content = error, ContentType = "plain/text", StatusCode = 400 };
        }

        var result = new Random().Next(1, 100000);
        return Json(new { OrderNo = result });
    }
}
