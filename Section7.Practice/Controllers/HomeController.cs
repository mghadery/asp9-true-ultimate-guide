using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Section7.Practice.Binders;
using Section7.Practice.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Section7.Practice.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return Content("Welcome");
    }

    [Route("/create-book")]
    [HttpPost]
    public IActionResult CreateBook([FromQuery] Book book)
    {
        //the following does not work as the body can be read once unless its buffer is enabled
        //Since the model binder reads the body, it is now read as empty.
        //string body;
        //using (var strr = new StreamReader(Request.Body))
        //    body = await strr.ReadToEndAsync();
        return Json(book);
    }

    [HttpPost("/create-person1")]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult CreatePerson1([FromForm] Person person)
    {
        if (ModelState.IsValid)
            return Json(person);

        List<String> errors = new() { $"ErrorCount: {ModelState.ErrorCount}" };

        errors.AddRange(ModelState
    .SelectMany(x => x.Value.Errors.Select(y => x.Key + ":" + y.ErrorMessage))
    .ToList());

        //errors.AddRange(ModelState.Values
        //    .SelectMany(x => x.Errors.Select(y =>  y.ErrorMessage))
        //    .ToList());
        var error = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        return BadRequest(error);
    }

    [HttpPost("/create-person2")]
    [Consumes("multipart/form-data")]
    public IActionResult CreatePerson2([FromForm] Person person)
    {
        if (ModelState.IsValid)
            return Json(person);

        List<String> errors = new() { $"ErrorCount: {ModelState.ErrorCount}" };

        errors.AddRange(ModelState
    .SelectMany(x => x.Value.Errors.Select(y => x.Key + ":" + y.ErrorMessage))
    .ToList());

        //errors.AddRange(ModelState.Values
        //    .SelectMany(x => x.Errors.Select(y =>  y.ErrorMessage))
        //    .ToList());
        var error = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        return BadRequest(error);
    }

    [HttpPost("/create-person3")]
    public IActionResult CreatePerson3([FromBody] Person person)
    {
        if (ModelState.IsValid)
            return Json(person);

        List<String> errors = new() { $"ErrorCount: {ModelState.ErrorCount}" };

        errors.AddRange(ModelState
    .SelectMany(x => x.Value.Errors.Select(y => x.Key + ":" + y.ErrorMessage))
    .ToList());

        //errors.AddRange(ModelState.Values
        //    .SelectMany(x => x.Errors.Select(y =>  y.ErrorMessage))
        //    .ToList());
        var error = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        return BadRequest(error);
    }
    [HttpPost("/create-person4")]
    public Person CreatePerson4([FromBody][ModelBinder(BinderType = typeof(PersonBinder))] Person person)
    {
        if (ModelState.IsValid)
            return person;

        List<String> errors = new() { $"ErrorCount: {ModelState.ErrorCount}" };

        errors.AddRange(ModelState
    .SelectMany(x => x.Value.Errors.Select(y => x.Key + ":" + y.ErrorMessage))
    .ToList());

        //errors.AddRange(ModelState.Values
        //    .SelectMany(x => x.Errors.Select(y =>  y.ErrorMessage))
        //    .ToList());
        var error = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
        throw new Exception(error);
        //return BadRequest(error);
    }

}

public record Book(int Id, string Title, string Description);
