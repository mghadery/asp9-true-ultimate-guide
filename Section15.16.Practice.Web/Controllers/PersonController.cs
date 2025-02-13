using Microsoft.AspNetCore.Mvc;
using Section15.Practice.ServiceContracts.DTOs;
using Section15.Practice.ServiceContracts.Interfaces;

namespace Section15_16.Practice.Web.Controllers;

public class PersonController(IPersonsService personsService, ICountriesService countriesService) : Controller
{
    [Route("/")]
    [Route("/persons/index")]
    [HttpGet]
    public IActionResult Index(string searchBy, string searchString, string sortBy, bool ascending)
    {
        var persons = personsService.GetFilteredPersons(searchBy, searchString);
        persons = personsService.GetSortedPersons(persons, sortBy, ascending);

        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            [nameof(PersonResponse.PersonName)] = "Name",
            [nameof(PersonResponse.Email)] = "Email",
        };
        ViewBag.SearchBy = searchBy;
        ViewBag.SearchString = searchString;
        ViewBag.SortBy = sortBy;
        ViewBag.Ascending = ascending;
        return View(persons);
    }

    [Route("/persons/create")]
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Countries = countriesService.GetCountryList();
        return View();
    }

    [Route("/persons/create")]
    [HttpPost]
    public IActionResult Create(AddPersonRequest addPersonRequest)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            ViewBag.Countries = countriesService.GetCountryList();
            return View();
        }

        personsService.AddPerson(addPersonRequest);

        return RedirectToAction("index", "person");
    }
}

