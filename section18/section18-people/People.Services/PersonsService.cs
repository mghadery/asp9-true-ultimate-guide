using Microsoft.EntityFrameworkCore;
using People.Entities;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace People.Services;

public class PersonsService(AppDbContext dbContext) : IPersonsService
{
    //private readonly ICountriesService _countriesService;
    //private readonly List<Person> _persons = new List<Person>();

    //private void setCountryInResponse(PersonResponse personResponse)
    //{
    //    personResponse.Country = _countriesService.GetCountry(personResponse.CountryId)?.CountryName;
    //}

    //private void setCountryInResponse(List<PersonResponse> personResponses)
    //{
    //    foreach (var personResponse in personResponses)
    //    {
    //        personResponse.Country = _countriesService.GetCountry(personResponse.CountryId)?.CountryName;
    //    }
    //}

    public async Task<PersonResponse> AddPerson(AddPersonRequest request)
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
        await dbContext.Persons.AddAsync(person);
        await dbContext.SaveChangesAsync();
        await dbContext.Entry(person).Reference(p=> p.Country).LoadAsync();

        PersonResponse personResponse = (PersonResponse)person;

        return personResponse;
    }

    public async Task<List<PersonResponse>> GetPersonList()
    {
        var r = await dbContext.Persons.Include(p => p.Country)
            .Select(x => (PersonResponse)x).ToListAsync();

        return r;
    }

    public async Task<PersonResponse?> GetPersonById(Guid? id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));

        var r = await dbContext.Persons.FirstOrDefaultAsync(x => x.PersonId == id);
        var rs = (PersonResponse?)r;
        return rs;
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string searchString)
    {
        var persons = await GetPersonList();
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
    public async Task<PersonResponse> UpdatePerson(UpdatePersonRequest? request)
    {
        if (request is null) throw new ArgumentNullException(nameof(UpdatePersonRequest));

        //Validation
        ValidationContext validationContext = new ValidationContext(request);
        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
        if (!isValid)
            throw new ArgumentException(validationResults[0].ErrorMessage);

        var person = await dbContext.Persons.FirstOrDefaultAsync(x => x.PersonId == request.PersonId);
        if (person is null) throw new ArgumentException(nameof(UpdatePersonRequest.PersonId));

        person.PersonName = request.PersonName;
        person.Email = request.Email;
        person.Address = request.Address;
        person.CountryId = request.CountryId.Value;
        person.DateOfBirth = request.DateOfBirth;
        person.Gender = request.GenderOptions.ToString();
        person.ReceiveNewsLetters = request.ReceiveNewsLetters;

        await dbContext.SaveChangesAsync();
        await dbContext.Entry(person).Reference(p=>p.Country).LoadAsync();

        return (PersonResponse)person;
    }

    public async Task<bool> DeletePerson(Guid? personId)
    {
        if (personId is null) throw new ArgumentNullException(nameof(personId));

        var r = dbContext.Persons.FirstOrDefault(x => x.PersonId == personId);

        if (r is null) return false;

        dbContext.Persons.Remove(r);

        await dbContext.SaveChangesAsync();
        return true;
    }
}