using Section15.Practice.Entities;
using Section15.Practice.ServiceContracts.DTOs;
using Section15.Practice.ServiceContracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace Section15.Practice.Services;

public class PersonsService : IPersonsService
{
    private readonly ICountriesService _countriesService;
    private readonly List<Person> _persons = new List<Person>();

    public PersonsService(ICountriesService countriesService, bool init = true)
    {
        _countriesService = countriesService;
        if (init)
        {
            _persons.AddRange(new List<Person>()
            {
                new(){PersonId=Guid.Parse("97f04cad-d109-4272-aebf-32b67ad4857d"),PersonName="Adena",Email="asteely0@guardian.co.uk",DateOfBirth=DateTime.Parse("1970-02-28"),Gender="Female",Address="0 Summit Pass",ReceiveNewsLetters=false, CountryId=Guid.Parse("1d0ab2da-19ca-474d-a6c9-6ed136ec4b8a")},

                new(){PersonId=Guid.Parse("5bdf8265-06cf-4d90-81e1-54f2f0cf75b7"),PersonName="Xenos",Email="xfranschini1@angelfire.com",DateOfBirth=DateTime.Parse("1984-06-01"),Gender="Male",Address="136 Badeau Court",ReceiveNewsLetters=true, CountryId=Guid.Parse("1f1c5d35-0d05-46f0-84e2-040375c59a86")},

                new(){PersonId=Guid.Parse("d56be199-0ee1-4296-82fc-d8e15e68b72e"),PersonName="Nester",Email="nbernardy2@skyrock.com",DateOfBirth=DateTime.Parse("1977-12-20"),Gender="Male",Address="177 Dixon Terrace",ReceiveNewsLetters=false, CountryId = Guid.Parse("d358077f-bcce-408b-8c47-675fec6d4ab7")},

                new(){PersonId=Guid.Parse("29bedca3-ba94-47c4-86b7-55bdef205342"),PersonName="Sheba",Email="schaston3@trellian.com",DateOfBirth=DateTime.Parse("1975-01-21"),Gender="Female",Address="5 Farmco Circle",ReceiveNewsLetters=true, CountryId=Guid.Parse("1f1c5d35-0d05-46f0-84e2-040375c59a86")},

                new(){PersonId=Guid.Parse("499058ea-bbd2-4ae3-8968-f5540582f6af"),PersonName="Addie",Email="asoppit4@amazon.co.jp",DateOfBirth=DateTime.Parse("1983-08-13"),Gender="Male",Address="42 Johnson Park",ReceiveNewsLetters=false, CountryId = Guid.Parse("1d0ab2da-19ca-474d-a6c9-6ed136ec4b8a")}
            });
        }
    }

    private void setCountryInResponse(PersonResponse personResponse)
    {
        personResponse.Country = _countriesService.GetCountry(personResponse.CountryId)?.CountryName;
    }

    private void setCountryInResponse(List<PersonResponse> personResponses)
    {
        foreach (var personResponse in personResponses)
        {
            personResponse.Country = _countriesService.GetCountry(personResponse.CountryId)?.CountryName;
        }
    }

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
        setCountryInResponse(personResponse);

        return personResponse;
    }

    public List<PersonResponse> GetPersonList()
    {
        var r = _persons.Select(x => (PersonResponse)x).ToList();
        setCountryInResponse(r);
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
            "Email" => persons.Where(x => x.Email != null && x.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList(),
            _ => persons
        };

        var peronResponses = persons.Select(x => (PersonResponse)x).ToList();
        return peronResponses;
    }

    public List<PersonResponse> GetSortedPersons(List<PersonResponse> persons, string sortBy, bool ascending)
    {
        var peronResponses = sortBy switch
        {
            "PersonName" => (ascending ? persons.OrderBy(x => x.PersonName, StringComparer.OrdinalIgnoreCase) :
            persons.OrderByDescending(x => x.PersonName, StringComparer.OrdinalIgnoreCase))
            .ToList(),
            "Email" => (ascending ? persons.OrderBy(x => x.Email, StringComparer.OrdinalIgnoreCase) :
            persons.OrderByDescending(x => x.Email, StringComparer.OrdinalIgnoreCase))
            .ToList(),
            "Address" => (ascending ? persons.OrderBy(x => x.Address, StringComparer.OrdinalIgnoreCase) :
            persons.OrderByDescending(x => x.Address, StringComparer.OrdinalIgnoreCase))
            .ToList(),
            "Country" => (ascending ? persons.OrderBy(x => x.Country, StringComparer.OrdinalIgnoreCase) :
            persons.OrderByDescending(x => x.Country, StringComparer.OrdinalIgnoreCase))
            .ToList(),
            "DateOfBirth" => (ascending ? persons.OrderBy(x => x.DateOfBirth) :
            persons.OrderByDescending(x => x.DateOfBirth))
            .ToList(),
            "Gender" => (ascending ? persons.OrderBy(x => x.Gender, StringComparer.OrdinalIgnoreCase) :
            persons.OrderByDescending(x => x.Gender, StringComparer.OrdinalIgnoreCase))
            .ToList(),
            "ReceiveNewsLetters" => (ascending ? persons.OrderBy(x => x.ReceiveNewsLetters) :
            persons.OrderByDescending(x => x.ReceiveNewsLetters))
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

        var person = _persons.FirstOrDefault(x => x.PersonId == request.PersonId);
        if (person is null) throw new ArgumentException(nameof(UpdatePersonRequest.PersonId));

        person.PersonName = request.PersonName;
        person.Email = request.Email;
        person.Address = request.Address;
        person.CountryId = request.CountryId;
        person.DateOfBirth = request.DateOfBirth;
        person.Gender = request.GenderOptions.ToString();
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