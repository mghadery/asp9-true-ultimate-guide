using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using People.Entities;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Enums;
using People.ServiceContracts.Interfaces;
using People.Services;
using Xunit.Abstractions;
using FluentAssertions;
using Moq;
using People.ServiceContracts.Repositories;

namespace People.Tests;

public class PersonsServiceTest
{
    //private readonly ICountriesService _countriesService;

    private readonly IPersonsService _personsService;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly Bogus.Faker<AddPersonRequest> _personReqFaker;
    //private readonly Bogus.Faker<AddCountryRequest> _countryReqFaker;
    private readonly Mock<IPersonRepository> mock;
    private readonly IPersonRepository repository;

    public PersonsServiceTest(ITestOutputHelper testOutputHelper)
    {
        //AppDbContext appDbContext = Create.MockedDbContextFor<AppDbContext>();
        mock = new Mock<IPersonRepository>();
        repository = mock.Object;

        //_countriesService = new CountriesService(null);
        _personsService = new PersonsService(repository);
        _testOutputHelper = testOutputHelper;
        _personReqFaker = new Bogus.Faker<AddPersonRequest>()
            .RuleFor(p => p.Address, f => f.Address.FullAddress())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.GenderOptions, f => f.PickRandom<GenderOptions>())
            .RuleFor(p => p.PersonName, f => f.Name.FullName())
            .RuleFor(p => p.CountryId, f => f.Random.Guid());

        //_countryReqFaker = new Bogus.Faker<AddCountryRequest>()
        //    .RuleFor(c => c.CountryName, f => f.Address.County());
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
        AddPersonRequest? addPersonRequest = _personReqFaker.Generate();
        Person person = (Person)addPersonRequest;
        PersonResponse personResponse = (PersonResponse)person;
        mock.Setup(x => x.Add(It.IsAny<Person>())).ReturnsAsync(person);

        //Act
        var actualResponse = await _personsService.AddPerson(addPersonRequest);

        //Assert
        personResponse.PersonId = actualResponse.PersonId;
        actualResponse.PersonId.Should().NotBe(Guid.Empty);
        actualResponse.Should().Be(personResponse);
    }
    #endregion

    #region GetPersonList
    [Fact]
    public async Task GetPersonList_Valid()
    {
        //Arrange
        Person person1 = new Person()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            Gender = ServiceContracts.Enums.GenderOptions.Male.ToString(),
            ReceiveNewsLetters = true,
            Country = null
        };
        //Arrange
        Person person2 = new Person()
        {
            PersonName = "Hasan",
            Address = "Street 2",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "hasan@gmail.com",
            Gender = ServiceContracts.Enums.GenderOptions.Male.ToString(),
            ReceiveNewsLetters = true,
            Country = null
        };
        List<Person> persons = new() { person1, person2 };
        List<PersonResponse> personResponses = persons.Select(x => (PersonResponse)x).ToList();
        mock.Setup(x => x.GetAll()).ReturnsAsync(persons);

        //Action
        var actual = await _personsService.GetPersonList();


        //Assert
        actual.Should().BeEquivalentTo(personResponses);
    }
    #endregion

    #region GetPersonById
    [Fact]
    public async Task GetPersonById_Valid()
    {
        //Arrange
        Person person = new Person()
        {
            PersonName = "Ali",
            Address = "Street 1",
            DateOfBirth = DateTime.Parse("2000-01-01"),
            Email = "ali@gmail.com",
            Gender = ServiceContracts.Enums.GenderOptions.Male.ToString(),
            ReceiveNewsLetters = true,
            Country = null
        };
        PersonResponse personResponse=(PersonResponse)person;
        mock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(person);

        //Act
        var actual = await _personsService.GetPersonById(person.PersonId);
        //expected.Country = "qaz";

        //Assert
        actual.Should().Be(personResponse);
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


    #endregion

}
