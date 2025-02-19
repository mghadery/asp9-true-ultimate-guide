using Microsoft.AspNetCore.Mvc;

namespace Section09.Practice.Controllers;

public class ProductsController: Controller
{
    [Route("/products")]
    public IActionResult Index()
    {
        return View();
    }
    [Route("/search-products")]
    public IActionResult Search()
    {
        return View();
    }
    [Route("/order-product")]
    public IActionResult Order()
    {
        return View();
    }
}
