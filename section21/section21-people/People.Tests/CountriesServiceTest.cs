using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using Moq;
using People.Entities;
using People.ServiceContracts.DTOs;
using People.ServiceContracts.Interfaces;
using People.Services;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Xunit.Abstractions;
using FluentAssertions;
using System.Linq.Expressions;
using People.ServiceContracts.Repositories;

namespace People.Tests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    private readonly Mock<ICountryRepository> _countriesRepMoq;
    private readonly ICountryRepository _countriesRep;
    private readonly ITestOutputHelper _testOutputHelper;

    public CountriesServiceTest(ITestOutputHelper testOutputHelper)
    {
        //var countries = new List<Country>();
        //new List<Country>() {
        // new Country() { CountryId = Guid.NewGuid() , CountryName="Iran"},
        // new Country() { CountryId = Guid.NewGuid(), CountryName="Italy" } };
        //var appDbContext = Create.MockedDbContextFor<AppDbContext>();
        //_countriesService = new CountriesService(appDbContext);

        _countriesRepMoq = new Mock<ICountryRepository>();
        _countriesRep = _countriesRepMoq.Object;
        _countriesService = new CountriesService(_countriesRep);

        _testOutputHelper = testOutputHelper;
    }

    #region AddCountry
    //when AddCountryRequest is null => ArgumentNullException
    [Fact]
    public async Task AddCountry_NullCountry()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = null;

        var method = async () =>
        {
            //Act
            await _countriesService.AddCountry(addCountryRequest);
        };

        //Assert
        method.Should().ThrowAsync<ArgumentNullException>();
        //await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        //{
        //    //Act
        //    await _countriesService.AddCountry(addCountryRequest);
        //});
    }


    //when AddCountryRequest.CountryName is null => ArgumentException
    [Fact]
    public async Task AddCountry_CountryNameIsNull()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = null };

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _countriesService.AddCountry(addCountryRequest);
        });
    }

    //when AddCountryRequest.CountryName is duplcate => ArgumentException
    [Fact]
    public async Task AddCountry_DuplicateCountryName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest1 = new() { CountryName = "Iran" };
        //AddCountryRequest? addCountryRequest2 = new() { CountryName = "Iran" };

        Country country = (Country)addCountryRequest1;
        List<Country> countryList = new() { country };
        _countriesRepMoq.Setup(r => r.Get(It.IsAny<Expression<Func<Country, bool>>>()))
    .ReturnsAsync(countryList);

        //Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            //Act
            await _countriesService.AddCountry(addCountryRequest1);
            //await _countriesService.AddCountry(addCountryRequest2);
        });
    }

    //when AddCountryRequest is OK => created country
    [Fact]
    public async Task AddCountry_ProperCountryName()
    {
        //Arrange
        AddCountryRequest? addCountryRequest = new() { CountryName = "Iran" };
        Country country = (Country)addCountryRequest;
        CountryResponse expCountryResponse = (CountryResponse)country;

        _countriesRepMoq.Setup(x=>x.Get(It.IsAny<Expression<Func<Country, bool>>>()))
            .ReturnsAsync(new List<Country>());
        _countriesRepMoq.Setup(x => x.Add(It.IsAny<Country>()))
            .ReturnsAsync(country);
        //Act
        CountryResponse countryResponse = await _countriesService.AddCountry(addCountryRequest);
        //var countryResponses = await _countriesService.GetCountryList();

        //Assert
        //must be equal in all fields except ID
        expCountryResponse.CountryId = countryResponse.CountryId;
        countryResponse.Should().Be(expCountryResponse);

        //Assert.True(countryResponse.CountryName == addCountryRequest.CountryName &&
        //    countryResponse.CountryId != Guid.Empty
        //    );

        //Assert.Contains(countryResponse, countryResponses);  //, new CountryResponseComparer());
    }
    #endregion
    #region GetCountryList
    [Fact]
    public async Task GetCountryList_Empty()
    {
        //Arrange
        List<Country> countryList = new();
        _countriesRepMoq.Setup(r => r.GetAll())
    .ReturnsAsync(countryList);

        //Act
        var actualList = await _countriesService.GetCountryList();

        //Assert
        Assert.Empty(actualList);
    }

    [Fact]
    public async Task GetCountryList_Data()
    {
        //Arrange
        List<Country> countries = new List<Country>()
        {
            new Country(){ CountryName="Iran", CountryId = Guid.NewGuid()},
            new Country(){ CountryName="India", CountryId = Guid.NewGuid()},
            new Country(){ CountryName="Turkey", CountryId = Guid.NewGuid()}
        };
        List<CountryResponse> countryResponses = countries.
            Select(x => (CountryResponse)x).ToList();

        _countriesRepMoq.Setup(x => x.GetAll()).ReturnsAsync(countries);


        //Act
        var actualList = await _countriesService.GetCountryList();

        //Expected
        _testOutputHelper.WriteLine("Expected:");
        foreach (var item in countryResponses)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }
        //Actual
        _testOutputHelper.WriteLine("Actual:");
        foreach (var item in actualList)
        {
            _testOutputHelper.WriteLine(item.ToString());
        }

        //Assert
        actualList.Should().BeEquivalentTo(countryResponses);
    }
    #endregion
    #region GetCountry
    [Fact]
    public async Task GetCountry_Null()
    {
        //Arrange
        Guid? guid = null;

        //Act
        var actual = await _countriesService.GetCountry(guid);

        //Assert
        Assert.Null(actual);
    }
    [Fact]
    public async Task GetCountry_WrongId()
    {
        //Arrange
        Guid guid = Guid.NewGuid();

        List<Country> countryList = new();
        _countriesRepMoq.Setup(r => r.Get(It.IsAny<Expression<Func<Country, bool>>>()))
    .ReturnsAsync(countryList);

        //Act
        var actual = await _countriesService.GetCountry(guid);

        //Assert
        Assert.Null(actual);
    }
    [Fact]
    public async Task GetCountry_Id()
    {
        //Arrange
        Country country = new Country() { CountryName = "Iran", CountryId = Guid.NewGuid() };
        CountryResponse countryResponse = (CountryResponse)country;

        List<Country> countryList = new() { country };
        _countriesRepMoq.Setup(r => r.Get(It.IsAny<Expression<Func<Country, bool>>>()))
    .ReturnsAsync(countryList);

        //Act
        var actual = await _countriesService.GetCountry(countryResponse.CountryId);

        //Assert
        actual.Should().Be(countryResponse);
    }
    #endregion
}

public class CountryResponseComparer : IEqualityComparer<CountryResponse>
{
    public bool Equals(CountryResponse? x, CountryResponse? y)
    {
        return (x?.CountryName == y?.CountryName && x?.CountryId == y?.CountryId);
    }

    public int GetHashCode([DisallowNull] CountryResponse obj)
    {
        return (obj.CountryName, obj.CountryId).GetHashCode();
    }
}