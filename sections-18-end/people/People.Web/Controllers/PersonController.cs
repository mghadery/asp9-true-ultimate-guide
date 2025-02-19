using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Interfaces;

namespace People.Web.Controllers;

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

    [Route("/persons/edit/{personId}")]
    [HttpGet]
    public IActionResult Edit(Guid personId)
    {
        ViewBag.Countries = countriesService.GetCountryList()
            .Select(x => new SelectListItem() { Text = x.CountryName, Value = x.CountryId.ToString() });

        var pr = (UpdatePersonRequest)personsService.GetPersonById(personId);

        return View(pr);

    }

    [Route("/persons/edit")]
    [HttpPost]
    public IActionResult Edit(UpdatePersonRequest updatePersonRequest)
    {
        ViewBag.Countries = countriesService.GetCountryList()
            .Select(x => new SelectListItem() { Text = x.CountryName, Value = x.CountryId.ToString() });

        if (!ModelState.IsValid)
            return View(updatePersonRequest);

        personsService.UpdatePerson(updatePersonRequest);

        return RedirectToAction("index");

    }

}

