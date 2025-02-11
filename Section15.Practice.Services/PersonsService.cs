using Section15.Practice.Entities;
using Section15.Practice.ServiceContracts.DTOs;
using Section15.Practice.ServiceContracts.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace Section15.Practice.Services;

public class PersonsService(ICountriesService countriesService) : IPersonsService
{
    private readonly List<Person> _persons = new List<Person>();

    public PersonResponse AddPerson(AddPersonRequest request)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        //Validation
        ValidationContext validationContext = new ValidationContext(request);
        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
        if (!isValid)
            throw new ArgumentException(validationResults[0].ErrorMessage);

        Person person = (Person)request;
        person.PersonId = Guid.NewGuid();
        _persons.Add(person);

        PersonResponse personResponse = (PersonResponse)person;
        personResponse.Country = countriesService.GetCountry(person.CountryId)?.CountryName;

        return personResponse;
    }

    public List<PersonResponse> GetPersonList()
    {
        var r = _persons.Select(x => (PersonResponse)x).ToList();
        return r;
    }

    public PersonResponse? GetPersonById(Guid? id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));

        var r = _persons.FirstOrDefault(x => x.PersonId == id);
        var rs = (PersonResponse?)r;
        return rs;
    }

    public List<PersonResponse> GetFilteredPersons(string searchBy, string searchString)
    {
        var persons = GetPersonList();
        if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchString))
            return persons;

        persons = searchBy switch
        {
            "PersonName" => persons.Where(x => x.PersonName != null && x.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
            _ => persons
        };

        var peronResponses = persons.Select(x=>(PersonResponse)x).ToList();
        return peronResponses;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, bool ascending)
    {
        var peronResponses = sortBy switch
        {
            "PersonName" => (ascending? persons.OrderBy(x => x.PersonName, StringComparer.OrdinalIgnoreCase): 
            persons.OrderByDescending(x => x.PersonName, StringComparer.OrdinalIgnoreCase))
            .ToList(),
            _ => persons
        };

        return peronResponses;
    }
    public PersonResponse UpdatePerson(UpdatePersonRequest? request)
    {
        if (request is null) throw new ArgumentNullException(nameof(UpdatePersonRequest));

        //Validation
        ValidationContext validationContext = new ValidationContext(request);
        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
        if (!isValid)
            throw new ArgumentException(validationResults[0].ErrorMessage);

        var person = _persons.FirstOrDefault(x=>x.PersonId==request.PersonId);
        if (person is null) throw new ArgumentException(nameof(UpdatePersonRequest.PersonId));

        person.Address = request.Address;
        person.CountryId = request.CountryId;
        person.DateOfBirth = request.DateOfBirth;
        person.Gender = request.GenderOptions.ToString();
        person.Email = request.Email;
        person.PersonName = request.PersonName;
        person.ReceiveNewsLetters = request.ReceiveNewsLetters;

        return (PersonResponse)person;
    }

    public bool DeletePerson(Guid? personId)
    {
        if (personId is null) throw new ArgumentNullException(nameof(personId));

        var r = _persons.FirstOrDefault(x => x.PersonId == personId);

        if (r is null) return false;

        _persons.Remove(r);
        return true;
    }
}