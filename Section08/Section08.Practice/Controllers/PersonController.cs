using Microsoft.AspNetCore.Mvc;
using Section08.Practice.Models;

namespace Section08.Practice.Controllers
{
    public class PersonController : Controller
    {
        private List<Person> persons = new() {
            new Person(){ Name="John", Title="Mr", DateOfBirth=Convert.ToDateTime( "1980-01-01")},
            new Person(){ Name="Mary", Title="Ms", DateOfBirth=Convert.ToDateTime( "1990-01-01")},
        };
        [Route("Person/")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Persons List";
            return View(persons);
        }
        [Route("Person/details/{name}")]
        public IActionResult details(string name)
        {
            Person? person = persons.FirstOrDefault(x => x.Name == name);
            //ViewBag.Person = person;
            ViewData["person"] = person;
            return View();
        }
    }
}
