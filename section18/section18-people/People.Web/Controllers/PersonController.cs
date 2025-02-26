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
    public async Task<IActionResult> Index(string searchBy, string searchString, string sortBy, bool ascending)
    {
        var persons = await personsService.GetFilteredPersons(searchBy, searchString);
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
    public async Task<IActionResult> Create()
    {
        ViewBag.Countries = await countriesService.GetCountryList();
        return View();
    }

    [Route("/persons/create")]
    [HttpPost]
    public async Task<IActionResult> Create(AddPersonRequest addPersonRequest)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            ViewBag.Countries = await countriesService.GetCountryList();
            return View();
        }

        await personsService.AddPerson(addPersonRequest);

        return RedirectToAction("index", "person");
    }

    [Route("/persons/edit/{personId}")]
    [HttpGet]
    public async Task<IActionResult> Edit(Guid personId)
    {
        ViewBag.Countries = (await countriesService.GetCountryList())
            .Select(x => new SelectListItem() { Text = x.CountryName, Value = x.CountryId.ToString() });

        var pr = (UpdatePersonRequest)(await personsService.GetPersonById(personId));

        return View(pr);

    }

    [Route("/persons/edit")]
    [HttpPost]
    public async Task<IActionResult> Edit(UpdatePersonRequest updatePersonRequest)
    {
        ViewBag.Countries = (await countriesService.GetCountryList())
            .Select(x => new SelectListItem() { Text = x.CountryName, Value = x.CountryId.ToString() });

        if (!ModelState.IsValid)
            return View(updatePersonRequest);

        await personsService.UpdatePerson(updatePersonRequest);

        return RedirectToAction("index");

    }

}

