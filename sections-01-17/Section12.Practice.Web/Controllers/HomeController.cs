using Autofac;
using Microsoft.AspNetCore.Mvc;
using Section12.Practice.IService;
using Section12.Practice.Service;
using System.ComponentModel;

namespace Section12.Practice.Controllers;

public class HomeController : Controller
{
    private readonly ICitiesService _citiesService;
    public HomeController(ICitiesService citiesService)
    {
        _citiesService = citiesService;
    }

    [Route("/")]
    public IActionResult Index()
    {
        var cities = _citiesService.GetCityNames();
        return View(cities);
    }

    [Route("/test1")]
    public IActionResult test1([FromServices] ICitiesService citiesService)
    {
        var cities = citiesService.GetCityNames();
        return View("index", cities);
    }

    [Route("/test2")]
    public IActionResult test2([FromServices] IServiceScopeFactory serviceScopeFactory)
    {
        IEnumerable<string>? cities1 = null;
        IEnumerable<string>? cities2 = null;

        using (var scope = serviceScopeFactory.CreateScope())
        {
            var citiesService1 = scope.ServiceProvider.GetService<ICitiesService>();
            if (citiesService1 is null) return StatusCode(500);
            cities1 = citiesService1.GetCityNames();
        }
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var citiesService2 = scope.ServiceProvider.GetService<ICitiesService>();
            if (citiesService2 is null) return StatusCode(500);
            cities2 = citiesService2.GetCityNames();
        }
        var cities = cities1?.Concat(cities2).ToList();
        return View("index", cities);
    }

    [Route("/test3")]
    public IActionResult test3([FromServices] ILifetimeScope lifetimeScope)
    {
        using (var scope = lifetimeScope.BeginLifetimeScope())
        {
            var citiesService2 = scope.Resolve<ICitiesService>();
            if (citiesService2 is null) return StatusCode(500);
            var cities2 = citiesService2.GetCityNames();
            return View("index", cities2);
        }
    }
}
