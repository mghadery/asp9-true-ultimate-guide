using ContactManager.Core.Domain.Entities;
using ContactManager.Core.Domain.RepositorieContracts;
using ContactManager.Core.DTOs;
using ContactManager.Core.ServiceContracts;
using CsvHelper;
using OfficeOpenXml;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;

namespace ContactManager.Core.Services;

public class PersonsService(IPersonRepository personRepository) : IPersonsService
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
        await personRepository.Add(person);

        PersonResponse personResponse = (PersonResponse)person;

        return personResponse;
    }

    public async Task<List<PersonResponse>> GetPersonList()
    {
        var r = (await personRepository.GetAll())
            .Select(p => (PersonResponse)p)
            .ToList();
        return r;
    }

    public async Task<PersonResponse?> GetPersonById(Guid? id)
    {
        if (id is null) throw new ArgumentNullException(nameof(id));

        var r = await personRepository.GetById(id.Value);
        if (r is null) return null;
        var rs = (PersonResponse)r;
        return rs;
    }

    public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchString))
            return await GetPersonList();

        var persons = searchBy switch
        {
            "PersonName" => await personRepository.Get(x => x.PersonName != null && x.PersonName.Contains(searchString)),
            "Email" => await personRepository.Get(x => x.Email != null && x.Email.Contains(searchString)),
            _ => await personRepository.GetAll()
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

        var person = await personRepository.GetById(request.PersonId);
        if (person is null) throw new ArgumentException(nameof(UpdatePersonRequest.PersonId));

        person.PersonName = request.PersonName;
        person.Email = request.Email;
        person.Address = request.Address;
        person.CountryId = request.CountryId.Value;
        person.DateOfBirth = request.DateOfBirth;
        person.Gender = request.GenderOptions.ToString();
        person.ReceiveNewsLetters = request.ReceiveNewsLetters;

        await personRepository.Update(person);

        return (PersonResponse)person;
    }

    public async Task<bool> DeletePerson(Guid? personId)
    {
        if (personId is null) throw new ArgumentNullException(nameof(personId));

        var person = await personRepository.GetById(personId.Value);

        if (person is null) return false;

        await personRepository.Delete(person);

        return true;
    }

    public async Task<MemoryStream> GetExcel()
    {
        var persons = await GetPersonList();

        MemoryStream memoryStream = new MemoryStream();
        using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
        {
            var sheet = excelPackage.Workbook.Worksheets.Add("S1");
            sheet.Cells["A1"].Value = "Name";
            sheet.Cells["B1"].Value = "Email";

            int i = 0;
            foreach (var person in persons)
            {
                sheet.Cells[2 + i, 1].Value = person.PersonName;
                sheet.Cells[2 + i++, 2].Value = person.Email;
            }
            excelPackage.Save();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    public async Task<MemoryStream> GetCsv()
    {
        var persons = await GetPersonList();

        MemoryStream memoryStream = new MemoryStream();

        //using(
        StreamWriter streamWriter = new StreamWriter(memoryStream);
        using (CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, true))
        {
            csvWriter.WriteHeader<PersonResponse>();
            await csvWriter.NextRecordAsync();
            await csvWriter.WriteRecordsAsync(persons);
        }

        memoryStream.Position = 0;
        return memoryStream;
    }
}