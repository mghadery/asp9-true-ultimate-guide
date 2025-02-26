using Section15.Practice.ServiceContracts.DTOs;
using Section15.Practice.ServiceContracts.Interfaces;
using Section15.Practice.Services;
using Xunit.Abstractions;

namespace Section15.Practice.Tests;

public class PersonsServiceTest
{
    private readonly ICountriesService _countriesService;

    private readonly IPersonsService _personsService;
    private readonly ITestOutputHelper _testOutputHelper;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        _countriesService = new CountriesService(false);
        _personsService = new PersonsService(_countriesService, false);
        _testOutputHelper = testOutputHelper;
    }

    #region AddPerson
    [Fact]
    public void AddPerson_NullRequest()
    {
        //Arrange
        AddPersonRequest? addPersonRequest = null;

        //Assert
        Assert.Throws<ArgumentNullException>(
            //Act
            () => _personsService.AddPerson(addPersonRequest));
    }

    [Fact]
    public void AddPerson_NullName()
    {
        //Arrange
        AddPersonRequest? addPersonRequest = new AddPersonRequest() { PersonName = null };

        //Assert
        Assert.Throws<ArgumentException>(
            //Act
            () => _personsService.AddPerson(addPersonRequest));
    }

    [Fact]
    public void AddPerson_ValidPerson()
    {
        //Arrange
        AddPersonRequest? addPersonRequest = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
        };

        //Act
        var actualResponse = _personsService.AddPerson(addPersonRequest);
        var list = _personsService.GetPersonList();

        //Assert
        Assert.True(actualResponse.PersonId != Guid.Empty);
        Assert.Contains(actualResponse, list);
    }
    #endregion

    #region GetPersonList
    [Fact]
    public void GetPersonList_Valid()
    {
        //Arrange
        AddPersonRequest addPersonRequest1 = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
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
            CountryId = Guid.NewGuid()
        };
        List<AddPersonRequest> addPersonRequests = new() { addPersonRequest1, addPersonRequest2 };
        List<PersonResponse> personResponses = new();
        foreach (var item in addPersonRequests)
        {
            personResponses.Add(_personsService.AddPerson(item));
        }

        //Action
        var actual = _personsService.GetPersonList();


        //Assert
        Assert.True(personResponses.SequenceEqual(actual), "Actual and expected arrays are not equal");
    }
    #endregion

    #region GetPersonById
    [Fact]
    public void GetPersonById_Valid()
    {
        //Arrange
        AddPersonRequest addPersonRequest = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
        };
        var expected = _personsService.AddPerson(addPersonRequest);

        //Act
        var actual = _personsService.GetPersonById(expected.PersonId);
        //expected.Country = "qaz";

        //Assert
        Assert.Equal(expected, actual);
    }
    #endregion

    #region GetFilteredPersons
    [Fact]
    public void GetFilteredPersons_Empty()
    {
        //Arrange
        AddPersonRequest addPersonRequest1 = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
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
            CountryId = Guid.NewGuid()
        };
        List<AddPersonRequest> addPersonRequests = new() { addPersonRequest1, addPersonRequest2 };
        List<PersonResponse> personResponses = new();
        foreach (var item in addPersonRequests)
        {
            personResponses.Add(_personsService.AddPerson(item));
        }

        //Action
        var actual = _personsService.GetFilteredPersons(nameof(AddPersonRequest.PersonName), "");


        //Assert
        Assert.True(personResponses.SequenceEqual(actual), "Actual and expected arrays are not equal");
    }

    [Fact]
    public void GetFilteredPersons_SearchName()
    {
        //Arrange
        AddPersonRequest addPersonRequest1 = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
        };
        AddPersonRequest addPersonRequest2 = new AddPersonRequest()
        {
            PersonName = "Hosein",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hosen@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
        };
        AddPersonRequest addPersonRequest3 = new AddPersonRequest()
        {
            PersonName = "Hasanali",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hasanali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
        };
        List<AddPersonRequest> addPersonRequests = new() { addPersonRequest1, addPersonRequest2, addPersonRequest3 };
        List<PersonResponse> personResponses = new();
        foreach (var item in addPersonRequests)
        {
            var personResponse = _personsService.AddPerson(item);
            personResponses.Add(personResponse);
        }

        string searchString = "ali";
        var expected = personResponses.Where(x => x.PersonName != null && x.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
        _testOutputHelper.WriteLine("Expected:");
        foreach (var item in expected)
            _testOutputHelper.WriteLine(item.ToString());

        //Action
        var actual = _personsService.GetFilteredPersons(nameof(AddPersonRequest.PersonName), searchString);
        _testOutputHelper.WriteLine("Actual:");
        foreach (var item in actual)
            _testOutputHelper.WriteLine(item.ToString());

        //Assert
        foreach (var item in expected)
        {
            Assert.Contains(item, actual);
        }
    }
    #endregion

    #region GetSortedPersons
    [Fact]
    public void GetSortedPersons_Valid()
    {
        //Arrange
        AddPersonRequest addPersonRequest1 = new AddPersonRequest()
        {
            PersonName = "Hosein",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hosen@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
        };
        AddPersonRequest addPersonRequest2 = new AddPersonRequest()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
        };
        AddPersonRequest addPersonRequest3 = new AddPersonRequest()
        {
            PersonName = "Hasanali",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hasanali@gmail.com",
            GenderOptions = ServiceContracts.Enums.GenderOptions.Male,
            ReceiveNewsLetters = true,
            CountryId = Guid.NewGuid()
        };
        List<AddPersonRequest> addPersonRequests = new() { addPersonRequest1, addPersonRequest2, addPersonRequest3 };
        List<PersonResponse> personResponses = new();
        foreach (var item in addPersonRequests)
        {
            var personResponse = _personsService.AddPerson(item);
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
        Assert.True(expected.SequenceEqual(actual), "Actual and expected arrays are not equal");
    }
    #endregion

    #region UpdatePerson
    [Fact]
    public void UpdatePerson_NullRequest()
    {
        //Arrange
        UpdatePersonRequest? personRequest = null;

        //Assert
        Assert.Throws<ArgumentNullException>(() =>
            //Act
            _personsService.UpdatePerson(personRequest)
        );
    }

    [Fact]
    public void UpdatePerson_InvalidRequest()
    {
        //Arrange
        UpdatePersonRequest? personRequest = new UpdatePersonRequest() { PersonId = Guid.NewGuid() };

        //Assert
        Assert.Throws<ArgumentException>(() =>
            //Act
            _personsService.UpdatePerson(personRequest)
        );
    }

    [Fact]
    public void UpdatePerson_NullName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = _countriesService.AddCountry(addCountryRequest);
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
        var addResponse = _personsService.AddPerson(addPersonRequest);

        UpdatePersonRequest? personRequest = (UpdatePersonRequest)addResponse;
        personRequest.PersonName = null;

        //Assert
        Assert.Throws<ArgumentException>(() =>
            //Act
            _personsService.UpdatePerson(personRequest)
        );
    }

    [Fact]
    public void UpdatePerson_Update()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = _countriesService.AddCountry(addCountryRequest);
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
        var addResponse = _personsService.AddPerson(addPersonRequest);

        UpdatePersonRequest? personRequest = (UpdatePersonRequest)addResponse;
        personRequest.PersonName = "Michele";

        //Act
        var updatedResponse = _personsService.UpdatePerson(personRequest);

        var expected = personRequest;
        var getResponse = _personsService.GetPersonById(personRequest.PersonId);
        var actual = (UpdatePersonRequest)getResponse;

        //Assert
        Assert.Equal(expected, actual);
    }
    #endregion

    #region DeletePerson
    [Fact]
    public void DeletePerson_Invalid()
    {
        var r = _personsService.DeletePerson(Guid.NewGuid());

        Assert.False(r);
    }

    [Fact]
    public void DeletePerson_valid()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        CountryResponse countryResponse = _countriesService.AddCountry(addCountryRequest);
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
        var addResponse = _personsService.AddPerson(addPersonRequest);

        //Act
        var r = _personsService.DeletePerson(addResponse.PersonId);

        var newList = _personsService.GetPersonList();

        //Assert
        Assert.True(r);
        Assert.DoesNotContain(addResponse, newList);
    }
    #endregion
}
