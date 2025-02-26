using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using People.Entities;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Enums;
using People.ServiceContracts.Interfaces;
using People.Services;
using Xunit.Abstractions;
using FluentAssertions;

namespace People.Tests;

public class PersonsServiceTest
{
    private readonly ICountriesService _countriesService;

    private readonly IPersonsService _personsService;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Bogus.Faker<AddPersonRequest> _personReqFaker;
    private readonly Bogus.Faker<AddCountryRequest> _countryReqFaker;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        AppDbContext appDbContext = Create.MockedDbContextFor<AppDbContext>();
        _countriesService = new CountriesService(appDbContext);
        _personsService = new PersonsService(appDbContext);
        _testOutputHelper = testOutputHelper;
        _personReqFaker = new Bogus.Faker<AddPersonRequest>()
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.GenderOptions, f => f.PickRandom<GenderOptions>())
            .RuleFor(p => p.PersonName, f => f.Name.FullName());

        _countryReqFaker = new Bogus.Faker<AddCountryRequest>()
            .RuleFor(c => c.CountryName, f => f.Address.County());
    }

    #region AddPerson
    [Fact]
    public async Task AddPerson_NullRequest()
    {
        //Arrange
        AddPersonRequest? addPersonRequest = null;

        //Assert
        //await Assert.ThrowsAsync<ArgumentNullException>(
        //    //Act
        //    async () => await _personsService.AddPerson(addPersonRequest));
        Func<Task> action = async () => await _personsService.AddPerson(addPersonRequest);

        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task AddPerson_NullName()
    {
        //Arrange
        AddPersonRequest? addPersonRequest = new AddPersonRequest() { PersonName = null };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(
            //Act
            async () => await _personsService.AddPerson(addPersonRequest));
    }

    [Fact]
    public async Task AddPerson_ValidPerson()
    {
        //Arrange
        //AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        AddCountryRequest? addCountryRequest = _countryReqFaker.Generate();
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);

        //AddPersonRequest? addPersonRequest = new AddPersonRequest()
        //{
        //    PersonName = "Ali",
        //    Address = "Street 1",
        //    DateOfBirth = DateTime.Parse("2000-01-01"),
        //    Email = "ali@gmail.com",
        //    GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
        //    ReceiveNewsLetters = true,
        //    CountryId = countryResponse.CountryId
        //};

        //_personReqFaker.RuleFor(x => x.CountryId, countryResponse.CountryId);
        AddPersonRequest? addPersonRequest = _personReqFaker.Generate();
        addPersonRequest.CountryId = countryResponse.CountryId;
        //Act
        var actualResponse = await _personsService.AddPerson(addPersonRequest);
        var list = await _personsService.GetPersonList();

        //Assert
        //Assert.True(actualResponse.PersonId != Guid.Empty);
        actualResponse.PersonId.Should().NotBe(Guid.Empty);

        //Assert.Contains(actualResponse, list);
        list.Should().Contain(actualResponse);
    }
    #endregion

    #region GetPersonList
    [Fact]
    public async Task GetPersonList_Valid()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);

        AddPersonRequest addPersonRequest1 = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        //Arrange
        AddPersonRequest addPersonRequest2 = new AddPersonRequest()
        {
            PersonName = "Hasan",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hasan@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        List<AddPersonRequest> addPersonRequests = new() { addPersonRequest1, addPersonRequest2 };
        List<PersonResponse> personResponses = new();
        foreach (var item in addPersonRequests)
        {
            personResponses.Add(await _personsService.AddPerson(item));
        }

        //Action
        var actual = await _personsService.GetPersonList();


        //Assert
        Assert.True(personResponses.SequenceEqual(actual), "Actual and expected arrays are not equal");
    }
    #endregion

    #region GetPersonById
    [Fact]
    public async Task GetPersonById_Valid()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);

        AddPersonRequest addPersonRequest = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        var expected = await _personsService.AddPerson(addPersonRequest);

        //Act
        var actual = await _personsService.GetPersonById(expected.PersonId);
        //expected.Country = "qaz";

        //Assert
        Assert.Equal(expected, actual);
    }
    #endregion

    #region GetFilteredPersons
    [Fact]
    public async Task GetFilteredPersons_Empty()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);
        AddPersonRequest addPersonRequest1 = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        //Arrange
        AddPersonRequest addPersonRequest2 = new AddPersonRequest()
        {
            PersonName = "Hasan",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hasan@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        List<AddPersonRequest> addPersonRequests = new() { addPersonRequest1, addPersonRequest2 };
        List<PersonResponse> personResponses = new();
        foreach (var item in addPersonRequests)
        {
            personResponses.Add(await _personsService.AddPerson(item));
        }

        //Action
        var actual = await _personsService.GetFilteredPersons(nameof(AddPersonRequest.PersonName), "");


        //Assert
        Assert.True(personResponses.SequenceEqual(actual), "Actual and expected arrays are not equal");
    }

    [Fact]
    public async Task GetFilteredPersons_SearchName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);
        AddPersonRequest addPersonRequest1 = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        AddPersonRequest addPersonRequest2 = new AddPersonRequest()
        {
            PersonName = "Hosein",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hosen@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        AddPersonRequest addPersonRequest3 = new AddPersonRequest()
        {
            PersonName = "Hasanali",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hasanali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        List<AddPersonRequest> addPersonRequests = new() { addPersonRequest1, addPersonRequest2, addPersonRequest3 };
        List<PersonResponse> personResponses = new();
        foreach (var item in addPersonRequests)
        {
            var personResponse = await _personsService.AddPerson(item);
            personResponses.Add(personResponse);
        }

        string searchString = "ali";
        var expected = personResponses.Where(x => x.PersonName != null && x.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        _testOutputHelper.WriteLine("Expected:");
        foreach (var item in expected)
            _testOutputHelper.WriteLine(item.ToString());

        //Action
        var actual = await _personsService.GetFilteredPersons(nameof(AddPersonRequest.PersonName), searchString);
        _testOutputHelper.WriteLine("Actual:");
        foreach (var item in actual)
            _testOutputHelper.WriteLine(item.ToString());

        //Assert
        //foreach (var item in expected)
        //{
        //    Assert.Contains(item, actual);
        //}
        actual.Should().BeEquivalentTo(expected);
        actual.Should().OnlyContain(x => x.PersonName.Contains(searchString,
            StringComparison.OrdinalIgnoreCase));
    }
    #endregion

    #region GetSortedPersons
    [Fact]
    public async Task GetSortedPersons_Valid()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);

        AddPersonRequest addPersonRequest1 = new AddPersonRequest()
        {
            PersonName = "Hosein",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hosen@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        AddPersonRequest addPersonRequest2 = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        AddPersonRequest addPersonRequest3 = new AddPersonRequest()
        {
            PersonName = "Hasanali",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hasanali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        List<AddPersonRequest> addPersonRequests = new() { addPersonRequest1, addPersonRequest2, addPersonRequest3 };
        List<PersonResponse> personResponses = new();
        foreach (var item in addPersonRequests)
        {
            var personResponse = await _personsService.AddPerson(item);
            personResponses.Add(personResponse);
        }

        _testOutputHelper.WriteLine("Expected:");
        var expected = personResponses.OrderBy(x => x.PersonName, StringComparer.OrdinalIgnoreCase).ToList();
        foreach (var item in expected)
            _testOutputHelper.WriteLine(item.ToString());

        //Action
        var actual = _personsService.GetSortedPersons(personResponses, nameof(AddPersonRequest.PersonName), true);
        _testOutputHelper.WriteLine("Actual:");
        foreach (var item in actual)
            _testOutputHelper.WriteLine(item.ToString());

        //Assert
        //Assert.True(expected.SequenceEqual(actual), "Actual and expected arrays are not equal");
        actual.Should().BeEquivalentTo(personResponses);  //having the same elements
        actual.Should().BeInAscendingOrder(x => x.PersonName);   //but with the expected order
    }
    #endregion

    #region UpdatePerson
    [Fact]
    public async Task UpdatePerson_NullRequest()
    {
        //Arrange
        UpdatePersonRequest? personRequest = null;

        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            //Act
            await _personsService.UpdatePerson(personRequest)
        );
    }

    [Fact]
    public async Task UpdatePerson_InvalidRequest()
    {
        //Arrange
        UpdatePersonRequest? personRequest = new UpdatePersonRequest() { PersonId = Guid.NewGuid() };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act
            await _personsService.UpdatePerson(personRequest)
        );
    }

    [Fact]
    public async Task UpdatePerson_NullName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);
        AddPersonRequest? addPersonRequest = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        var addResponse = await _personsService.AddPerson(addPersonRequest);

        UpdatePersonRequest? personRequest = (UpdatePersonRequest)addResponse;
        personRequest.PersonName = null;

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            //Act
            await _personsService.UpdatePerson(personRequest)
        );
    }

    [Fact]
    public async Task UpdatePerson_Update()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);
        AddPersonRequest? addPersonRequest = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        var addResponse = await _personsService.AddPerson(addPersonRequest);

        UpdatePersonRequest? personRequest = (UpdatePersonRequest)addResponse;
        personRequest.PersonName = "Michele";

        //Act
        var updatedResponse =   _personsService.UpdatePerson(personRequest);

        var expected = personRequest;
        var getResponse =  await _personsService.GetPersonById(personRequest.PersonId);
        var actual = (UpdatePersonRequest)getResponse;

        //Assert
        Assert.Equal(expected, actual);
    }
    #endregion

    #region DeletePerson
    [Fact]
    public async Task DeletePerson_Invalid()
    {
        var r = await _personsService.DeletePerson(Guid.NewGuid());

        Assert.False(r);
    }

    [Fact]
    public async Task DeletePerson_valid()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);
        AddPersonRequest? addPersonRequest = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = countryResponse.CountryId
        };
        var addResponse = await _personsService.AddPerson(addPersonRequest);

        //Act
        var r = await _personsService.DeletePerson(addResponse.PersonId);

        var newList = await _personsService.GetPersonList();

        //Assert
        Assert.True(r);
        Assert.DoesNotContain(addResponse, newList);
    }
    #endregion
}
